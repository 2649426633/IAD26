using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using _180Detection.Models;

namespace _180Detection.Services
{
    public sealed class ProductConfigService
    {
        private readonly JavaScriptSerializer _json = new JavaScriptSerializer();

        public string ConfigPath { get; private set; }

        public ProductConfigService()
        {
            string configured = ConfigurationManager.AppSettings["ProductConfigPath"];
            if (string.IsNullOrWhiteSpace(configured))
                configured = @"runtime\config\products.json";

            string expanded = Environment.ExpandEnvironmentVariables(configured.Trim());
            ConfigPath = Path.IsPathRooted(expanded)
                ? Path.GetFullPath(expanded)
                : Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, expanded));
        }

        public List<ProductConfig> Load()
        {
            try
            {
                EnsureDefaultFile();
                string text = File.ReadAllText(ConfigPath, Encoding.UTF8);
                List<ProductConfig> products = _json.Deserialize<List<ProductConfig>>(text);
                return products ?? new List<ProductConfig>();
            }
            catch
            {
                return CreateDefaultProducts();
            }
        }

        public void Save(IList<ProductConfig> products)
        {
            if (products == null)
                throw new ArgumentNullException("products");

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
                    PatchCoreModelDirectory = (source.PatchCoreModelDirectory ?? string.Empty).Trim(),
                    DefectBankDirectory = (source.DefectBankDirectory ?? string.Empty).Trim(),
                    AnomalyThreshold = Math.Max(0D, source.AnomalyThreshold),
                    SimilarityThreshold = Math.Max(0D, source.SimilarityThreshold),
                    Enabled = source.Enabled
                });
            }

            if (normalized.Count == 0)
                throw new InvalidOperationException("至少保留一个产品配置。");

            string directory = Path.GetDirectoryName(ConfigPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(ConfigPath, _json.Serialize(normalized), new UTF8Encoding(false));
        }

        public string[] GetEnabledProductNames()
        {
            List<string> names = new List<string>();
            foreach (ProductConfig product in Load())
            {
                if (product != null && product.Enabled && !string.IsNullOrWhiteSpace(product.Name))
                    names.Add(product.Name.Trim());
            }

            if (names.Count == 0)
                names.Add("Phone");

            return names.ToArray();
        }

        private void EnsureDefaultFile()
        {
            if (File.Exists(ConfigPath))
                return;

            string directory = Path.GetDirectoryName(ConfigPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(
                ConfigPath,
                _json.Serialize(CreateDefaultProducts()),
                new UTF8Encoding(false));
        }

        private static List<ProductConfig> CreateDefaultProducts()
        {
            return new List<ProductConfig>
            {
                new ProductConfig
                {
                    Name = "Phone",
                    PatchCoreModelDirectory = string.Empty,
                    DefectBankDirectory = string.Empty,
                    AnomalyThreshold = 0.5D,
                    SimilarityThreshold = 0.8D,
                    Enabled = true
                }
            };
        }
    }
}
