using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using _180Detection.Models;
using _180Detection.Services;

namespace _180Detection
{
    public sealed class TabSystemSettings : UserControl
    {
        private readonly AppSettingsService _service = new AppSettingsService();
        private TextBox txtEngine;
        private TextBox txtProducts;
        private TextBox txtRecords;
        private TextBox txtImages;
        private TextBox txtLogs;
        private TextBox txtMvsSdk;
        private TextBox txtCameraModel;
        private CheckBox chkSaveOriginal;
        private CheckBox chkSaveMarked;
        private Label lblStatus;

        public event EventHandler SettingsSaved;

        public TabSystemSettings()
        {
            Font = new Font("Microsoft YaHei UI", 9F);
            BackColor = UiTheme.WindowBackground;
            Dock = DockStyle.Fill;
            BuildUi();
            ReloadSettings();
        }

        public void ReloadSettings()
        {
            AppSettings settings = _service.Load();
            txtEngine.Text = settings.EngineDirectory ?? "engine";
            txtProducts.Text = settings.ProductsRoot ?? "products";
            txtRecords.Text = settings.RecordsRoot ?? "records";
            txtImages.Text = settings.ImagesRoot ?? "images";
            txtLogs.Text = settings.LogsRoot ?? "logs";
            txtMvsSdk.Text = settings.HikCameraSdkAssembly ?? string.Empty;
            txtCameraModel.Text = string.IsNullOrWhiteSpace(settings.CameraExpectedModel)
                ? "MV-CS200-10GM"
                : settings.CameraExpectedModel;
            chkSaveOriginal.Checked = settings.SaveOriginalImage;
            chkSaveMarked.Checked = settings.SaveMarkedImage;
            lblStatus.Text = "配置文件：" + _service.ConfigPath;
        }

        private void BuildUi()
        {
            Panel surface = new Panel
            {
                BackColor = UiTheme.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Padding = new Padding(18)
            };

            TableLayoutPanel root = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 4,
                Dock = DockStyle.Fill
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

            root.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
                ForeColor = UiTheme.TextPrimary,
                Text = "ONNX 引擎、数据目录与设备设置",
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);

