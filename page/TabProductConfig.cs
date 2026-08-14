using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using _180Detection.Models;
using _180Detection.Services;

namespace _180Detection
{
    public sealed class TabProductConfig : UserControl
    {
        private readonly ProductConfigService _service = new ProductConfigService();
        private readonly List<ProductConfig> _products = new List<ProductConfig>();

        private ListBox lstProducts;
        private TextBox txtName;
        private TextBox txtPatchCore;
        private TextBox txtDefectBank;
        private NumericUpDown nudAnomaly;
        private NumericUpDown nudSimilarity;
        private CheckBox chkEnabled;
        private Label lblPath;
        private Label lblStatus;

        public event EventHandler ConfigurationsChanged;

        public TabProductConfig()
        {
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
            lstProducts.Items.Clear();
            foreach (ProductConfig product in _products)
                lstProducts.Items.Add(product.Name);
            lstProducts.EndUpdate();

            lblPath.Text = "配置文件：" + _service.ConfigPath;
            if (lstProducts.Items.Count > 0)
                lstProducts.SelectedIndex = 0;
            else
                ClearEditor();
        }

        public string[] GetEnabledProductNames()
        {
            return _service.GetEnabledProductNames();
        }

        private void BuildUi()
        {
            TableLayoutPanel root = new TableLayoutPanel();
            root.BackColor = UiTheme.WindowBackground;
            root.ColumnCount = 2;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowCount = 1;
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.Dock = DockStyle.Fill;

            root.Controls.Add(BuildListPanel(), 0, 0);
            root.Controls.Add(BuildEditorPanel(), 1, 0);
            Controls.Add(root);
        }

        private Control BuildListPanel()
        {
            Panel panel = CreatePanel(new Padding(12));
            panel.Margin = new Padding(0, 0, 8, 0);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowCount = 4;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            layout.Dock = DockStyle.Fill;

            Label title = CreateTitle("产品列表");
            layout.Controls.Add(title, 0, 0);

            lstProducts = new ListBox();
            lstProducts.BorderStyle = BorderStyle.FixedSingle;
            lstProducts.Dock = DockStyle.Fill;
            lstProducts.Font = new Font("Microsoft YaHei UI", 9F);
            lstProducts.SelectedIndexChanged += lstProducts_SelectedIndexChanged;
            layout.Controls.Add(lstProducts, 0, 1);

            FlowLayoutPanel actions = new FlowLayoutPanel();
            actions.Dock = DockStyle.Fill;
            actions.FlowDirection = FlowDirection.LeftToRight;
            actions.WrapContents = false;
            Button add = CreateButton("新增产品", false);
            Button delete = CreateButton("删除", false);
            add.Click += btnAdd_Click;
            delete.Click += btnDelete_Click;
            actions.Controls.Add(add);
            actions.Controls.Add(delete);
            layout.Controls.Add(actions, 0, 2);

            lblPath = new Label();
            lblPath.AutoEllipsis = true;
            lblPath.Dock = DockStyle.Fill;
            lblPath.ForeColor = UiTheme.TextMuted;
            lblPath.Font = new Font("Microsoft YaHei UI", 8F);
            lblPath.TextAlign = ContentAlignment.MiddleLeft;
            layout.Controls.Add(lblPath, 0, 3);

            panel.Controls.Add(layout);
            return panel;
        }

        private Control BuildEditorPanel()
        {
            Panel panel = CreatePanel(new Padding(18));

            TableLayoutPanel root = new TableLayoutPanel();
            root.ColumnCount = 1;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowCount = 3;
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            root.Dock = DockStyle.Fill;

            root.Controls.Add(CreateTitle("产品参数"), 0, 0);

            TableLayoutPanel form = new TableLayoutPanel();
            form.ColumnCount = 3;
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92F));
            form.RowCount = 7;
            for (int i = 0; i < 7; i++)
                form.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            form.Dock = DockStyle.Top;

            txtName = CreateTextBox();
            txtPatchCore = CreateTextBox();
            txtDefectBank = CreateTextBox();
            nudAnomaly = CreateNumber(0.5M);
            nudSimilarity = CreateNumber(0.8M);
            chkEnabled = new CheckBox();
            chkEnabled.Text = "启用此产品";
            chkEnabled.Checked = true;
            chkEnabled.Dock = DockStyle.Fill;
            chkEnabled.ForeColor = UiTheme.TextPrimary;

            AddFormRow(form, 0, "产品名称", txtName, null);
            Button browsePatch = CreateButton("浏览...", false);
            browsePatch.Click += delegate { BrowseFolder(txtPatchCore); };
            AddFormRow(form, 1, "PatchCore 模型目录", txtPatchCore, browsePatch);

            Button browseDefect = CreateButton("浏览...", false);
            browseDefect.Click += delegate { BrowseFolder(txtDefectBank); };
            AddFormRow(form, 2, "Defect Bank 目录", txtDefectBank, browseDefect);
            AddFormRow(form, 3, "异常阈值", nudAnomaly, null);
            AddFormRow(form, 4, "分类相似度阈值", nudSimilarity, null);
            AddFormRow(form, 5, "状态", chkEnabled, null);

            Label hint = new Label();
            hint.Dock = DockStyle.Fill;
            hint.ForeColor = UiTheme.TextMuted;
            hint.Text = "这里只保存推理配置，不提供训练入口。产品保存后会立即同步到“检测工作台”的产品下拉框。";
            hint.TextAlign = ContentAlignment.MiddleLeft;
            form.Controls.Add(hint, 0, 6);
            form.SetColumnSpan(hint, 3);

