using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace _180Detection
{
    public sealed class TabRecords : UserControl
    {
        private sealed class RecordItem
        {
            public DateTime Time;
            public string Product;
            public string Status;
            public string Defect;
            public double Score;
            public double Similarity;
            public long Elapsed;
            public string ImagePath;
            public string MarkedPath;
            public string JsonPath;
        }

        private readonly JavaScriptSerializer _json = new JavaScriptSerializer();
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
            string directory = ResolveResultDirectory();
            lblDirectory.Text = "结果目录：" + directory;

            if (Directory.Exists(directory))
            {
                string[] files = Directory.GetFiles(
                    directory, "*.json", SearchOption.TopDirectoryOnly);

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
            TableLayoutPanel root = new TableLayoutPanel();
            root.BackColor = UiTheme.WindowBackground;
            root.ColumnCount = 1;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowCount = 3;
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            root.Dock = DockStyle.Fill;

            root.Controls.Add(BuildToolbar(), 0, 0);
            root.Controls.Add(BuildGridPanel(), 0, 1);
            root.Controls.Add(BuildFooter(), 0, 2);
            Controls.Add(root);
        }

        private Control BuildToolbar()
        {
            Panel panel = new Panel();
            panel.BackColor = UiTheme.Surface;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(10, 6, 10, 6);
            panel.Margin = new Padding(0, 0, 0, 8);

            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.WrapContents = false;

            flow.Controls.Add(CreateCaption("关键字"));
            txtKeyword = new TextBox();
            txtKeyword.Width = 190;
            txtKeyword.Margin = new Padding(0, 3, 14, 3);
            txtKeyword.TextChanged += delegate { ApplyFilter(); };
            flow.Controls.Add(txtKeyword);

            flow.Controls.Add(CreateCaption("结果"));
            cmbStatus = new ComboBox();
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.AddRange(new object[] { "全部", "PASS", "NG" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.Width = 100;
            cmbStatus.Margin = new Padding(0, 2, 14, 2);
            cmbStatus.SelectedIndexChanged += delegate { ApplyFilter(); };
            flow.Controls.Add(cmbStatus);

            flow.Controls.Add(CreateCaption("时间"));
            cmbRange = new ComboBox();
            cmbRange.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRange.Items.AddRange(new object[] { "全部", "今天", "最近7天", "最近30天" });
            cmbRange.SelectedIndex = 0;
            cmbRange.Width = 110;
            cmbRange.Margin = new Padding(0, 2, 14, 2);
            cmbRange.SelectedIndexChanged += delegate { ApplyFilter(); };
            flow.Controls.Add(cmbRange);

            Button refresh = CreateButton("刷新", false);
            Button openImage = CreateButton("打开结果图", false);
            Button openFolder = CreateButton("打开目录", false);
            refresh.Click += delegate { RefreshRecords(); };
            openImage.Click += btnOpenImage_Click;
            openFolder.Click += btnOpenFolder_Click;
            flow.Controls.Add(refresh);
            flow.Controls.Add(openImage);
            flow.Controls.Add(openFolder);

            panel.Controls.Add(flow);
            return panel;
        }

        private Control BuildGridPanel()
        {
            Panel panel = new Panel();
            panel.BackColor = UiTheme.Surface;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(0);

            grid = new DataGridView();
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = UiTheme.Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersDefaultCellStyle.BackColor = UiTheme.SurfaceSoft;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Microsoft YaHei UI", 8.5F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = UiTheme.Surface;
            grid.DefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = UiTheme.NavigationActive;
            grid.DefaultCellStyle.SelectionForeColor = UiTheme.TextPrimary;
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 32;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.CellDoubleClick += delegate { OpenSelectedImage(); };

            grid.Columns.Add("time", "检测时间");
            grid.Columns.Add("status", "结果");
            grid.Columns.Add("product", "产品");
            grid.Columns.Add("defect", "异常类型");
            grid.Columns.Add("score", "PatchCore Score");
            grid.Columns.Add("similarity", "相似度");
            grid.Columns.Add("elapsed", "耗时(ms)");
            grid.Columns.Add("file", "文件名");

            grid.Columns["time"].FillWeight = 125;
            grid.Columns["status"].FillWeight = 55;
            grid.Columns["product"].FillWeight = 80;
            grid.Columns["defect"].FillWeight = 90;
            grid.Columns["score"].FillWeight = 85;
            grid.Columns["similarity"].FillWeight = 70;
            grid.Columns["elapsed"].FillWeight = 70;
            grid.Columns["file"].FillWeight = 150;

            panel.Controls.Add(grid);
            return panel;
        }

        private Control BuildFooter()
        {
            Panel panel = new Panel();
            panel.BackColor = UiTheme.Surface;
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(0, 8, 0, 0);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.ColumnCount = 2;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(10, 0, 10, 0);

            lblSummary = new Label();
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.ForeColor = UiTheme.TextSecondary;
            lblSummary.TextAlign = ContentAlignment.MiddleLeft;

            lblDirectory = new Label();
            lblDirectory.AutoEllipsis = true;
            lblDirectory.Dock = DockStyle.Fill;
            lblDirectory.ForeColor = UiTheme.TextMuted;
            lblDirectory.TextAlign = ContentAlignment.MiddleRight;

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
            string status = cmbStatus.SelectedItem == null
                ? "全部" : cmbStatus.SelectedItem.ToString();
            DateTime minTime = DateTime.MinValue;

            string range = cmbRange.SelectedItem == null
                ? "全部" : cmbRange.SelectedItem.ToString();
            if (range == "今天")
                minTime = DateTime.Today;
            else if (range == "最近7天")
                minTime = DateTime.Now.AddDays(-7);
            else if (range == "最近30天")
                minTime = DateTime.Now.AddDays(-30);

            List<RecordItem> filtered = new List<RecordItem>();
            foreach (RecordItem item in _all)
            {
                if (item.Time < minTime)
                    continue;
                if (status != "全部" &&
                    !string.Equals(item.Status, status, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string haystack = (item.Product + " " + item.Defect + " " +
                        item.ImagePath + " " + item.MarkedPath).ToLowerInvariant();
                    if (haystack.IndexOf(keyword.ToLowerInvariant(), StringComparison.Ordinal) < 0)
                        continue;
                }
                filtered.Add(item);
            }

            grid.Rows.Clear();
            int ok = 0;
            int ng = 0;
            foreach (RecordItem item in filtered.OrderByDescending(r => r.Time))
            {
                int index = grid.Rows.Add(
                    item.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    item.Status,
                    string.IsNullOrWhiteSpace(item.Product) ? "--" : item.Product,
                    string.IsNullOrWhiteSpace(item.Defect) ? "--" : item.Defect,
                    item.Score.ToString("0.0000", CultureInfo.InvariantCulture),
                    FormatSimilarity(item.Similarity),
                    item.Elapsed.ToString(CultureInfo.InvariantCulture),
                    Path.GetFileName(string.IsNullOrWhiteSpace(item.ImagePath)
                        ? item.JsonPath : item.ImagePath));
                grid.Rows[index].Tag = item;

                if (item.Status == "NG") ng++;
                else if (item.Status == "PASS") ok++;
            }

            lblSummary.Text = "记录：" + filtered.Count + "   PASS：" + ok + "   NG：" + ng;
        }

        private RecordItem TryRead(string jsonPath)
        {
            try
            {
                object parsed = _json.DeserializeObject(File.ReadAllText(jsonPath));
                Dictionary<string, object> root = parsed as Dictionary<string, object>;
                if (root == null)
                    return null;

                Dictionary<string, object> data = root;
                object nested;
                if (root.TryGetValue("result", out nested))
                {
                    Dictionary<string, object> nestedResult =
                        nested as Dictionary<string, object>;
                    if (nestedResult != null)
                        data = nestedResult;
                }

                string status = GetString(data, "status", "result_status", "decision");
                object ng = GetValue(data, "is_ng", "isNg", "IsNg");
                bool isNg = ng != null
                    ? ToBool(ng)
                    : string.Equals(status, "NG", StringComparison.OrdinalIgnoreCase) ||
                      string.Equals(status, "FAIL", StringComparison.OrdinalIgnoreCase) ||
                      string.Equals(status, "ANOMALY", StringComparison.OrdinalIgnoreCase);

                string product = GetString(root, "_ui_product", "product", "product_name");
                if (string.IsNullOrWhiteSpace(product))
                    product = GetString(data, "product", "product_name");

                DateTime time = File.GetLastWriteTime(jsonPath);
                string timeText = GetString(root, "_ui_record_time", "record_time", "timestamp");
                DateTime parsedTime;
                if (DateTime.TryParse(timeText, out parsedTime))
                    time = parsedTime;

                string image = GetString(root, "_ui_image_path");
                if (string.IsNullOrWhiteSpace(image))
                    image = GetString(data, "image_path", "imagePath", "ImagePath", "image");

                string marked = GetString(root, "_ui_marked_image_path");
                if (string.IsNullOrWhiteSpace(marked))
                    marked = GetString(data,
                        "marked_image_path", "markedImagePath", "MarkedImagePath", "marked");

                return new RecordItem
                {
                    Time = time,
                    Product = product,
                    Status = isNg ? "NG" : "PASS",
                    Defect = isNg
                        ? GetString(data, "defect_class", "defectClass", "class_name", "class")
                        : "Normal",
                    Score = GetDouble(data,
                        "anomaly_score", "anomalyScore", "patchcore_score", "score"),
                    Similarity = GetDouble(data,
                        "similarity", "dino_similarity", "classification_similarity"),
                    Elapsed = GetLong(data,
                        "elapsed_ms", "elapsedMilliseconds", "duration_ms"),
                    ImagePath = ResolveRecordPath(image, jsonPath),
                    MarkedPath = ResolveRecordPath(marked, jsonPath),
                    JsonPath = jsonPath
                };
            }
            catch
            {
                return null;
            }
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            OpenSelectedImage();
        }

        private void OpenSelectedImage()
        {
            RecordItem item = GetSelected();
            if (item == null)
                return;

            string path = File.Exists(item.MarkedPath) ? item.MarkedPath : item.ImagePath;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                MessageBox.Show("该记录对应的图像文件不存在。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法打开图像：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            string directory = ResolveResultDirectory();
            Directory.CreateDirectory(directory);
            Process.Start(new ProcessStartInfo
            {
                FileName = directory,
                UseShellExecute = true
            });
        }

        private RecordItem GetSelected()
        {
            if (grid.SelectedRows.Count == 0)
                return null;
            return grid.SelectedRows[0].Tag as RecordItem;
        }

        private static Label CreateCaption(string text)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.ForeColor = UiTheme.TextSecondary;
            label.Margin = new Padding(0, 8, 8, 0);
            label.Text = text;
            return label;
        }

        private static Button CreateButton(string text, bool primary)
        {
            Button button = new Button();
            button.BackColor = primary ? UiTheme.PrimaryButton : UiTheme.Surface;
            button.ForeColor = primary ? Color.White : UiTheme.TextPrimary;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor =
                primary ? UiTheme.PrimaryButton : UiTheme.BorderStrong;
            button.FlatAppearance.BorderSize = 1;
            button.Font = new Font("Microsoft YaHei UI", 8.5F);
            button.Height = 30;
            button.Width = text.Length > 3 ? 92 : 72;
            button.Margin = new Padding(4, 2, 4, 2);
            button.Text = text;
            return button;
        }

        private static string ResolveResultDirectory()
        {
            string configured = ConfigurationManager.AppSettings["InferenceResultDirectory"];
            if (string.IsNullOrWhiteSpace(configured))
                configured = @"runtime\results";

            string expanded = Environment.ExpandEnvironmentVariables(configured.Trim());
            return Path.IsPathRooted(expanded)
                ? Path.GetFullPath(expanded)
                : Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, expanded));
        }

        private static string ResolveRecordPath(string value, string jsonPath)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            if (Path.IsPathRooted(value))
                return value;
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(jsonPath), value));
        }

        private static object GetValue(Dictionary<string, object> values, params string[] keys)
        {
            foreach (string key in keys)
            {
                object value;
                if (values.TryGetValue(key, out value))
                    return value;
            }
            return null;
        }

        private static string GetString(Dictionary<string, object> values, params string[] keys)
        {
            object value = GetValue(values, keys);
            return value == null ? string.Empty : Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private static double GetDouble(Dictionary<string, object> values, params string[] keys)
        {
            object value = GetValue(values, keys);
            if (value == null) return 0D;
            try { return Convert.ToDouble(value, CultureInfo.InvariantCulture); }
            catch { return 0D; }
        }

        private static long GetLong(Dictionary<string, object> values, params string[] keys)
        {
            object value = GetValue(values, keys);
            if (value == null) return 0L;
            try { return Convert.ToInt64(value, CultureInfo.InvariantCulture); }
            catch { return 0L; }
        }

        private static bool ToBool(object value)
        {
            if (value is bool) return (bool)value;
            string text = Convert.ToString(value, CultureInfo.InvariantCulture);
            return text == "1" ||
                string.Equals(text, "true", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(text, "ng", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(text, "anomaly", StringComparison.OrdinalIgnoreCase);
        }

        private static string FormatSimilarity(double similarity)
        {
            double value = similarity;
            if (value >= 0D && value <= 1D)
                value *= 100D;
            return value.ToString("0.00", CultureInfo.InvariantCulture) + "%";
        }
    }
}
