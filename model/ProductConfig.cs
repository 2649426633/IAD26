namespace _180Detection.Models
{
    public sealed class ProductConfig
    {
        public string Name { get; set; }
        public string PatchCoreModelDirectory { get; set; }
        public string DefectBankDirectory { get; set; }
        public double AnomalyThreshold { get; set; }
        public double SimilarityThreshold { get; set; }
        public bool Enabled { get; set; }
    }
}
