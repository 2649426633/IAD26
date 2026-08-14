using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using _180Detection.Services;

namespace _180Detection
{
    public sealed class TabRecords : UserControl
    {
        private sealed class RecordItem
        {
            public DateTime Time;
            public string Product = string.Empty;
            public string Status = string.Empty;
            public string Defect = string.Empty;
            public double Score;
            public double Similarity;
            public double Margin;
            public long Elapsed;
            public string ImagePath = string.Empty;
            public string MarkedPath = string.Empty;
            public string JsonPath = string.Empty;
        }

        private readonly AppSettingsService _settingsService = new AppSettingsService();
        private readonly List<RecordItem> _all = new List<RecordItem>();
        private TextBox txtKeyword;
        private ComboBox cmbStatus;
        private ComboBox cmbRange;
        private DataGridView grid;
        private Label lblSummary;
        private Label lblDirectory;

        public TabRecords()
        {
            Font = new Font("Microsoft YaHei UI", 9F);
            BackColor = UiTheme.WindowBackground;
            Dock = DockStyle.Fill;
            BuildUi();
        }

        public void RefreshRecords()
        {
            _all.Clear();
            var settings = _settingsService.Load();
            string directory = _settingsService.RecordsRoot(settings);
            lblDirectory.Text = "记录目录：" + directory;
            if (Directory.Exists(directory))
            {
                string[] files = Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories);
                foreach (string file in files.OrderByDescending(File.GetLastWriteTime))
                {
                    RecordItem item = TryRead(file);
                    if (item != null)
                        _all.Add(item);
                }
            }
            ApplyFilter();
        }

        private void BuildUi()
        {
            TableLayoutPanel root = new TableLayoutPanel { BackColor = UiTheme.WindowBackground, ColumnCount = 1, RowCount = 3, Dock = DockStyle.Fill };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            root.Controls.Add(BuildToolbar(), 0, 0);
            root.Controls.Add(BuildGridPanel(), 0, 1);
            root.Controls.Add(BuildFooter(), 0, 2);
            Controls.Add(root);
        }

        private Control BuildToolbar()
        {
            Panel panel = new Panel { BackColor = UiTheme.Surface, BorderStyle = BorderStyle.FixedSingle, Dock = DockStyle.Fill, Padding = new Padding(10, 6, 10, 6), Margin = new Padding(0, 0, 0, 8) };
            FlowLayoutPanel flow = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            flow.Controls.Add(CreateCaption("关键字"));
            txtKeyword = new TextBox { Width = 190, Margin = new Padding(0, 3, 14, 3) };
            txtKeyword.TextChanged += delegate { ApplyFilter(); };
            flow.Controls.Add(txtKeyword);
            flow.Controls.Add(CreateCaption("结果"));
            cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Margin = new Padding(0, 2, 14, 2) };
            cmbStatus.Items.AddRange(new object[] { "全部", "PASS", "NG", "UNCALIBRATED" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += delegate { ApplyFilter(); };
            flow.Controls.Add(cmbStatus);
            flow.Controls.Add(CreateCaption("时间"));
            cmbRange = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 110, Margin = new Padding(0, 2, 14, 2) };
            cmbRange.Items.AddRange(new object[] { "全部", "今天", "最近7天", "最近30天" });
            cmbRange.SelectedIndex = 0;
            cmbRange.SelectedIndexChanged += delegate { ApplyFilter(); };
            flow.Controls.Add(cmbRange);
            Button refresh = CreateButton("刷新");
            Button openImage = CreateButton("打开结果图");
            Button openFolder = CreateButton("打开目录");
            refresh.Click += delegate { RefreshRecords(); };
            openImage.Click += delegate { OpenSelectedImage(); };
            openFolder.Click += delegate { OpenSelectedFolder(); };
            flow.Controls.Add(refresh);
            flow.Controls.Add(openImage);
            flow.Controls.Add(openFolder);
            panel.Controls.Add(flow);
            return panel;
        }