            root.Controls.Add(form, 0, 1);

            FlowLayoutPanel footer = new FlowLayoutPanel();
            footer.Dock = DockStyle.Fill;
            footer.FlowDirection = FlowDirection.RightToLeft;
            Button save = CreateButton("保存配置", true);
            save.Click += btnSave_Click;
            lblStatus = new Label();
            lblStatus.AutoSize = false;
            lblStatus.Width = 420;
            lblStatus.Dock = DockStyle.Left;
            lblStatus.ForeColor = UiTheme.TextSecondary;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            footer.Controls.Add(save);
            footer.Controls.Add(lblStatus);
            root.Controls.Add(footer, 0, 2);

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
            txtPatchCore.Text = product.PatchCoreModelDirectory ?? string.Empty;
            txtDefectBank.Text = product.DefectBankDirectory ?? string.Empty;
            nudAnomaly.Value = ClampDecimal(product.AnomalyThreshold);
            nudSimilarity.Value = ClampDecimal(product.SimilarityThreshold);
            chkEnabled.Checked = product.Enabled;
            lblStatus.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string baseName = "NewProduct";
            string name = baseName;
            int suffix = 1;
            while (_products.Exists(delegate(ProductConfig p)
            {
                return string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase);
            }))
            {
                suffix++;
                name = baseName + suffix;
            }

            _products.Add(new ProductConfig
            {
                Name = name,
                AnomalyThreshold = 0.5D,
                SimilarityThreshold = 0.8D,
                Enabled = true
            });

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
                MessageBox.Show("至少保留一个产品配置。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = _products[index].Name;
            if (MessageBox.Show("确定删除产品“" + name + "”吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
                MessageBox.Show("产品名称不能为空。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ProductConfig product = _products[index];
            product.Name = txtName.Text.Trim();
            product.PatchCoreModelDirectory = txtPatchCore.Text.Trim();
            product.DefectBankDirectory = txtDefectBank.Text.Trim();
            product.AnomalyThreshold = (double)nudAnomaly.Value;
            product.SimilarityThreshold = (double)nudSimilarity.Value;
            product.Enabled = chkEnabled.Checked;

            try
            {
                _service.Save(_products);
                ReloadConfigurations();

                for (int i = 0; i < lstProducts.Items.Count; i++)
                {
                    if (string.Equals(lstProducts.Items[i].ToString(), product.Name,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        lstProducts.SelectedIndex = i;
                        break;
                    }
                }

                lblStatus.Text = "已保存，检测工作台产品列表已同步。";
                EventHandler handler = ConfigurationsChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearEditor()
        {
            if (txtName == null)
                return;
            txtName.Text = string.Empty;
            txtPatchCore.Text = string.Empty;
            txtDefectBank.Text = string.Empty;
            nudAnomaly.Value = 0.5M;
            nudSimilarity.Value = 0.8M;
            chkEnabled.Checked = true;
        }

        private static decimal ClampDecimal(double value)
        {
            decimal result;
            try { result = (decimal)value; }
            catch { result = 0M; }
            if (result < 0M) return 0M;
            if (result > 1000M) return 1000M;
            return result;
        }

        private static void BrowseFolder(TextBox target)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = target.Text;
                if (dialog.ShowDialog() == DialogResult.OK)
                    target.Text = dialog.SelectedPath;
            }
        }

        private static Panel CreatePanel(Padding padding)
        {
            Panel panel = new Panel();
            panel.BackColor = UiTheme.Surface;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Dock = DockStyle.Fill;
            panel.Padding = padding;
            return panel;
        }

        private static Label CreateTitle(string text)
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            label.ForeColor = UiTheme.TextPrimary;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            return label;
        }

        private static TextBox CreateTextBox()
        {
            TextBox text = new TextBox();
            text.BorderStyle = BorderStyle.FixedSingle;
            text.Dock = DockStyle.Fill;
            text.Font = new Font("Microsoft YaHei UI", 9F);
            text.Margin = new Padding(0, 10, 8, 10);
            return text;
        }

        private static NumericUpDown CreateNumber(decimal value)
        {
            NumericUpDown number = new NumericUpDown();
            number.DecimalPlaces = 4;
            number.Increment = 0.01M;
            number.Minimum = 0M;
            number.Maximum = 1000M;
            number.Value = value;
            number.Dock = DockStyle.Left;
            number.Width = 180;
            number.Margin = new Padding(0, 10, 0, 10);
            return number;
        }

        private static Button CreateButton(string text, bool primary)
        {
            Button button = new Button();
            button.BackColor = primary ? UiTheme.PrimaryButton : UiTheme.Surface;
            button.ForeColor = primary ? Color.White : UiTheme.TextPrimary;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = primary ? UiTheme.PrimaryButton : UiTheme.BorderStrong;
            button.FlatAppearance.BorderSize = 1;
            button.Font = new Font("Microsoft YaHei UI", 8.5F,
                primary ? FontStyle.Bold : FontStyle.Regular);
            button.Height = 30;
            button.Width = primary ? 96 : 86;
            button.Margin = new Padding(4, 5, 4, 5);
            button.Text = text;
            return button;
        }

        private static void AddFormRow(TableLayoutPanel form, int row, string caption,
            Control editor, Control action)
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.ForeColor = UiTheme.TextSecondary;
            label.Text = caption;
            label.TextAlign = ContentAlignment.MiddleLeft;
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
