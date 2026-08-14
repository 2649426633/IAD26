using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using IndustrialAnomaly.Runtime;
using _180Detection.Models;

namespace _180Detection.Services
{
    /// <summary>
    /// WinForms 与 IndustrialAnomaly.Runtime 之间的唯一推理边界。
    /// 不启动 Python，不启动 Console；ONNX Session 在服务生命周期内常驻。
    /// </summary>
    public sealed class OnnxInferenceService : IDisposable
    {
        private readonly object _gate = new object();
        private readonly AppSettingsService _settingsService;
        private readonly ProductConfigService _productConfigService;

        private AppSettings _settings;
        private OnnxFeatureEngine _featureEngine;
        private ProductModel _productModel;
        private IndustrialAnomalyEngine _runtime;
        private string _loadedProductDirectory = string.Empty;

        public OnnxInferenceService(AppSettingsService settingsService)
        {
            _settingsService = settingsService
                ?? throw new ArgumentNullException(nameof(settingsService));
            _productConfigService = new ProductConfigService(_settingsService);
            _settings = _settingsService.Load();
        }

        public bool CanInspect(ProductConfig product, out string status)
        {
            string engineDirectory = _settingsService.EngineDirectory(_settings);
            if (string.IsNullOrWhiteSpace(engineDirectory) ||
                !Directory.Exists(engineDirectory))
            {
                status = "ONNX 引擎目录不存在";
                return false;
            }

            string manifest = Path.Combine(engineDirectory, "engine_config.json");
            if (!File.Exists(manifest))
            {
                status = "engine_config.json 不存在";
                return false;
            }

            if (product == null)
            {
                status = "产品配置不存在";
                return false;
            }

            string productDirectory = _productConfigService.ResolveProductDirectory(product);
            if (string.IsNullOrWhiteSpace(productDirectory) ||
                !File.Exists(Path.Combine(productDirectory, "product_model.json")))
            {
                status = "产品模型不存在";
                return false;
            }

            status = string.Equals(
                _loadedProductDirectory,
                Path.GetFullPath(productDirectory),
                StringComparison.OrdinalIgnoreCase)
                ? "ONNX 模型就绪"
                : "ONNX 引擎已配置";
            return true;
        }

        public Task LoadProductAsync(ProductConfig product)
        {
            return Task.Run(() => LoadProduct(product));
        }

        public Task<DetectionResult> InspectAsync(
            string imagePath,
            ProductConfig product)
        {
            return Task.Run(() => Inspect(imagePath, product));
        }

        public void ReloadSettings()
        {
            lock (_gate)
            {
                DisposeRuntime();
                _settings = _settingsService.Load();
            }
        }

