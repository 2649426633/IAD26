namespace _180Detection.Models
{
    public sealed class ProductConfig
    {
        public string Name { get; set; } = string.Empty;
        public string ProductDirectory { get; set; } = string.Empty;
        public double? AnomalyThreshold { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
