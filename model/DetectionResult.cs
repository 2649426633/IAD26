namespace _180Detection.Models
{
    /// <summary>
    /// WinForms 与推理层之间统一使用的最终检测结果。
    /// UI 不关心 PatchCore、FAISS、DINOv2 或 Tile 的内部实现。
    /// </summary>
    public class DetectionResult
    {
        public string ImagePath { get; set; }

        public bool IsNg { get; set; }

        public string DefectClass { get; set; }

        public double AnomalyScore { get; set; }

        public double Similarity { get; set; }

        public double Margin { get; set; }

        public string MarkedImagePath { get; set; }

        public string HeatmapPath { get; set; }

        public long ElapsedMilliseconds { get; set; }
    }
}
