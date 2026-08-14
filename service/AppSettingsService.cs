using System;
using System.IO;
using System.Text.Json;
using _180Detection.Models;

namespace _180Detection.Services
{
    public sealed class AppSettingsService
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public string RootDirectory { get; }
        public string ConfigPath { get; }

        public AppSettingsService()
        {
            RootDirectory = FindApplicationRoot();
            ConfigPath = Path.Combine(RootDirectory, "config", "appsettings.json");
        }

        public AppSettings Load()
        {
            try
            {
                EnsureDefaultFile();
                string json = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<AppSettings>(json, _jsonOptions)
                    ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public void Save(AppSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            File.WriteAllText(
                ConfigPath,
                JsonSerializer.Serialize(settings, _jsonOptions));
        }

        public string ResolvePath(string configuredPath)
        {
            if (string.IsNullOrWhiteSpace(configuredPath))
                return string.Empty;

            string expanded = Environment.ExpandEnvironmentVariables(configuredPath.Trim());
            return Path.IsPathRooted(expanded)
                ? Path.GetFullPath(expanded)
                : Path.GetFullPath(Path.Combine(RootDirectory, expanded));
        }

        public string EngineDirectory(AppSettings settings)
        {
            return ResolvePath(settings.EngineDirectory);
        }

        public string ProductsRoot(AppSettings settings)
        {
            return ResolvePath(settings.ProductsRoot);
        }

        public string RecordsRoot(AppSettings settings)
        {
            return ResolvePath(settings.RecordsRoot);
        }

        public string ImagesRoot(AppSettings settings)
        {
            return ResolvePath(settings.ImagesRoot);
        }

        public string LogsRoot(AppSettings settings)
        {
            return ResolvePath(settings.LogsRoot);
        }

        public string ProductConfigPath(AppSettings settings)
        {
            return ResolvePath(settings.ProductConfigPath);
        }

        private void EnsureDefaultFile()
        {
            if (File.Exists(ConfigPath))
                return;

            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            Save(new AppSettings());
        }

        private static string FindApplicationRoot()
        {
            DirectoryInfo current = new DirectoryInfo(AppContext.BaseDirectory);
            for (int i = 0; i < 8 && current != null; i++)
            {
                if (File.Exists(Path.Combine(current.FullName, "180Detection.csproj")))
                    return current.FullName;

                current = current.Parent;
            }

            return AppContext.BaseDirectory;
        }
    }
}
