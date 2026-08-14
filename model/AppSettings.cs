namespace _180Detection.Models
{
    public sealed class AppSettings
    {
        public string EngineDirectory { get; set; } = "engine";
        public string ProductsRoot { get; set; } = "products";
        public string RecordsRoot { get; set; } = "records";
        public string ImagesRoot { get; set; } = "images";
        public string LogsRoot { get; set; } = "logs";
        public string ProductConfigPath { get; set; } = @"runtime\config\products.json";
        public string HikCameraSdkAssembly { get; set; } = string.Empty;
        public string CameraExpectedModel { get; set; } = "MV-CS200-10GM";
        public bool SaveOriginalImage { get; set; } = true;
        public bool SaveMarkedImage { get; set; } = true;
    }
}
