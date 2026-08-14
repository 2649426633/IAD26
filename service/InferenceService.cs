using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using _180Detection.Models;

namespace _180Detection.Services
{
    /// <summary>
    /// WinForms 与 Python 推理脚本之间的边界。
    /// 当前版本每次检测启动一次 Python，后续可无缝替换为常驻推理服务。
    /// </summary>
    public sealed class InferenceService
    {
        private readonly JavaScriptSerializer _json = new JavaScriptSerializer();

        public string PythonExecutable { get; private set; }
        public string ScriptPath { get; private set; }
        public string WorkingDirectory { get; private set; }
        public string ResultDirectory { get; private set; }
        public string ArgumentsTemplate { get; private set; }
        public int TimeoutMilliseconds { get; private set; }

        public bool IsConfigured
        {
            get
            {
                return IsExecutableConfigured(PythonExecutable) &&
                       !string.IsNullOrWhiteSpace(ScriptPath) &&
                       File.Exists(ScriptPath);
            }
        }

        private InferenceService()
        {
        }

        public static InferenceService FromConfiguration()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string script = ResolveConfiguredPath(
                ConfigurationManager.AppSettings["InferenceScript"],
                baseDirectory);
            string workingDirectory = ResolveConfiguredPath(
                ConfigurationManager.AppSettings["InferenceWorkingDirectory"],
                baseDirectory);
            string resultDirectory = ResolveConfiguredPath(
                ConfigurationManager.AppSettings["InferenceResultDirectory"],
                baseDirectory);

            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                workingDirectory = !string.IsNullOrWhiteSpace(script)
                    ? Path.GetDirectoryName(script)
                    : baseDirectory;
            }

            if (string.IsNullOrWhiteSpace(resultDirectory))
                resultDirectory = Path.Combine(baseDirectory, "runtime", "results");

            int timeoutSeconds;
            if (!int.TryParse(ConfigurationManager.AppSettings["InferenceTimeoutSeconds"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out timeoutSeconds) ||
                timeoutSeconds <= 0)
            {
                timeoutSeconds = 120;
            }

            string argumentsTemplate = ConfigurationManager.AppSettings["InferenceArgumentsTemplate"];
            if (string.IsNullOrWhiteSpace(argumentsTemplate))
            {
                argumentsTemplate =
                    "\"{script}\" --image \"{image}\" --product \"{product}\" --output \"{output}\"";
            }

            return new InferenceService
            {
                PythonExecutable = string.IsNullOrWhiteSpace(
                    ConfigurationManager.AppSettings["PythonExecutable"])
                    ? "python"
                    : ConfigurationManager.AppSettings["PythonExecutable"].Trim(),
                ScriptPath = script,
                WorkingDirectory = workingDirectory,
                ResultDirectory = resultDirectory,
                ArgumentsTemplate = argumentsTemplate,
                TimeoutMilliseconds = timeoutSeconds * 1000
            };
        }

        public string GetConfigurationStatus()
        {
            if (!IsExecutableConfigured(PythonExecutable))
                return "Python 未配置";
            if (string.IsNullOrWhiteSpace(ScriptPath))
                return "推理脚本未配置";
            if (!File.Exists(ScriptPath))
                return "推理脚本不存在";
            return "推理已配置";
        }

        public Task<DetectionResult> InspectAsync(string imagePath, string productName)
        {
            return Task.Run(() => Inspect(imagePath, productName));
        }

        private DetectionResult Inspect(string imagePath, string productName)
        {
            if (!IsConfigured)
                throw new InvalidOperationException("Python 推理尚未配置完整，请先设置解释器和推理脚本。");
            if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                throw new FileNotFoundException("待检测图片不存在。", imagePath);

            Directory.CreateDirectory(ResultDirectory);

            string outputPath = Path.Combine(
                ResultDirectory,
                DateTime.Now.ToString("yyyyMMdd_HHmmss_fff", CultureInfo.InvariantCulture) +
                "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + "_result.json");

            string arguments = ArgumentsTemplate
                .Replace("{script}", EscapeArgumentValue(ScriptPath))
                .Replace("{image}", EscapeArgumentValue(Path.GetFullPath(imagePath)))
                .Replace("{product}", EscapeArgumentValue(productName ?? string.Empty))
                .Replace("{output}", EscapeArgumentValue(outputPath));

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = PythonExecutable,
                Arguments = arguments,
                WorkingDirectory = WorkingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            StringBuilder standardOutput = new StringBuilder();
            StringBuilder standardError = new StringBuilder();
            Stopwatch stopwatch = Stopwatch.StartNew();

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    if (e.Data != null)
                        standardOutput.AppendLine(e.Data);
                };
                process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    if (e.Data != null)
                        standardError.AppendLine(e.Data);
                };

                try
                {
                    if (!process.Start())
                        throw new InvalidOperationException("无法启动 Python 推理进程。");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        "启动 Python 失败，请检查 PythonExecutable 配置。", ex);
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (!process.WaitForExit(TimeoutMilliseconds))
                {
                    try { process.Kill(); } catch { }
                    throw new TimeoutException(
                        "Python 推理超时，超过 " +
                        (TimeoutMilliseconds / 1000).ToString(CultureInfo.InvariantCulture) +
                        " 秒。");
                }

                process.WaitForExit();
                stopwatch.Stop();

