using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace QemuLauncher
{
    public class QemuConfig
    {
        public List<string> CPUs { get; set; } = new List<string>();
        public List<string> Machines { get; set; } = new List<string>();
        public List<string> VideoCards { get; set; } = new List<string>();
        public List<string> AudioDevices { get; set; } = new List<string>();
        public List<string> NetworkDevices { get; set; } = new List<string>();

        private const int ProcessTimeoutMs = 5000;
        private const int StreamDrainTimeoutMs = 1000;

        public static QemuConfig LoadOrCreate(string qemuPath)
        {
            var config = new QemuConfig();
            if (!File.Exists(qemuPath)) return config;

            // Видео (список для -vga)
            string vgaOutput = RunQemuOutput(qemuPath, "-vga help");
            config.VideoCards = ParseFirstWords(vgaOutput,
                stopAt: null,
                exactSkip: new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "none" },
                prefixSkip: new[] { "available", "valid" });

            // Аудио (список для -audiodev)
            string audioOutput = RunQemuOutput(qemuPath, "-audiodev help");
            config.AudioDevices = ParseFirstWords(audioOutput,
                stopAt: null,
                exactSkip: null,
                prefixSkip: new[] { "available", "valid" });

            // Сеть (модели для -nic model=...)
            string nicOutput = RunQemuOutput(qemuPath, "-nic help");
            config.NetworkDevices = ParseNicModels(nicOutput);

            // CPU — останавливаемся перед секцией CPUID-флагов
            string cpuOutput = RunQemuOutput(qemuPath, "-cpu help");
            config.CPUs = ParseFirstWords(cpuOutput,
                stopAt: "Recognized CPUID flags:",
                exactSkip: new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "none" },
                prefixSkip: new[] { "available", "supported", "note:", "x86" });

            // Машины
            string machineOutput = RunQemuOutput(qemuPath, "-machine help");
            config.Machines = ParseFirstWords(machineOutput,
                stopAt: null,
                exactSkip: new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "none" },
                prefixSkip: new[] { "available", "supported", "note:", "(default)" });

            return config;
        }

        // ----------------------------------------------------------------
        // Запуск процесса и получение объединённого stdout + stderr.
        // Потоки читаются асинхронно во избежание deadlock при заполнении буфера.
        // ----------------------------------------------------------------
        private static string RunQemuOutput(string qemuPath, string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = qemuPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                // Явная кодировка, чтобы не получить кракозябры на Windows (OEM → UTF-8)
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            try
            {
                using (var proc = Process.Start(psi))
                {
                    if (proc == null) return string.Empty;

                    var stdout = new StringBuilder();
                    var stderr = new StringBuilder();

                    // AutoResetEvent сигнализирует об окончании каждого потока
                    using (var outDone = new AutoResetEvent(false))
                    using (var errDone = new AutoResetEvent(false))
                    {
                        proc.OutputDataReceived += (s, e) =>
                        {
                            if (e.Data == null) outDone.Set();
                            else stdout.AppendLine(e.Data);
                        };
                        proc.ErrorDataReceived += (s, e) =>
                        {
                            if (e.Data == null) errDone.Set();
                            else stderr.AppendLine(e.Data);
                        };

                        proc.BeginOutputReadLine();
                        proc.BeginErrorReadLine();

                        bool exited = proc.WaitForExit(ProcessTimeoutMs);

                        if (!exited)
                        {
                            TryKill(proc);
                            return string.Empty;
                        }

                        // WaitForExit() без таймаута гарантирует, что асинхронные
                        // обработчики OutputDataReceived/ErrorDataReceived завершились.
                        proc.WaitForExit();

                        // Дополнительное ожидание сигналов на случай медленной записи
                        outDone.WaitOne(StreamDrainTimeoutMs);
                        errDone.WaitOne(StreamDrainTimeoutMs);

                        return stdout.ToString() + stderr.ToString();
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        // ----------------------------------------------------------------
        // Безопасное завершение процесса: игнорируем InvalidOperationException,
        // которое возникает, если процесс уже завершился к моменту вызова Kill().
        // ----------------------------------------------------------------
        private static void TryKill(Process proc)
        {
            try { proc.Kill(); }
            catch (InvalidOperationException) { }
        }

        // ----------------------------------------------------------------
        // Универсальный парсер: берёт первое слово каждой строки,
        // пропуская заголовки/служебные строки и останавливаясь на stopAt.
        //
        //   stopAt      — подстрока, при обнаружении которой парсинг прерывается
        //   exactSkip   — точные слова, которые нужно пропустить (без учёта регистра)
        //   prefixSkip  — префиксы строк, которые нужно пропустить (без учёта регистра)
        // ----------------------------------------------------------------
        private static List<string> ParseFirstWords(
            string output,
            string stopAt,
            HashSet<string> exactSkip,
            string[] prefixSkip)
        {
            // HashSet для O(1)-дедупликации; порядок сохраняем через List
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var result = new List<string>();

            foreach (var line in SplitLines(output))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Проверка признака остановки
                if (stopAt != null &&
                    line.IndexOf(stopAt, StringComparison.OrdinalIgnoreCase) >= 0)
                    break;

                // Пропуск строк по префиксу
                if (prefixSkip != null && StartsWithAny(line, prefixSkip))
                    continue;

                string word = ExtractFirstWord(line);
                if (string.IsNullOrEmpty(word)) continue;

                // Пропуск точных совпадений
                if (exactSkip != null && exactSkip.Contains(word)) continue;

                if (seen.Add(word))
                    result.Add(word);
            }

            return result;
        }

        // ----------------------------------------------------------------
        // Парсер секции "Available NIC models" из вывода -nic help.
        // ----------------------------------------------------------------
        private static List<string> ParseNicModels(string output)
        {
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var result = new List<string>();
            bool inSection = false;

            foreach (var line in SplitLines(output))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (!inSection)
                {
                    if (line.IndexOf("Available NIC models", StringComparison.OrdinalIgnoreCase) >= 0)
                        inSection = true;
                    continue;
                }

                // Новая секция "Available ..." означает конец раздела NIC-моделей
                if (line.IndexOf("Available", StringComparison.OrdinalIgnoreCase) >= 0)
                    break;

                string model = ExtractFirstWord(line);
                if (!string.IsNullOrEmpty(model) && seen.Add(model))
                    result.Add(model);
            }

            return result;
        }

        // ----------------------------------------------------------------
        // Вспомогательные методы
        // ----------------------------------------------------------------

        private static string[] SplitLines(string text) =>
            text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        private static string ExtractFirstWord(string trimmedLine)
        {
            var parts = trimmedLine.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0].Trim('(', ')', '"') : string.Empty;
        }

        private static bool StartsWithAny(string line, string[] prefixes)
        {
            foreach (var prefix in prefixes)
            {
                if (line.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