        private void LoadProduct(ProductConfig product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            lock (_gate)
            {
                EnsureFeatureEngine();

                string productDirectory =
                    _productConfigService.ResolveProductDirectory(product);

                if (string.IsNullOrWhiteSpace(productDirectory))
                    throw new InvalidOperationException("产品模型目录未配置。");

                productDirectory = Path.GetFullPath(productDirectory);

                if (_runtime != null &&
                    string.Equals(
                        _loadedProductDirectory,
                        productDirectory,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                _productModel = ProductModel.Load(productDirectory);
                _runtime = new IndustrialAnomalyEngine(
                    _featureEngine,
                    _productModel);
                _loadedProductDirectory = productDirectory;
            }
        }

        private DetectionResult Inspect(
            string imagePath,
            ProductConfig product)
        {
            if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                throw new FileNotFoundException("待检测图片不存在。", imagePath);

            Stopwatch stopwatch = Stopwatch.StartNew();

            lock (_gate)
            {
                LoadProduct(product);

                DateTime recordTime = DateTime.Now;
                string recordId =
                    recordTime.ToString("yyyyMMdd_HHmmss_fff") + "_" +
                    Guid.NewGuid().ToString("N").Substring(0, 8);

                string recordsRoot = _settingsService.RecordsRoot(_settings);
                string recordDirectory = Path.Combine(
                    recordsRoot,
                    recordTime.ToString("yyyyMMdd"),
                    recordId);
                Directory.CreateDirectory(recordDirectory);

                string originalPath = imagePath;
                if (_settings.SaveOriginalImage)
                {
                    string extension = Path.GetExtension(imagePath);
                    if (string.IsNullOrWhiteSpace(extension))
                        extension = ".bmp";

                    originalPath = Path.Combine(
                        recordDirectory,
                        "original" + extension);
                    File.Copy(imagePath, originalPath, true);
                }

                string markedPath = _settings.SaveMarkedImage
                    ? Path.Combine(recordDirectory, "full_marked.jpg")
                    : null;

                float? threshold = product.AnomalyThreshold.HasValue
                    ? (float?)product.AnomalyThreshold.Value
                    : null;

                InspectionPrediction prediction = _runtime.InspectFile(
                    imagePath,
                    markedPath,
                    threshold);

                stopwatch.Stop();

                DetectionResult result = new DetectionResult
                {
                    ProductName = product.Name ?? string.Empty,
                    RecordTime = recordTime,
                    ImagePath = originalPath,
                    MarkedImagePath = markedPath ?? string.Empty,
                    Decision = prediction.AnomalyDecision ?? "UNCALIBRATED",
                    IsNg = string.Equals(
                        prediction.AnomalyDecision,
                        "NG",
                        StringComparison.OrdinalIgnoreCase),
                    FinalResult = prediction.FinalResult ?? string.Empty,
                    DefectClass = prediction.PredictedDefect ?? string.Empty,
                    AnomalyScore = prediction.PatchCoreAnomalyScore,
                    Similarity = prediction.Top1Similarity ?? 0F,
                    Margin = prediction.Margin ?? 0F,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
                };

                if (prediction.Bbox.HasValue)
                {
                    var bbox = prediction.Bbox.Value;
                    result.HasBbox = true;
                    result.BboxX = bbox.X;
                    result.BboxY = bbox.Y;
                    result.BboxWidth = bbox.Width;
                    result.BboxHeight = bbox.Height;
                }

                WriteRecord(
                    result,
                    imagePath,
                    recordId,
                    recordDirectory,
                    product.AnomalyThreshold);

                return result;
            }
        }

        private void EnsureFeatureEngine()
        {
            if (_featureEngine != null)
                return;

            string engineDirectory = _settingsService.EngineDirectory(_settings);
            if (string.IsNullOrWhiteSpace(engineDirectory) ||
                !Directory.Exists(engineDirectory))
            {
                throw new DirectoryNotFoundException(
                    "ONNX 引擎目录不存在：" + engineDirectory);
            }

            _featureEngine = new OnnxFeatureEngine(engineDirectory);
        }

        private static void WriteRecord(
            DetectionResult result,
            string sourceImagePath,
            string recordId,
            string recordDirectory,
            double? anomalyThreshold)
        {
            Dictionary<string, object> payload =
                new Dictionary<string, object>
                {
                    ["schema_version"] = 1,
                    ["record_id"] = recordId,
                    ["record_time"] = result.RecordTime.ToString("O"),
                    ["product"] = result.ProductName,
                    ["status"] = result.Decision,
                    ["is_ng"] = result.IsNg,
                    ["final_result"] = result.FinalResult,
                    ["defect_class"] = result.DefectClass,
                    ["anomaly_score"] = result.AnomalyScore,
                    ["anomaly_threshold"] = anomalyThreshold,
                    ["similarity"] = result.Similarity,
                    ["margin"] = result.Margin,
                    ["elapsed_ms"] = result.ElapsedMilliseconds,
                    ["source_image_path"] = Path.GetFullPath(sourceImagePath),
                    ["image_path"] = result.ImagePath,
                    ["marked_image_path"] = result.MarkedImagePath,
                    ["bbox_x"] = result.HasBbox ? result.BboxX : null,
                    ["bbox_y"] = result.HasBbox ? result.BboxY : null,
                    ["bbox_width"] = result.HasBbox ? result.BboxWidth : null,
                    ["bbox_height"] = result.HasBbox ? result.BboxHeight : null
                };

            string resultPath = Path.Combine(recordDirectory, "result.json");
            File.WriteAllText(
                resultPath,
                JsonSerializer.Serialize(
                    payload,
                    new JsonSerializerOptions { WriteIndented = true }));
        }

        private void DisposeRuntime()
        {
            _runtime = null;
            _productModel = null;
            _loadedProductDirectory = string.Empty;

            if (_featureEngine != null)
            {
                _featureEngine.Dispose();
                _featureEngine = null;
            }
        }

        public void Dispose()
        {
            lock (_gate)
            {
                DisposeRuntime();
            }
        }
    }
}