            Panel scroll = new Panel { AutoScroll = true, Dock = DockStyle.Fill };
            TableLayoutPanel form = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 3,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 20, 20)
            };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 700F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));

            txtEngine = CreateTextBox();
            txtProducts = CreateTextBox();
            txtRecords = CreateTextBox();
            txtImages = CreateTextBox();
            txtLogs = CreateTextBox();
            txtMvsSdk = CreateTextBox();
            txtCameraModel = CreateTextBox();
            chkSaveOriginal = new CheckBox { Text = "保存原始图像", Dock = DockStyle.Fill };
            chkSaveMarked = new CheckBox { Text = "保存 marked 结果图", Dock = DockStyle.Fill };

            int row = 0;
            AddSection(form, ref row, "ONNX Runtime");
            AddPathRow(form, ref row, "Engine Directory", txtEngine, true);
            AddPathRow(form, ref row, "Products Root", txtProducts, true);
            AddSection(form, ref row, "检测数据");
            AddPathRow(form, ref row, "Records Root", txtRecords, true);
            AddPathRow(form, ref row, "Images Root", txtImages, true);
            AddPathRow(form, ref row, "Logs Root", txtLogs, true);
            AddRow(form, ref row, "保存原图", chkSaveOriginal, null);
            AddRow(form, ref row, "保存结果图", chkSaveMarked, null);
            AddSection(form, ref row, "海康相机");
            AddPathRow(form, ref row, "MVS .NET SDK DLL", txtMvsSdk, false);
            AddRow(form, ref row, "目标相机型号", txtCameraModel, null);

            Label hint = new Label
            {
                AutoSize = true,
                ForeColor = UiTheme.TextMuted,
                Margin = new Padding(0, 12, 0, 12),
                Text = "正式推理不再依赖 Python / Anaconda / PyTorch。Engine Directory 应包含 patchcore_feature.onnx、dinov2_feature.onnx、engine_config.json。"
            };
            form.RowCount = row + 1;
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            form.Controls.Add(hint, 0, row);
            form.SetColumnSpan(hint, 3);

            scroll.Controls.Add(form);
            root.Controls.Add(scroll, 0, 1);
            root.Controls.Add(new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Border }, 0, 2);

            TableLayoutPanel footer = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112F));
            lblStatus = new Label
            {
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                ForeColor = UiTheme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Button save = CreateButton("保存设置", true);
            save.Dock = DockStyle.Fill;
            save.Margin = new Padding(8, 10, 0, 10);
            save.Click += btnSave_Click;
            footer.Controls.Add(lblStatus, 0, 0);
            footer.Controls.Add(save, 1, 0);
            root.Controls.Add(footer, 0, 3);

            surface.Controls.Add(root);
            Controls.Add(surface);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEngine.Text))
            {
                MessageBox.Show("Engine Directory 不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                AppSettings current = _service.Load();
                current.EngineDirectory = txtEngine.Text.Trim();
                current.ProductsRoot = txtProducts.Text.Trim();
                current.RecordsRoot = txtRecords.Text.Trim();
                current.ImagesRoot = txtImages.Text.Trim();
                current.LogsRoot = txtLogs.Text.Trim();
                current.HikCameraSdkAssembly = txtMvsSdk.Text.Trim();
                current.CameraExpectedModel = string.IsNullOrWhiteSpace(txtCameraModel.Text)
                    ? "MV-CS200-10GM"
                    : txtCameraModel.Text.Trim();
                current.SaveOriginalImage = chkSaveOriginal.Checked;
                current.SaveMarkedImage = chkSaveMarked.Checked;
                _service.Save(current);
                lblStatus.Text = "设置已保存 · ONNX 引擎将在当前程序中重新加载";
                SettingsSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存设置失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void AddSection(TableLayoutPanel form, ref int row, string text)
        {
            form.RowCount = row + 1;
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            Label title = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold),
                ForeColor = UiTheme.TextPrimary,
                Text = text,
                TextAlign = ContentAlignment.BottomLeft
            };
            form.Controls.Add(title, 0, row);
            form.SetColumnSpan(title, 3);
            row++;
        }

        private static void AddPathRow(TableLayoutPanel form, ref int row, string caption, TextBox editor, bool folder)
        {
            Button browse = CreateButton("浏览...", false);
            browse.Dock = DockStyle.Fill;
            browse.Margin = new Padding(4, 9, 0, 9);
            browse.Click += delegate
            {
                if (folder)
                {
                    using FolderBrowserDialog dialog = new FolderBrowserDialog();
                    if (Directory.Exists(editor.Text))
                        dialog.SelectedPath = editor.Text;
                    if (dialog.ShowDialog() == DialogResult.OK)
                        editor.Text = dialog.SelectedPath;
                }
                else
                {
                    using OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "DLL 文件|*.dll|所有文件|*.*";
                    if (File.Exists(editor.Text))
                        dialog.FileName = editor.Text;
                    if (dialog.ShowDialog() == DialogResult.OK)
                        editor.Text = dialog.FileName;
                }
            };
            AddRow(form, ref row, caption, editor, browse);
        }

        private static void AddRow(TableLayoutPanel form, ref int row, string caption, Control editor, Control action)
        {
            form.RowCount = row + 1;
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            Label label = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = UiTheme.TextSecondary,
                Text = caption,
                TextAlign = ContentAlignment.MiddleLeft
            };
            form.Controls.Add(label, 0, row);
            form.Controls.Add(editor, 1, row);
            if (action != null)
                form.Controls.Add(action, 2, row);
            row++;
        }

        private static TextBox CreateTextBox()
        {
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft YaHei UI", 9F),
                Margin = new Padding(0, 10, 8, 10)
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
            button.Text = text;
            button.UseVisualStyleBackColor = false;
            return button;
        }
    }
}
