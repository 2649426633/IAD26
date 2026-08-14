using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using IndustrialAnomaly.Runtime;
using _180Detection.Models;
using _180Detection.Services;

namespace _180Detection
{
    public sealed class TabProductConfig : UserControl
    {
        private readonly AppSettingsService _settingsService = new AppSettingsService();
        private readonly ProductConfigService _service;
        private readonly List<ProductConfig> _products = new List<ProductConfig>();
        private ListBox lstProducts;
        private TextBox txtName;
        private TextBox txtProductDirectory;
        private TextBox txtAnomalyThreshold;
        private CheckBox chkEnabled;
        private Label lblModelInfo;
        private Label lblConfigPath;
        private Label lblStatus;

        public event EventHandler ConfigurationsChanged;

        public TabProductConfig()
        {
            _service = new ProductConfigService(_settingsService);
            Font = new Font("Microsoft YaHei UI", 9F);
            BackColor = UiTheme.WindowBackground;
            Dock = DockStyle.Fill;
            BuildUi();
            ReloadConfigurations();
        }

        public void ReloadConfigurations()
        {
            _products.Clear();
            _products.AddRange(_service.Load());
            lstProducts.BeginUpdate();
            try
            {
                lstProducts.Items.Clear();
                foreach (ProductConfig product in _products)
                    lstProducts.Items.Add(product.Name);
            }
            finally
            {
                lstProducts.EndUpdate();
            }

            lblConfigPath.Text = "配置文件：" + _service.ConfigPath;
            if (lstProducts.Items.Count > 0)
                lstProducts.SelectedIndex = 0;
            else
                ClearEditor();
        }

        public string[] GetEnabledProductNames()
        {
            return _service.GetEnabledProductNames();
        }

        public ProductConfig GetProductByName(string name)
        {
            return _service.GetByName(name);
        }

        private void BuildUi()
        {
            TableLayoutPanel root = new TableLayoutPanel
            {
                BackColor = UiTheme.WindowBackground,
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.Controls.Add(BuildListPanel(), 0, 0);
            root.Controls.Add(BuildEditorPanel(), 1, 0);
            Controls.Add(root);
        }

        private Control BuildListPanel()
        {
            Panel panel = CreatePanel(new Padding(12));
            panel.Margin = new Padding(0, 0, 8, 0);
            TableLayoutPanel layout = new TableLayoutPanel { ColumnCount = 1, RowCount = 4, Dock = DockStyle.Fill };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            layout.Controls.Add(CreateTitle("产品列表"), 0, 0);

            lstProducts = new ListBox { BorderStyle = BorderStyle.FixedSingle, Dock = DockStyle.Fill };
            lstProducts.SelectedIndexChanged += lstProducts_SelectedIndexChanged;
            layout.Controls.Add(lstProducts, 0, 1);

            FlowLayoutPanel actions = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            Button add = CreateButton("新增产品", false);
            Button delete = CreateButton("删除", false);
            add.Click += btnAdd_Click;
            delete.Click += btnDelete_Click;
            actions.Controls.Add(add);
            actions.Controls.Add(delete);
            layout.Controls.Add(actions, 0, 2);

            lblConfigPath = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft YaHei UI", 8F),
                ForeColor = UiTheme.TextMuted,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(lblConfigPath, 0, 3);
            panel.Controls.Add(layout);
            return panel;
        }

        private Control BuildEditorPanel()
        {
            Panel panel = CreatePanel(new Padding(18));
            TableLayoutPanel root = new TableLayoutPanel { ColumnCount = 1, RowCount = 4, Dock = DockStyle.Fill };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            root.Controls.Add(CreateTitle("ONNX 产品模型"), 0, 0);

            TableLayoutPanel form = new TableLayoutPanel { ColumnCount = 3, RowCount = 4, Dock = DockStyle.Fill };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            for (int i = 0; i < 4; i++)
                form.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

            txtName = CreateTextBox();
            txtProductDirectory = CreateTextBox();
            txtAnomalyThreshold = CreateTextBox();
            txtAnomalyThreshold.PlaceholderText = "留空 = 未标定";
            chkEnabled = new CheckBox { Text = "启用此产品", Dock = DockStyle.Fill, Checked = true };

            AddFormRow(form, 0, "产品名称", txtName, null);
            Button browse = CreateButton("浏览...", false);
            browse.Click += delegate { BrowseFolder(txtProductDirectory); };
            AddFormRow(form, 1, "产品模型目录", txtProductDirectory, browse);
            AddFormRow(form, 2, "PASS/NG 阈值", txtAnomalyThreshold, null);
            AddFormRow(form, 3, "状态", chkEnabled, null);
            root.Controls.Add(form, 0, 1);

            Panel infoPanel = new Panel
            {
                BackColor = UiTheme.SurfaceSoft,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Padding = new Padding(14)
            };
            lblModelInfo = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9F),
                ForeColor = UiTheme.TextSecondary,
                Text = "选择产品后检查 product_model.json / 三个 bin 文件。"
            };
            infoPanel.Controls.Add(lblModelInfo);
            root.Controls.Add(infoPanel, 0, 2);