        private Control BuildGridPanel()
        {
            Panel panel = new Panel { BackColor = UiTheme.Surface, BorderStyle = BorderStyle.FixedSingle, Dock = DockStyle.Fill };
            grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = UiTheme.Surface,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                Dock = DockStyle.Fill,
                EnableHeadersVisualStyles = false,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = UiTheme.SurfaceSoft;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = UiTheme.Surface;
            grid.DefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = UiTheme.NavigationActive;
            grid.DefaultCellStyle.SelectionForeColor = UiTheme.TextPrimary;
            grid.RowTemplate.Height = 32;
            grid.CellDoubleClick += delegate { OpenSelectedImage(); };
            grid.Columns.Add("time", "检测时间");
            grid.Columns.Add("status", "结果");
            grid.Columns.Add("product", "产品");
            grid.Columns.Add("defect", "异常类型");
            grid.Columns.Add("score", "PatchCore Score");
            grid.Columns.Add("similarity", "相似度");
            grid.Columns.Add("margin", "Margin");
            grid.Columns.Add("elapsed", "耗时(ms)");
            grid.Columns.Add("file", "文件名");
            panel.Controls.Add(grid);
            return panel;
        }

        private Control BuildFooter()
        {
            Panel panel = new Panel { BackColor = UiTheme.Surface, Dock = DockStyle.Fill, Margin = new Padding(0, 8, 0, 0) };
            TableLayoutPanel layout = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, Padding = new Padding(10, 0, 10, 0) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            lblSummary = new Label { Dock = DockStyle.Fill, ForeColor = UiTheme.TextSecondary, TextAlign = ContentAlignment.MiddleLeft };
            lblDirectory = new Label { AutoEllipsis = true, Dock = DockStyle.Fill, ForeColor = UiTheme.TextMuted, TextAlign = ContentAlignment.MiddleRight };
            layout.Controls.Add(lblSummary, 0, 0);
            layout.Controls.Add(lblDirectory, 1, 0);
            panel.Controls.Add(layout);
            return panel;
        }