                if (process.ExitCode != 0)
                {
                    string errorText = standardError.ToString().Trim();
                    if (string.IsNullOrWhiteSpace(errorText))
                        errorText = standardOutput.ToString().Trim();
                    throw new InvalidOperationException(
                        "Python 推理失败，ExitCode=" + process.ExitCode +
                        (string.IsNullOrWhiteSpace(errorText) ? string.Empty : "\r\n" + errorText));
                }
            }

            string jsonText;
            if (File.Exists(outputPath))
            {
                jsonText = File.ReadAllText(outputPath, Encoding.UTF8);
            }
            else
            {
                jsonText = standardOutput.ToString().Trim();
                if (string.IsNullOrWhiteSpace(jsonText) || !jsonText.StartsWith("{"))
                {
                    throw new InvalidOperationException(
                        "Python 已执行完成，但没有生成结果 JSON：" + outputPath);
                }
            }

            DetectionResult result = ParseResult(jsonText);
            result.ImagePath = string.IsNullOrWhiteSpace(result.ImagePath)
                ? Path.GetFullPath(imagePath)
                : ResolveResultPath(result.ImagePath, outputPath);
            result.MarkedImagePath = ResolveResultPath(result.MarkedImagePath, outputPath);
            result.HeatmapPath = ResolveResultPath(result.HeatmapPath, outputPath);

            if (result.ElapsedMilliseconds <= 0)
                result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            return result;
        }

        private DetectionResult ParseResult(string jsonText)
        {
            object parsed = _json.DeserializeObject(jsonText);
            Dictionary<string, object> root = parsed as Dictionary<string, object>;
            if (root == null)
                throw new InvalidOperationException("Python 返回的 JSON 不是对象格式。");

            object nested;
            if (root.TryGetValue("result", out nested))
            {
                Dictionary<string, object> nestedResult = nested as Dictionary<string, object>;
                if (nestedResult != null)
                    root = nestedResult;
            }

            DetectionResult result = new DetectionResult();
            result.ImagePath = GetString(root, "image_path", "imagePath", "ImagePath", "image");
            result.MarkedImagePath = GetString(
                root, "marked_image_path", "markedImagePath", "MarkedImagePath", "marked");
            result.HeatmapPath = GetString(
                root, "heatmap_path", "heatmapPath", "HeatmapPath", "heatmap");
            result.DefectClass = GetString(
                root, "defect_class", "defectClass", "DefectClass", "class_name", "class");
            result.AnomalyScore = GetDouble(
                root, "anomaly_score", "anomalyScore", "AnomalyScore", "patchcore_score", "score");
            result.Similarity = GetDouble(
                root, "similarity", "Similarity", "dino_similarity", "classification_similarity");
            result.Margin = GetDouble(root, "margin", "Margin");
            result.ElapsedMilliseconds = GetLong(
                root, "elapsed_ms", "elapsedMilliseconds", "ElapsedMilliseconds", "duration_ms");

            object ngValue = GetValue(root, "is_ng", "isNg", "IsNg");
            if (ngValue != null)
            {
                result.IsNg = ConvertToBoolean(ngValue);
            }
            else
            {
                string status = GetString(root, "status", "result_status", "decision");
                result.IsNg =
                    string.Equals(status, "NG", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(status, "FAIL", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(status, "ANOMALY", StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }

        private string ResolveResultPath(string value, string outputJsonPath)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string trimmed = value.Trim();
            if (Path.IsPathRooted(trimmed))
                return trimmed;

            string resultBase = Path.GetDirectoryName(outputJsonPath);
            string fromResultDirectory = Path.GetFullPath(Path.Combine(resultBase, trimmed));
            if (File.Exists(fromResultDirectory))
                return fromResultDirectory;

            return Path.GetFullPath(Path.Combine(WorkingDirectory, trimmed));
        }

        private static string ResolveConfiguredPath(string configuredValue, string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(configuredValue))
                return string.Empty;

            string value = Environment.ExpandEnvironmentVariables(configuredValue.Trim());
            if (Path.IsPathRooted(value))
                return Path.GetFullPath(value);

            return Path.GetFullPath(Path.Combine(baseDirectory, value));
        }

        private static bool IsExecutableConfigured(string executable)
        {
            if (string.IsNullOrWhiteSpace(executable))
                return false;

            if (executable.IndexOf(Path.DirectorySeparatorChar) >= 0 ||
                executable.IndexOf(Path.AltDirectorySeparatorChar) >= 0 ||
                Path.IsPathRooted(executable))
            {
                return File.Exists(executable);
            }

            return true;
        }

        private static string EscapeArgumentValue(string value)
        {
            return (value ?? string.Empty).Replace("\"", "\\\"");
        }

        private static object GetValue(Dictionary<string, object> values, params string[] keys)
        {
            foreach (string key in keys)
            {
                object value;
                if (values.TryGetValue(key, out value))
                    return value;
            }
            return null;
        }

        private static string GetString(Dictionary<string, object> values, params string[] keys)
        {
            object value = GetValue(values, keys);
            return value == null
                ? string.Empty
                : Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private static double GetDouble(Dictionary<string, object> values, params string[] keys)
        {
            object value = GetValue(values, keys);
            if (value == null)
                return 0D;

            try
            {
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                double parsed;
                return double.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture),
                    NumberStyles.Float, CultureInfo.InvariantCulture, out parsed)
                    ? parsed
                    : 0D;
            }
        }

        private static long GetLong(Dictionary<string, object> values, params string[] keys)
        {
            object value = GetValue(values, keys);
            if (value == null)
                return 0L;

            try
            {
                return Convert.ToInt64(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                long parsed;
                return long.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture),
                    NumberStyles.Integer, CultureInfo.InvariantCulture, out parsed)
                    ? parsed
                    : 0L;
            }
        }

        private static bool ConvertToBoolean(object value)
        {
            if (value is bool)
                return (bool)value;

            string text = Convert.ToString(value, CultureInfo.InvariantCulture);
            if (string.Equals(text, "1", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(text, "true", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(text, "yes", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(text, "ng", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(text, "anomaly", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