            TableLayoutPanel footer = new TableLayoutPanel { ColumnCount = 3, Dock = DockStyle.Fill };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            lblStatus = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                ForeColor = UiTheme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Button validate = CreateButton("模型自检", false);
            Button save = CreateButton("保存配置", true);
            validate.Margin = new Padding(6, 10, 0, 10);
            save.Margin = new Padding(6, 10, 0, 10);
            validate.Click += delegate { RefreshModelInfo(true); };
            save.Click += btnSave_Click;
            footer.Controls.Add(lblStatus, 0, 0);
            footer.Controls.Add(validate, 1, 0);
            footer.Controls.Add(save, 2, 0);
            root.Controls.Add(footer, 0, 3);

            panel.Controls.Add(root);
            return panel;
        }

        private void lstProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lstProducts.SelectedIndex;
            if (index < 0 || index >= _products.Count)
            {
                ClearEditor();
                return;
            }

            ProductConfig product = _products[index];
            txtName.Text = product.Name ?? string.Empty;
            txtProductDirectory.Text = product.ProductDirectory ?? string.Empty;
            txtAnomalyThreshold.Text = product.AnomalyThreshold.HasValue
                ? product.AnomalyThreshold.Value.ToString("0.######", CultureInfo.InvariantCulture)
                : string.Empty;
            chkEnabled.Checked = product.Enabled;
            lblStatus.Text = string.Empty;
            RefreshModelInfo(false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string baseName = "NewProduct";
            string name = baseName;
            int suffix = 1;
            while (_products.Exists(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                suffix++;
                name = baseName + suffix;
            }

            ProductConfig product = new ProductConfig
            {
                Name = name,
                ProductDirectory = Path.Combine("products", name.ToLowerInvariant()),
                AnomalyThreshold = null,
                Enabled = true
            };
            _products.Add(product);
            lstProducts.Items.Add(name);
            lstProducts.SelectedIndex = lstProducts.Items.Count - 1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = lstProducts.SelectedIndex;
            if (index < 0)
                return;
            if (_products.Count <= 1)
            {
                MessageBox.Show("至少保留一个产品配置。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show(
                "确定删除产品“" + _products[index].Name + "”吗？\r\n这里只删除 IAD26 配置，不删除产品模型目录。",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _products.RemoveAt(index);
            lstProducts.Items.RemoveAt(index);
            lstProducts.SelectedIndex = Math.Min(index, lstProducts.Items.Count - 1);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int index = lstProducts.SelectedIndex;
            if (index < 0 || index >= _products.Count)
                return;
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("产品名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            double? threshold = null;
            string thresholdText = (txtAnomalyThreshold.Text ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(thresholdText))
            {
                if (!double.TryParse(thresholdText, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsed) || parsed < 0D)
                {
                    MessageBox.Show("PASS/NG 阈值必须是非负数字，或留空表示“未标定”。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                threshold = parsed;
            }

            ProductConfig product = _products[index];
            product.Name = txtName.Text.Trim();
            product.ProductDirectory = txtProductDirectory.Text.Trim();
            product.AnomalyThreshold = threshold;
            product.Enabled = chkEnabled.Checked;

            try
            {
                _service.Save(_products);
                string selectedName = product.Name;
                ReloadConfigurations();
                for (int i = 0; i < lstProducts.Items.Count; i++)
                {
                    if (string.Equals(lstProducts.Items[i].ToString(), selectedName, StringComparison.OrdinalIgnoreCase))
                    {
                        lstProducts.SelectedIndex = i;
                        break;
                    }
                }
                lblStatus.Text = "已保存 · 检测工作台产品列表已同步";
                ConfigurationsChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshModelInfo(bool showDialog)
        {
            int index = lstProducts.SelectedIndex;
            if (index < 0 || index >= _products.Count)
                return;
            try
            {
                ProductConfig temp = new ProductConfig
                {
                    Name = txtName.Text.Trim(),
                    ProductDirectory = txtProductDirectory.Text.Trim(),
                    AnomalyThreshold = _products[index].AnomalyThreshold,
                    Enabled = chkEnabled.Checked
                };
                string directory = _service.ResolveProductDirectory(temp);
                ProductModel model = ProductModel.Load(directory);
                lblModelInfo.Text =
                    "状态：可用\r\n" +
                    "目录：" + directory + "\r\n" +
                    "产品：" + model.Manifest.ProductName + "\r\n" +
                    "类别：" + string.Join(", ", model.Manifest.Classes) + "\r\n" +
                    "Memory Rows：" + model.Manifest.PatchCoreMemoryRows + "\r\n" +
                    "Memory Strategy：" + model.Manifest.PatchCoreMemoryStrategy + "\r\n" +
                    "BBox Threshold：" + model.Manifest.BboxRelativeThreshold.ToString("0.###");
                if (showDialog)
                    MessageBox.Show("产品模型自检通过。", "模型自检", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblModelInfo.Text = "状态：不可用\r\n" + ex.Message;
                if (showDialog)
                    MessageBox.Show(ex.Message, "模型自检失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearEditor()
        {
            if (txtName == null)
                return;
            txtName.Text = string.Empty;
            txtProductDirectory.Text = string.Empty;
            txtAnomalyThreshold.Text = string.Empty;
            chkEnabled.Checked = true;
            lblModelInfo.Text = "--";
        }

        private static void BrowseFolder(TextBox target)
        {
            using FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (Directory.Exists(target.Text))
                dialog.SelectedPath = target.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
                target.Text = dialog.SelectedPath;
        }

        private static Panel CreatePanel(Padding padding)
        {
            return new Panel { BackColor = UiTheme.Surface, BorderStyle = BorderStyle.FixedSingle, Dock = DockStyle.Fill, Padding = padding };
        }

        private static Label CreateTitle(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
                ForeColor = UiTheme.TextPrimary,
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private static TextBox CreateTextBox()
        {
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft YaHei UI", 9F),
                Margin = new Padding(0, 11, 8, 11)
            };
        }

        private static Button CreateButton(string text, bool primary)
        {
            Button button = new Button();
            button.BackColor = primary ? UiTheme.PrimaryButton : UiTheme.Surface;
            button.ForeColor = primary ? Color.White : UiTheme.TextPrimary;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = primary ? UiTheme.PrimaryButton : UiTheme.BorderStrong;
            button.Font = new Font("Microsoft YaHei UI", 8.5F, primary ? FontStyle.Bold : FontStyle.Regular);
            button.Size = new Size(96, 30);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
            return button;
        }

        private static void AddFormRow(TableLayoutPanel form, int row, string caption, Control editor, Control action)
        {
            Label label = new Label { Dock = DockStyle.Fill, ForeColor = UiTheme.TextSecondary, Text = caption, TextAlign = ContentAlignment.MiddleLeft };
            form.Controls.Add(label, 0, row);
            form.Controls.Add(editor, 1, row);
            if (action != null)
            {
                action.Dock = DockStyle.Fill;
                action.Margin = new Padding(4, 10, 0, 10);
                form.Controls.Add(action, 2, row);
            }
        }
    }
}
