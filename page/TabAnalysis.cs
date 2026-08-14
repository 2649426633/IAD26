using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _180Detection
{
    public partial class TabAnalysis : UserControl
    {
        private const int MaxRecordCount = 1000;

        private Series OkTrendSeries
        {
            get { return chartTrend.Series["seriesOk"]; }
        }

        private Series NgTrendSeries
        {
            get { return chartTrend.Series["seriesNg"]; }
        }

        private Series DistributionSeries
        {
            get { return chartDistribution.Series["seriesDistribution"]; }
        }

        public TabAnalysis()
        {
            InitializeComponent();
            ShowEmptyDistribution();
        }

        /// <summary>
        /// 更新产能汇总。该方法可从采集或检测工作线程调用。
        /// </summary>
        public void UpdateSummary(int total, int ok, int ng, double averageDurationMs)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int, int, int, double>(UpdateSummary),
                    total, ok, ng, averageDurationMs);
                return;
            }

            total = Math.Max(0, total);
            ok = Math.Max(0, ok);
            ng = Math.Max(0, ng);

            lblTotalValue.Text = total.ToString("N0");
            lblOkValue.Text = ok.ToString("N0");
            lblNgValue.Text = ng.ToString("N0");
            lblYieldValue.Text = total == 0
                ? "0.00%"
                : ((double)ok / total).ToString("P2");
            lblDurationValue.Text = averageDurationMs <= 0
                ? "-- ms"
                : averageDurationMs.ToString("0.0") + " ms";

            UpdateDistribution(ok, ng);
            lblLastUpdate.Text = "最后更新：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 更新趋势图。三个数组必须长度一致，标签通常为日期、小时或批次号。
        /// </summary>
        public void SetTrendData(string[] labels, int[] okCounts, int[] ngCounts)
        {
            if (labels == null) throw new ArgumentNullException(nameof(labels));
            if (okCounts == null) throw new ArgumentNullException(nameof(okCounts));
            if (ngCounts == null) throw new ArgumentNullException(nameof(ngCounts));
            if (labels.Length != okCounts.Length || labels.Length != ngCounts.Length)
                throw new ArgumentException("趋势图标签、OK 数量和 NG 数量必须一一对应。");

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string[], int[], int[]>(SetTrendData),
                    labels, okCounts, ngCounts);
                return;
            }

            OkTrendSeries.Points.Clear();
            NgTrendSeries.Points.Clear();

            for (int i = 0; i < labels.Length; i++)
            {
                OkTrendSeries.Points.AddXY(labels[i], Math.Max(0, okCounts[i]));
                NgTrendSeries.Points.AddXY(labels[i], Math.Max(0, ngCounts[i]));
            }
        }

        /// <summary>
        /// 在明细表顶部加入一条检测记录。
        /// </summary>
        public void AddDetectionRecord(
            DateTime time,
            string productId,
            double frontAngle,
            double rearAngle,
            bool isOk,
            double durationMs)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<DateTime, string, double, double, bool, double>(AddDetectionRecord),
                    time, productId, frontAngle, rearAngle, isOk, durationMs);
                return;
            }

            gridRecords.Rows.Insert(0,
                time.ToString("yyyy-MM-dd HH:mm:ss"),
                string.IsNullOrWhiteSpace(productId) ? "--" : productId,
                frontAngle.ToString("0.00") + "°",
                rearAngle.ToString("0.00") + "°",
                isOk ? "OK" : "NG",
                Math.Max(0, durationMs).ToString("0.0") + " ms");

            int rowIndex = 0;
            DataGridViewCell resultCell = gridRecords.Rows[rowIndex].Cells[colResult.Index];
            resultCell.Style.ForeColor = isOk
                ? Color.FromArgb(31, 157, 85)
                : Color.FromArgb(220, 68, 68);
            resultCell.Style.Font = new Font(gridRecords.Font, FontStyle.Bold);

            while (gridRecords.Rows.Count > MaxRecordCount)
                gridRecords.Rows.RemoveAt(gridRecords.Rows.Count - 1);

            lblRecordCount.Text = "共 " + gridRecords.Rows.Count.ToString("N0") + " 条";
        }

        /// <summary>
        /// 清空分析页中的图表和检测明细。
        /// </summary>
        public void ClearAnalysis()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ClearAnalysis));
                return;
            }

            lblTotalValue.Text = "0";
            lblOkValue.Text = "0";
            lblNgValue.Text = "0";
            lblYieldValue.Text = "0.00%";
            lblDurationValue.Text = "-- ms";
            lblLastUpdate.Text = "最后更新：--";
            lblRecordCount.Text = "共 0 条";
            gridRecords.Rows.Clear();
            OkTrendSeries.Points.Clear();
            NgTrendSeries.Points.Clear();
            ShowEmptyDistribution();
        }

        private void UpdateDistribution(int ok, int ng)
        {
            DistributionSeries.Points.Clear();
            if (ok + ng == 0)
            {
                ShowEmptyDistribution();
                return;
            }

            int okPoint = DistributionSeries.Points.AddXY("OK", ok);
            int ngPoint = DistributionSeries.Points.AddXY("NG", ng);
            DistributionSeries.Points[okPoint].Color = Color.FromArgb(48, 181, 107);
            DistributionSeries.Points[ngPoint].Color = Color.FromArgb(235, 87, 87);
            DistributionSeries.Points[okPoint].Label = "OK  #PERCENT{P1}";
            DistributionSeries.Points[ngPoint].Label = "NG  #PERCENT{P1}";
        }

        private void ShowEmptyDistribution()
        {
            DistributionSeries.Points.Clear();
            int point = DistributionSeries.Points.AddXY("暂无数据", 1);
            DistributionSeries.Points[point].Color = Color.FromArgb(224, 228, 234);
            DistributionSeries.Points[point].Label = "暂无数据";
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