        private void ApplyFilter()
        {
            if (grid == null)
                return;
            string keyword = (txtKeyword.Text ?? string.Empty).Trim();
            string status = cmbStatus.SelectedItem == null ? "全部" : cmbStatus.SelectedItem.ToString();
            DateTime minTime = DateTime.MinValue;
            string range = cmbRange.SelectedItem == null ? "全部" : cmbRange.SelectedItem.ToString();
            if (range == "今天") minTime = DateTime.Today;
            else if (range == "最近7天") minTime = DateTime.Now.AddDays(-7);
            else if (range == "最近30天") minTime = DateTime.Now.AddDays(-30);

            List<RecordItem> filtered = _all
                .Where(item => item.Time >= minTime)
                .Where(item => status == "全部" || string.Equals(item.Status, status, StringComparison.OrdinalIgnoreCase))
                .Where(item => string.IsNullOrWhiteSpace(keyword) ||
                    (item.Product + " " + item.Defect + " " + item.ImagePath + " " + item.MarkedPath)
                    .IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderByDescending(item => item.Time)
                .ToList();

            grid.Rows.Clear();
            foreach (RecordItem item in filtered)
            {
                int row = grid.Rows.Add(
                    item.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    DisplayStatus(item.Status),
                    item.Product,
                    string.IsNullOrWhiteSpace(item.Defect) ? "--" : item.Defect,
                    item.Score.ToString("0.000000"),
                    FormatSimilarity(item.Similarity),
                    item.Margin.ToString("0.0000"),
                    item.Elapsed.ToString(CultureInfo.InvariantCulture),
                    Path.GetFileName(string.IsNullOrWhiteSpace(item.ImagePath) ? item.JsonPath : item.ImagePath));
                grid.Rows[row].Tag = item;
            }

            int ng = filtered.Count(x => string.Equals(x.Status, "NG", StringComparison.OrdinalIgnoreCase));
            int pass = filtered.Count(x => string.Equals(x.Status, "PASS", StringComparison.OrdinalIgnoreCase));
            int uncalibrated = filtered.Count(x => string.Equals(x.Status, "UNCALIBRATED", StringComparison.OrdinalIgnoreCase));
            lblSummary.Text = "共 " + filtered.Count + " 条 · PASS " + pass + " · NG " + ng + " · 未标定 " + uncalibrated;
        }

        private static RecordItem TryRead(string file)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(File.ReadAllText(file));
                JsonElement root = document.RootElement;
                if (TryGet(root, "result", out JsonElement nested) && nested.ValueKind == JsonValueKind.Object)
                    root = nested;

                return new RecordItem
                {
                    Time = GetDateTime(root, File.GetLastWriteTime(file), "record_time", "timestamp", "time"),
                    Product = GetString(root, "product", "product_name"),
                    Status = GetStatus(root),
                    Defect = GetString(root, "defect_class", "predicted_known_defect", "predicted_defect", "class_name"),
                    Score = GetDouble(root, "anomaly_score", "patchcore_anomaly_score", "patchcore_score", "score"),
                    Similarity = GetDouble(root, "similarity", "top1_similarity", "classification_similarity"),
                    Margin = GetDouble(root, "margin"),
                    Elapsed = GetLong(root, "elapsed_ms", "elapsedMilliseconds", "duration_ms"),
                    ImagePath = GetString(root, "image_path", "source_image_path", "image"),
                    MarkedPath = GetString(root, "marked_image_path", "full_marked_image", "marked"),
                    JsonPath = file
                };
            }
            catch { return null; }
        }

        private static string GetStatus(JsonElement root)
        {
            string status = GetString(root, "status", "anomaly_decision", "decision");
            if (!string.IsNullOrWhiteSpace(status))
                return status.ToUpperInvariant();
            if (TryGet(root, "is_ng", out JsonElement ng))
            {
                if (ng.ValueKind == JsonValueKind.True) return "NG";
                if (ng.ValueKind == JsonValueKind.False) return "PASS";
            }
            string final = GetString(root, "final_result");
            if (final.StartsWith("NG", StringComparison.OrdinalIgnoreCase)) return "NG";
            if (string.Equals(final, "PASS", StringComparison.OrdinalIgnoreCase)) return "PASS";
            return "UNCALIBRATED";
        }

        private void OpenSelectedImage()
        {
            RecordItem item = SelectedItem();
            if (item == null) return;
            string path = !string.IsNullOrWhiteSpace(item.MarkedPath) && File.Exists(item.MarkedPath) ? item.MarkedPath : item.ImagePath;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                MessageBox.Show("记录对应的图像不存在。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true });
        }

        private void OpenSelectedFolder()
        {
            RecordItem item = SelectedItem();
            if (item == null || string.IsNullOrWhiteSpace(item.JsonPath) || !File.Exists(item.JsonPath)) return;
            Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = "/select,\"" + item.JsonPath + "\"", UseShellExecute = true });
        }

        private RecordItem SelectedItem()
        {
            if (grid.SelectedRows.Count == 0) return null;
            return grid.SelectedRows[0].Tag as RecordItem;
        }

        private static bool TryGet(JsonElement root, string name, out JsonElement value)
        {
            if (root.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in root.EnumerateObject())
                {
                    if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        value = property.Value;
                        return true;
                    }
                }
            }
            value = default;
            return false;
        }

        private static string GetString(JsonElement root, params string[] names)
        {
            foreach (string name in names)
            {
                if (!TryGet(root, name, out JsonElement value)) continue;
                if (value.ValueKind == JsonValueKind.String) return value.GetString() ?? string.Empty;
                if (value.ValueKind != JsonValueKind.Null && value.ValueKind != JsonValueKind.Undefined) return value.ToString();
            }
            return string.Empty;
        }

        private static double GetDouble(JsonElement root, params string[] names)
        {
            foreach (string name in names)
            {
                if (!TryGet(root, name, out JsonElement value)) continue;
                if (value.ValueKind == JsonValueKind.Number && value.TryGetDouble(out double number)) return number;
                if (double.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out number)) return number;
            }
            return 0D;
        }

        private static long GetLong(JsonElement root, params string[] names)
        {
            foreach (string name in names)
            {
                if (!TryGet(root, name, out JsonElement value)) continue;
                if (value.ValueKind == JsonValueKind.Number && value.TryGetInt64(out long number)) return number;
                if (long.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out number)) return number;
            }
            return 0L;
        }

        private static DateTime GetDateTime(JsonElement root, DateTime fallback, params string[] names)
        {
            string text = GetString(root, names);
            if (DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime value))
                return value.ToLocalTime();
            return fallback;
        }

        private static string DisplayStatus(string status)
        {
            return string.Equals(status, "UNCALIBRATED", StringComparison.OrdinalIgnoreCase) ? "未标定" : status;
        }

        private static string FormatSimilarity(double similarity)
        {
            double percent = similarity;
            if (percent >= 0D && percent <= 1D) percent *= 100D;
            return percent.ToString("0.00") + "%";
        }

        private static Label CreateCaption(string text)
        {
            return new Label { AutoSize = true, ForeColor = UiTheme.TextSecondary, Margin = new Padding(0, 7, 8, 0), Text = text };
        }

        private static Button CreateButton(string text)
        {
            Button button = new Button();
            button.BackColor = UiTheme.Surface;
            button.ForeColor = UiTheme.TextPrimary;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = UiTheme.BorderStrong;
            button.Font = new Font("Microsoft YaHei UI", 8.5F);
            button.Margin = new Padding(6, 0, 0, 0);
            button.Size = new Size(96, 30);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
            return button;
        }
    }
}
