using System;

namespace _180Detection.Models
{
    /// <summary>
    /// UI 与 ONNX Runtime 推理层之间的统一结果。
    /// </summary>
    public class DetectionResult
    {
        public string ProductName { get; set; } = string.Empty;
        public DateTime RecordTime { get; set; }

        public string ImagePath { get; set; } = string.Empty;
        public string MarkedImagePath { get; set; } = string.Empty;
        public string HeatmapPath { get; set; } = string.Empty;

        public string Decision { get; set; } = "UNCALIBRATED";
        public bool IsNg { get; set; }
        public string FinalResult { get; set; } = string.Empty;
        public string DefectClass { get; set; } = string.Empty;

        public double AnomalyScore { get; set; }
        public double Similarity { get; set; }
        public double Margin { get; set; }

        public bool HasBbox { get; set; }
        public int BboxX { get; set; }
        public int BboxY { get; set; }
        public int BboxWidth { get; set; }
        public int BboxHeight { get; set; }

        public long ElapsedMilliseconds { get; set; }
    }
}
