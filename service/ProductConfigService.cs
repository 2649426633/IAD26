using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using _180Detection.Models;

namespace _180Detection.Services
{
    public sealed class ProductConfigService
    {
        private readonly AppSettingsService _settingsService;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public string ConfigPath
        {
            get
            {
                AppSettings settings = _settingsService.Load();
                return _settingsService.ProductConfigPath(settings);
            }
        }

        public ProductConfigService()
            : this(new AppSettingsService())
        {
        }

        public ProductConfigService(AppSettingsService settingsService)
        {
            _settingsService = settingsService
                ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public List<ProductConfig> Load()
        {
            try
            {
                EnsureDefaultFile();
                string text = File.ReadAllText(ConfigPath);
                List<ProductConfig> products =
                    JsonSerializer.Deserialize<List<ProductConfig>>(text, _jsonOptions);

                if (products == null)
                    return CreateDefaultProducts();

                bool legacyPythonConfig =
                    text.IndexOf(
                        "PatchCoreModelDirectory",
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    text.IndexOf(
                        "DefectBankDirectory",
                        StringComparison.OrdinalIgnoreCase) >= 0;

                if (legacyPythonConfig)
                {
                    foreach (ProductConfig product in products)
                    {
                        if (product == null)
                            continue;

                        if (string.IsNullOrWhiteSpace(product.ProductDirectory))
                        {
                            product.ProductDirectory = Path.Combine(
                                "products",
                                (product.Name ?? string.Empty)
                                    .Trim()
                                    .ToLowerInvariant());
                        }

                        // 旧版 0.5 是占位参数，不自动当作生产标定阈值。
                        product.AnomalyThreshold = null;
                    }

                    Save(products);
                }

                return products;
            }
            catch
            {
                return CreateDefaultProducts();
            }
        }

        public void Save(IList<ProductConfig> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            List<ProductConfig> normalized = new List<ProductConfig>();
            HashSet<string> names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (ProductConfig source in products)
            {
                if (source == null || string.IsNullOrWhiteSpace(source.Name))
                    continue;

                string name = source.Name.Trim();
                if (!names.Add(name))
                    throw new InvalidOperationException("产品名称不能重复：" + name);

                normalized.Add(new ProductConfig
                {
                    Name = name,
                    ProductDirectory = (source.ProductDirectory ?? string.Empty).Trim(),
                    AnomalyThreshold = source.AnomalyThreshold,
                    Enabled = source.Enabled
                });
            }

            if (normalized.Count == 0)
                throw new InvalidOperationException("至少保留一个产品配置。");

            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            File.WriteAllText(
                ConfigPath,
                JsonSerializer.Serialize(normalized, _jsonOptions));
        }

        public ProductConfig GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return Load().FirstOrDefault(p =>
                p != null &&
                string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public string[] GetEnabledProductNames()
        {
            string[] names = Load()
                .Where(p => p != null && p.Enabled && !string.IsNullOrWhiteSpace(p.Name))
                .Select(p => p.Name.Trim())
                .ToArray();

            return names.Length == 0 ? new[] { "Phone" } : names;
        }

        public string ResolveProductDirectory(ProductConfig product)
        {
            if (product == null)
                return string.Empty;

            AppSettings settings = _settingsService.Load();
            string configured = product.ProductDirectory;

            if (string.IsNullOrWhiteSpace(configured))
            {
                configured = Path.Combine(
                    settings.ProductsRoot ?? "products",
                    (product.Name ?? string.Empty).Trim().ToLowerInvariant());
            }

            return _settingsService.ResolvePath(configured);
        }

        private void EnsureDefaultFile()
        {
            if (File.Exists(ConfigPath))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            File.WriteAllText(
                ConfigPath,
                JsonSerializer.Serialize(CreateDefaultProducts(), _jsonOptions));
        }

        private static List<ProductConfig> CreateDefaultProducts()
        {
            return new List<ProductConfig>
            {
                new ProductConfig
                {
                    Name = "Phone",
                    ProductDirectory = @"products\phone",
                    AnomalyThreshold = null,
                    Enabled = true
                }
            };
        }
    }
}
