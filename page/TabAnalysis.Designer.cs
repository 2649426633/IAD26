using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _180Detection
{
    partial class TabAnalysis
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.metricsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.metricTotal = new System.Windows.Forms.Panel();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.metricOk = new System.Windows.Forms.Panel();
            this.lblOkValue = new System.Windows.Forms.Label();
            this.lblOkTitle = new System.Windows.Forms.Label();
            this.metricNg = new System.Windows.Forms.Panel();
            this.lblNgValue = new System.Windows.Forms.Label();
            this.lblNgTitle = new System.Windows.Forms.Label();
            this.metricYield = new System.Windows.Forms.Panel();
            this.lblYieldValue = new System.Windows.Forms.Label();
            this.lblYieldTitle = new System.Windows.Forms.Label();
            this.metricDuration = new System.Windows.Forms.Panel();
            this.lblDurationValue = new System.Windows.Forms.Label();
            this.lblDurationTitle = new System.Windows.Forms.Label();
            this.chartsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panelTrend = new System.Windows.Forms.Panel();
            this.chartTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTrendTitle = new System.Windows.Forms.Label();
            this.panelDistribution = new System.Windows.Forms.Panel();
            this.chartDistribution = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblDistributionTitle = new System.Windows.Forms.Label();
            this.panelRecords = new System.Windows.Forms.Panel();
            this.gridRecords = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFrontAngle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRearAngle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordsHeader = new System.Windows.Forms.Panel();
            this.lblRecordsTitle = new System.Windows.Forms.Label();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.lblPageSubtitle = new System.Windows.Forms.Label();
            this.rootLayout.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.metricsLayout.SuspendLayout();
            this.metricTotal.SuspendLayout();
            this.metricOk.SuspendLayout();
            this.metricNg.SuspendLayout();
            this.metricYield.SuspendLayout();
            this.metricDuration.SuspendLayout();
            this.chartsLayout.SuspendLayout();
            this.panelTrend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).BeginInit();
            this.panelDistribution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDistribution)).BeginInit();
            this.panelRecords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecords)).BeginInit();
            this.recordsHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // rootLayout
            // 
            this.rootLayout.BackColor = System.Drawing.SystemColors.Control;
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.headerPanel, 0, 0);
            this.rootLayout.Controls.Add(this.metricsLayout, 0, 1);
            this.rootLayout.Controls.Add(this.chartsLayout, 0, 2);
            this.rootLayout.Controls.Add(this.panelRecords, 0, 3);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Location = new System.Drawing.Point(0, 0);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.Padding = new System.Windows.Forms.Padding(12);
            this.rootLayout.RowCount = 4;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.rootLayout.Size = new System.Drawing.Size(2363, 1271);
            this.rootLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.SystemColors.Control;
            this.headerPanel.Controls.Add(this.lblPageTitle);
            this.headerPanel.Controls.Add(this.lblPageSubtitle);
            this.headerPanel.Controls.Add(this.lblLastUpdate);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(12, 12);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(2339, 62);
            this.headerPanel.TabIndex = 0;
            this.headerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.headerPanel_Paint);
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPageTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPageTitle.Location = new System.Drawing.Point(18, 9);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(165, 37);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "数据分析";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblLastUpdate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLastUpdate.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblLastUpdate.Location = new System.Drawing.Point(1839, 0);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Padding = new System.Windows.Forms.Padding(0, 0, 18, 0);
            this.lblLastUpdate.Size = new System.Drawing.Size(500, 62);
            this.lblLastUpdate.TabIndex = 2;
            this.lblLastUpdate.Text = "最后更新：--";
            this.lblLastUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // metricsLayout
            // 
            this.metricsLayout.ColumnCount = 5;
            this.metricsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.metricsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.metricsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.metricsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.metricsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.metricsLayout.Controls.Add(this.metricTotal, 0, 0);
            this.metricsLayout.Controls.Add(this.metricOk, 1, 0);
            this.metricsLayout.Controls.Add(this.metricNg, 2, 0);
            this.metricsLayout.Controls.Add(this.metricYield, 3, 0);
            this.metricsLayout.Controls.Add(this.metricDuration, 4, 0);
            this.metricsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metricsLayout.Location = new System.Drawing.Point(12, 84);
            this.metricsLayout.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.metricsLayout.Name = "metricsLayout";
            this.metricsLayout.RowCount = 1;
            this.metricsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.metricsLayout.Size = new System.Drawing.Size(2339, 118);
            this.metricsLayout.TabIndex = 1;
            // 
            // metricTotal
            // 
            this.metricTotal.BackColor = System.Drawing.SystemColors.Control;
            this.metricTotal.Controls.Add(this.lblTotalValue);
            this.metricTotal.Controls.Add(this.lblTotalTitle);
            this.metricTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metricTotal.Location = new System.Drawing.Point(0, 0);
            this.metricTotal.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.metricTotal.Name = "metricTotal";
            this.metricTotal.Size = new System.Drawing.Size(459, 118);
            this.metricTotal.TabIndex = 0;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTotalValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalValue.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotalValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalValue.Location = new System.Drawing.Point(0, 42);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Padding = new System.Windows.Forms.Padding(10, 0, 4, 8);
            this.lblTotalValue.Size = new System.Drawing.Size(459, 76);
            this.lblTotalValue.TabIndex = 0;
            this.lblTotalValue.Text = "0";
            this.lblTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalTitle
            // 
            this.lblTotalTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalTitle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotalTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTotalTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTotalTitle.Name = "lblTotalTitle";
            this.lblTotalTitle.Padding = new System.Windows.Forms.Padding(10, 12, 0, 0);
            this.lblTotalTitle.Size = new System.Drawing.Size(459, 42);
            this.lblTotalTitle.TabIndex = 1;
            this.lblTotalTitle.Text = "检测总数";
            // 
            // metricOk
            // 
            this.metricOk.BackColor = System.Drawing.SystemColors.Control;
            this.metricOk.Controls.Add(this.lblOkValue);
            this.metricOk.Controls.Add(this.lblOkTitle);
            this.metricOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metricOk.Location = new System.Drawing.Point(467, 0);
            this.metricOk.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.metricOk.Name = "metricOk";
            this.metricOk.Size = new System.Drawing.Size(459, 118);
            this.metricOk.TabIndex = 1;
            // 
            // lblOkValue
            // 
            this.lblOkValue.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblOkValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOkValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOkValue.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOkValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblOkValue.Location = new System.Drawing.Point(0, 42);
            this.lblOkValue.Name = "lblOkValue";
            this.lblOkValue.Padding = new System.Windows.Forms.Padding(10, 0, 4, 8);
            this.lblOkValue.Size = new System.Drawing.Size(459, 76);
            this.lblOkValue.TabIndex = 0;
            this.lblOkValue.Text = "0";
            this.lblOkValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOkTitle
            // 
            this.lblOkTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOkTitle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOkTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblOkTitle.Location = new System.Drawing.Point(0, 0);
            this.lblOkTitle.Name = "lblOkTitle";
            this.lblOkTitle.Padding = new System.Windows.Forms.Padding(10, 12, 0, 0);
            this.lblOkTitle.Size = new System.Drawing.Size(459, 42);
            this.lblOkTitle.TabIndex = 1;
            this.lblOkTitle.Text = "OK 数量";
            // 
            // metricNg
            // 
            this.metricNg.BackColor = System.Drawing.SystemColors.Control;
            this.metricNg.Controls.Add(this.lblNgValue);
            this.metricNg.Controls.Add(this.lblNgTitle);
            this.metricNg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metricNg.Location = new System.Drawing.Point(934, 0);
            this.metricNg.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.metricNg.Name = "metricNg";
            this.metricNg.Size = new System.Drawing.Size(459, 118);
            this.metricNg.TabIndex = 2;
            // 
            // lblNgValue
            // 
            this.lblNgValue.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblNgValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNgValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNgValue.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNgValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNgValue.Location = new System.Drawing.Point(0, 42);
            this.lblNgValue.Name = "lblNgValue";
            this.lblNgValue.Padding = new System.Windows.Forms.Padding(10, 0, 4, 8);
            this.lblNgValue.Size = new System.Drawing.Size(459, 76);
            this.lblNgValue.TabIndex = 0;
            this.lblNgValue.Text = "0";
            this.lblNgValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNgTitle
            // 
            this.lblNgTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNgTitle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNgTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNgTitle.Location = new System.Drawing.Point(0, 0);
            this.lblNgTitle.Name = "lblNgTitle";
            this.lblNgTitle.Padding = new System.Windows.Forms.Padding(10, 12, 0, 0);
            this.lblNgTitle.Size = new System.Drawing.Size(459, 42);
            this.lblNgTitle.TabIndex = 1;
            this.lblNgTitle.Text = "NG 数量";
            // 
            // metricYield
            // 
            this.metricYield.BackColor = System.Drawing.SystemColors.Control;
            this.metricYield.Controls.Add(this.lblYieldValue);
            this.metricYield.Controls.Add(this.lblYieldTitle);
            this.metricYield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metricYield.Location = new System.Drawing.Point(1401, 0);
            this.metricYield.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.metricYield.Name = "metricYield";
            this.metricYield.Size = new System.Drawing.Size(459, 118);
            this.metricYield.TabIndex = 3;
            // 
            // lblYieldValue
            // 
            this.lblYieldValue.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblYieldValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblYieldValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblYieldValue.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblYieldValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblYieldValue.Location = new System.Drawing.Point(0, 42);
            this.lblYieldValue.Name = "lblYieldValue";
            this.lblYieldValue.Padding = new System.Windows.Forms.Padding(10, 0, 4, 8);
            this.lblYieldValue.Size = new System.Drawing.Size(459, 76);
            this.lblYieldValue.TabIndex = 0;
            this.lblYieldValue.Text = "0.00%";
            this.lblYieldValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblYieldTitle
            // 
            this.lblYieldTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblYieldTitle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblYieldTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblYieldTitle.Location = new System.Drawing.Point(0, 0);
            this.lblYieldTitle.Name = "lblYieldTitle";
            this.lblYieldTitle.Padding = new System.Windows.Forms.Padding(10, 12, 0, 0);
            this.lblYieldTitle.Size = new System.Drawing.Size(459, 42);
            this.lblYieldTitle.TabIndex = 1;
            this.lblYieldTitle.Text = "检测良率";
            // 
            // metricDuration
            // 
            this.metricDuration.BackColor = System.Drawing.SystemColors.Control;
            this.metricDuration.Controls.Add(this.lblDurationValue);
            this.metricDuration.Controls.Add(this.lblDurationTitle);
            this.metricDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metricDuration.Location = new System.Drawing.Point(1868, 0);
            this.metricDuration.Margin = new System.Windows.Forms.Padding(0);
            this.metricDuration.Name = "metricDuration";
            this.metricDuration.Size = new System.Drawing.Size(471, 118);
            this.metricDuration.TabIndex = 4;
            // 
            // lblDurationValue
            // 
            this.lblDurationValue.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblDurationValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDurationValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDurationValue.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDurationValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDurationValue.Location = new System.Drawing.Point(0, 42);
            this.lblDurationValue.Name = "lblDurationValue";
            this.lblDurationValue.Padding = new System.Windows.Forms.Padding(10, 0, 4, 8);
            this.lblDurationValue.Size = new System.Drawing.Size(471, 76);
            this.lblDurationValue.TabIndex = 0;
            this.lblDurationValue.Text = "-- ms";
            this.lblDurationValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDurationTitle
            // 
            this.lblDurationTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDurationTitle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDurationTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDurationTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDurationTitle.Name = "lblDurationTitle";
            this.lblDurationTitle.Padding = new System.Windows.Forms.Padding(10, 12, 0, 0);
            this.lblDurationTitle.Size = new System.Drawing.Size(471, 42);
            this.lblDurationTitle.TabIndex = 1;
            this.lblDurationTitle.Text = "平均耗时";
            // 
            // chartsLayout
            // 
            this.chartsLayout.ColumnCount = 2;
            this.chartsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68F));
            this.chartsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.chartsLayout.Controls.Add(this.panelTrend, 0, 0);
            this.chartsLayout.Controls.Add(this.panelDistribution, 1, 0);
            this.chartsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartsLayout.Location = new System.Drawing.Point(12, 212);
            this.chartsLayout.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.chartsLayout.Name = "chartsLayout";
            this.chartsLayout.RowCount = 1;
            this.chartsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.chartsLayout.Size = new System.Drawing.Size(2339, 534);
            this.chartsLayout.TabIndex = 2;
            // 
            // panelTrend
            // 
            this.panelTrend.BackColor = System.Drawing.SystemColors.Control;
            this.panelTrend.Controls.Add(this.chartTrend);
            this.panelTrend.Controls.Add(this.lblTrendTitle);
            this.panelTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTrend.Location = new System.Drawing.Point(0, 0);
            this.panelTrend.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.panelTrend.Name = "panelTrend";
            this.panelTrend.Size = new System.Drawing.Size(1582, 534);
            this.panelTrend.TabIndex = 0;
            // 
            // chartTrend
            // 
            chartArea3.AxisX.LabelStyle.Font = new System.Drawing.Font("宋体", 9F);
            chartArea3.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(106)))), ((int)(((byte)(116)))));
            chartArea3.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(215)))), ((int)(((byte)(222)))));
            chartArea3.AxisX.MajorGrid.Enabled = false;
            chartArea3.AxisY.LabelStyle.Font = new System.Drawing.Font("宋体", 9F);
            chartArea3.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(106)))), ((int)(((byte)(116)))));
            chartArea3.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(215)))), ((int)(((byte)(222)))));
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(235)))), ((int)(((byte)(239)))));
            chartArea3.AxisY.Minimum = 0D;
            chartArea3.BackColor = System.Drawing.Color.White;
            chartArea3.Name = "TrendArea";
            this.chartTrend.ChartAreas.Add(chartArea3);
            this.chartTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Alignment = System.Drawing.StringAlignment.Center;
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend3.Font = new System.Drawing.Font("宋体", 9.5F);
            legend3.IsTextAutoFit = false;
            legend3.Name = "TrendLegend";
            this.chartTrend.Legends.Add(legend3);
            this.chartTrend.Location = new System.Drawing.Point(0, 46);
            this.chartTrend.Name = "chartTrend";
            this.chartTrend.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series4.BorderWidth = 3;
            series4.ChartArea = "TrendArea";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(181)))), ((int)(((byte)(107)))));
            series4.Legend = "TrendLegend";
            series4.LegendText = "OK";
            series4.MarkerSize = 7;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series4.Name = "seriesOk";
            series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            series5.BorderWidth = 3;
            series5.ChartArea = "TrendArea";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(87)))), ((int)(((byte)(87)))));
            series5.Legend = "TrendLegend";
            series5.LegendText = "NG";
            series5.MarkerSize = 7;
            series5.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series5.Name = "seriesNg";
            series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            this.chartTrend.Series.Add(series4);
            this.chartTrend.Series.Add(series5);
            this.chartTrend.Size = new System.Drawing.Size(1582, 488);
            this.chartTrend.TabIndex = 0;
            // 
            // lblTrendTitle
            // 
            this.lblTrendTitle.BackColor = System.Drawing.SystemColors.Control;
            this.lblTrendTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrendTitle.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTrendTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTrendTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTrendTitle.Name = "lblTrendTitle";
            this.lblTrendTitle.Padding = new System.Windows.Forms.Padding(14, 8, 0, 0);
            this.lblTrendTitle.Size = new System.Drawing.Size(1582, 46);
            this.lblTrendTitle.TabIndex = 1;
            this.lblTrendTitle.Text = "检测趋势";
            // 
            // panelDistribution
            // 
            this.panelDistribution.BackColor = System.Drawing.SystemColors.Control;
            this.panelDistribution.Controls.Add(this.chartDistribution);
            this.panelDistribution.Controls.Add(this.lblDistributionTitle);
            this.panelDistribution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDistribution.Location = new System.Drawing.Point(1590, 0);
            this.panelDistribution.Margin = new System.Windows.Forms.Padding(0);
            this.panelDistribution.Name = "panelDistribution";
            this.panelDistribution.Size = new System.Drawing.Size(749, 534);
            this.panelDistribution.TabIndex = 1;
            // 
            // chartDistribution
            // 
            chartArea4.BackColor = System.Drawing.Color.White;
            chartArea4.Name = "DistributionArea";
            this.chartDistribution.ChartAreas.Add(chartArea4);
            this.chartDistribution.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Alignment = System.Drawing.StringAlignment.Center;
            legend4.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend4.Font = new System.Drawing.Font("宋体", 9.5F);
            legend4.IsTextAutoFit = false;
            legend4.Name = "DistributionLegend";
            this.chartDistribution.Legends.Add(legend4);
            this.chartDistribution.Location = new System.Drawing.Point(0, 46);
            this.chartDistribution.Name = "chartDistribution";
            this.chartDistribution.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series6.ChartArea = "DistributionArea";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series6.CustomProperties = "DoughnutRadius=58, PieLabelStyle=Outside";
            series6.Font = new System.Drawing.Font("宋体", 9F);
            series6.IsValueShownAsLabel = true;
            series6.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(79)))), ((int)(((byte)(88)))));
            series6.Legend = "DistributionLegend";
            series6.Name = "seriesDistribution";
            this.chartDistribution.Series.Add(series6);
            this.chartDistribution.Size = new System.Drawing.Size(749, 488);
            this.chartDistribution.TabIndex = 0;
            // 
            // lblDistributionTitle
            // 
            this.lblDistributionTitle.BackColor = System.Drawing.SystemColors.Control;
            this.lblDistributionTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDistributionTitle.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDistributionTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDistributionTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDistributionTitle.Name = "lblDistributionTitle";
            this.lblDistributionTitle.Padding = new System.Windows.Forms.Padding(14, 8, 0, 0);
            this.lblDistributionTitle.Size = new System.Drawing.Size(749, 46);
            this.lblDistributionTitle.TabIndex = 1;
            this.lblDistributionTitle.Text = "结果分布";
            // 
            // panelRecords
            // 
            this.panelRecords.BackColor = System.Drawing.SystemColors.Control;
            this.panelRecords.Controls.Add(this.gridRecords);
            this.panelRecords.Controls.Add(this.recordsHeader);
            this.panelRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRecords.Location = new System.Drawing.Point(12, 756);
            this.panelRecords.Margin = new System.Windows.Forms.Padding(0);
            this.panelRecords.Name = "panelRecords";
            this.panelRecords.Size = new System.Drawing.Size(2339, 503);
            this.panelRecords.TabIndex = 3;
            // 
            // gridRecords
            // 
            this.gridRecords.AllowUserToAddRows = false;
            this.gridRecords.AllowUserToDeleteRows = false;
            this.gridRecords.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.gridRecords.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gridRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridRecords.BackgroundColor = System.Drawing.Color.White;
            this.gridRecords.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridRecords.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.gridRecords.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            this.gridRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gridRecords.ColumnHeadersHeight = 38;
            this.gridRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colProductId,
            this.colFrontAngle,
            this.colRearAngle,
            this.colResult,
            this.colDuration});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridRecords.DefaultCellStyle = dataGridViewCellStyle6;
            this.gridRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRecords.EnableHeadersVisualStyles = false;
            this.gridRecords.GridColor = System.Drawing.SystemColors.ControlLight;
            this.gridRecords.Location = new System.Drawing.Point(0, 46);
            this.gridRecords.MultiSelect = false;
            this.gridRecords.Name = "gridRecords";
            this.gridRecords.ReadOnly = true;
            this.gridRecords.RowHeadersVisible = false;
            this.gridRecords.RowHeadersWidth = 82;
            this.gridRecords.RowTemplate.Height = 34;
            this.gridRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRecords.Size = new System.Drawing.Size(2339, 457);
            this.gridRecords.TabIndex = 0;
            // 
            // colTime
            // 
            this.colTime.FillWeight = 125F;
            this.colTime.HeaderText = "检测时间";
            this.colTime.MinimumWidth = 10;
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            // 
            // colProductId
            // 
            this.colProductId.FillWeight = 115F;
            this.colProductId.HeaderText = "产品编号";
            this.colProductId.MinimumWidth = 10;
            this.colProductId.Name = "colProductId";
            this.colProductId.ReadOnly = true;
            // 
            // colFrontAngle
            // 
            this.colFrontAngle.HeaderText = "前相机展平角";
            this.colFrontAngle.MinimumWidth = 10;
            this.colFrontAngle.Name = "colFrontAngle";
            this.colFrontAngle.ReadOnly = true;
            // 
            // colRearAngle
            // 
            this.colRearAngle.HeaderText = "后相机展平角";
            this.colRearAngle.MinimumWidth = 10;
            this.colRearAngle.Name = "colRearAngle";
            this.colRearAngle.ReadOnly = true;
            // 
            // colResult
            // 
            this.colResult.FillWeight = 75F;
            this.colResult.HeaderText = "检测结果";
            this.colResult.MinimumWidth = 10;
            this.colResult.Name = "colResult";
            this.colResult.ReadOnly = true;
            // 
            // colDuration
            // 
            this.colDuration.FillWeight = 80F;
            this.colDuration.HeaderText = "检测耗时";
            this.colDuration.MinimumWidth = 10;
            this.colDuration.Name = "colDuration";
            this.colDuration.ReadOnly = true;
            // 
            // recordsHeader
            // 
            this.recordsHeader.BackColor = System.Drawing.SystemColors.Control;
            this.recordsHeader.Controls.Add(this.lblRecordsTitle);
            this.recordsHeader.Controls.Add(this.lblRecordCount);
            this.recordsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.recordsHeader.Location = new System.Drawing.Point(0, 0);
            this.recordsHeader.Name = "recordsHeader";
            this.recordsHeader.Size = new System.Drawing.Size(2339, 46);
            this.recordsHeader.TabIndex = 1;
            // 
            // lblRecordsTitle
            // 
            this.lblRecordsTitle.Font = new System.Drawing.Font("宋体", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordsTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRecordsTitle.Location = new System.Drawing.Point(14, 8);
            this.lblRecordsTitle.Name = "lblRecordsTitle";
            this.lblRecordsTitle.Size = new System.Drawing.Size(240, 30);
            this.lblRecordsTitle.TabIndex = 0;
            this.lblRecordsTitle.Text = "最近检测明细";
            this.lblRecordsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblRecordCount.Font = new System.Drawing.Font("宋体", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordCount.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblRecordCount.Location = new System.Drawing.Point(2159, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Padding = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.lblRecordCount.Size = new System.Drawing.Size(180, 46);
            this.lblRecordCount.TabIndex = 1;
            this.lblRecordCount.Text = "共 0 条";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPageSubtitle
            // 
            this.lblPageSubtitle.AutoSize = true;
            this.lblPageSubtitle.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPageSubtitle.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPageSubtitle.Location = new System.Drawing.Point(189, 34);
            this.lblPageSubtitle.Name = "lblPageSubtitle";
            this.lblPageSubtitle.Size = new System.Drawing.Size(460, 28);
            this.lblPageSubtitle.TabIndex = 1;
            this.lblPageSubtitle.Text = "检测统计、质量趋势与最近检测明细";
            // 
            // TabAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.rootLayout);
            this.Name = "TabAnalysis";
            this.Size = new System.Drawing.Size(2363, 1271);
            this.rootLayout.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.metricsLayout.ResumeLayout(false);
            this.metricTotal.ResumeLayout(false);
            this.metricOk.ResumeLayout(false);
            this.metricNg.ResumeLayout(false);
            this.metricYield.ResumeLayout(false);
            this.metricDuration.ResumeLayout(false);
            this.chartsLayout.ResumeLayout(false);
            this.panelTrend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).EndInit();
            this.panelDistribution.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDistribution)).EndInit();
            this.panelRecords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRecords)).EndInit();
            this.recordsHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel rootLayout;
        private Panel headerPanel;
        private Label lblPageTitle;
        private Label lblLastUpdate;

        private TableLayoutPanel metricsLayout;
        private Panel metricTotal;
        private Label lblTotalTitle;
        private Label lblTotalValue;
        private Panel metricOk;
        private Label lblOkTitle;
        private Label lblOkValue;
        private Panel metricNg;
        private Label lblNgTitle;
        private Label lblNgValue;
        private Panel metricYield;
        private Label lblYieldTitle;
        private Label lblYieldValue;
        private Panel metricDuration;
        private Label lblDurationTitle;
        private Label lblDurationValue;

        private TableLayoutPanel chartsLayout;
        private Panel panelTrend;
        private Label lblTrendTitle;
        private Chart chartTrend;
        private Panel panelDistribution;
        private Label lblDistributionTitle;
        private Chart chartDistribution;

        private Panel panelRecords;
        private Panel recordsHeader;
        private Label lblRecordsTitle;
        private Label lblRecordCount;
        private DataGridView gridRecords;
        private DataGridViewTextBoxColumn colTime;
        private DataGridViewTextBoxColumn colProductId;
        private DataGridViewTextBoxColumn colFrontAngle;
        private DataGridViewTextBoxColumn colRearAngle;
        private DataGridViewTextBoxColumn colResult;
        private DataGridViewTextBoxColumn colDuration;
        private Label lblPageSubtitle;
    }
}
