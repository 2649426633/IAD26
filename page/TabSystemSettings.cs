using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace _180Detection
{
    public sealed class TabSystemSettings : UserControl
    {
        private TextBox txtPython;
        private TextBox txtScript;
        private TextBox txtWorking;
        private TextBox txtResults;
        private NumericUpDown nudTimeout;
        private TextBox txtArguments;
        private TextBox txtMvsSdk;
        private TextBox txtCameraModel;
        private TextBox txtLogDirectory;
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
            txtPython.Text = Get("PythonExecutable", "python");
            txtScript.Text = Get("InferenceScript", string.Empty);
            txtWorking.Text = Get("InferenceWorkingDirectory", string.Empty);
            txtResults.Text = Get("InferenceResultDirectory", @"runtime\results");
            txtArguments.Text = Get("InferenceArgumentsTemplate",
                "\"{script}\" --image \"{image}\" --product \"{product}\" --output \"{output}\"");
            txtMvsSdk.Text = Get("HikCameraSdkAssembly", string.Empty);
            txtCameraModel.Text = Get("CameraExpectedModel", "MV-CS200-10GM");
            txtLogDirectory.Text = Get("LogDirectory", @"runtime\logs");
            chkSaveOriginal.Checked = GetBool("SaveOriginalImage", true);
            chkSaveMarked.Checked = GetBool("SaveMarkedImage", true);

            int timeout;
            if (!int.TryParse(Get("InferenceTimeoutSeconds", "120"), out timeout))
                timeout = 120;
            nudTimeout.Value = Math.Max(nudTimeout.Minimum,
                Math.Min(nudTimeout.Maximum, timeout));

            lblStatus.Text = "当前设置来自：" +
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
        }

        private void BuildUi()
        {
            Panel surface = new Panel();
            surface.BackColor = UiTheme.Surface;
            surface.BorderStyle = BorderStyle.FixedSingle;
            surface.Dock = DockStyle.Fill;
            surface.Padding = new Padding(18);

            TableLayoutPanel root = new TableLayoutPanel();
            root.ColumnCount = 1;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowCount = 4;
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            root.Dock = DockStyle.Fill;

            Label title = new Label();
            title.Dock = DockStyle.Fill;
            title.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            title.ForeColor = UiTheme.TextPrimary;
            title.Text = "运行环境与设备设置";
            title.TextAlign = ContentAlignment.MiddleLeft;
            root.Controls.Add(title, 0, 0);

            Panel scroll = new Panel();
            scroll.AutoScroll = true;
            scroll.Dock = DockStyle.Fill;

            TableLayoutPanel form = new TableLayoutPanel();
            form.AutoSize = true;
            form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            form.ColumnCount = 3;
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 700F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92F));
            form.Dock = DockStyle.Top;
            form.Padding = new Padding(0, 0, 20, 20);

            txtPython = CreateTextBox();
            txtScript = CreateTextBox();
            txtWorking = CreateTextBox();
            txtResults = CreateTextBox();
            txtArguments = CreateTextBox();
            txtMvsSdk = CreateTextBox();
            txtCameraModel = CreateTextBox();
            txtLogDirectory = CreateTextBox();

            nudTimeout = new NumericUpDown();
            nudTimeout.Minimum = 5;
            nudTimeout.Maximum = 3600;
            nudTimeout.Value = 120;
            nudTimeout.Width = 180;
            nudTimeout.Margin = new Padding(0, 10, 0, 10);

            chkSaveOriginal = new CheckBox();
            chkSaveOriginal.Text = "保存原始图像";
            chkSaveOriginal.Dock = DockStyle.Fill;
            chkSaveMarked = new CheckBox();
            chkSaveMarked.Text = "保存 marked 结果图";
            chkSaveMarked.Dock = DockStyle.Fill;

            int row = 0;
            AddSection(form, ref row, "Python 推理");
            AddPathRow(form, ref row, "Python 解释器", txtPython, false);
            AddPathRow(form, ref row, "推理脚本", txtScript, false);
            AddPathRow(form, ref row, "工作目录", txtWorking, true);
            AddPathRow(form, ref row, "结果目录", txtResults, true);
            AddRow(form, ref row, "超时时间（秒）", nudTimeout, null);
            AddRow(form, ref row, "参数模板", txtArguments, null);

            AddSection(form, ref row, "海康相机");
            AddPathRow(form, ref row, "MVS .NET SDK DLL", txtMvsSdk, false);
            AddRow(form, ref row, "目标相机型号", txtCameraModel, null);

            AddSection(form, ref row, "保存与日志");
            AddPathRow(form, ref row, "日志目录", txtLogDirectory, true);
            AddRow(form, ref row, "保存原图", chkSaveOriginal, null);
            AddRow(form, ref row, "保存结果图", chkSaveMarked, null);

            scroll.Controls.Add(form);
            root.Controls.Add(scroll, 0, 1);

            Panel separator = new Panel();
            separator.Dock = DockStyle.Fill;
            separator.BackColor = UiTheme.Border;
            root.Controls.Add(separator, 0, 2);

            TableLayoutPanel footer = new TableLayoutPanel();
            footer.ColumnCount = 2;
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            footer.Dock = DockStyle.Fill;

            lblStatus = new Label();
            lblStatus.AutoEllipsis = true;
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.ForeColor = UiTheme.TextSecondary;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;

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
            if (string.IsNullOrWhiteSpace(txtPython.Text))
            {
                MessageBox.Show("Python 解释器不能为空。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection settings = config.AppSettings.Settings;

                Set(settings, "PythonExecutable", txtPython.Text.Trim());
                Set(settings, "InferenceScript", txtScript.Text.Trim());
                Set(settings, "InferenceWorkingDirectory", txtWorking.Text.Trim());
                Set(settings, "InferenceResultDirectory", txtResults.Text.Trim());
                Set(settings, "InferenceTimeoutSeconds", ((int)nudTimeout.Value).ToString());
                Set(settings, "InferenceArgumentsTemplate", txtArguments.Text.Trim());
                Set(settings, "HikCameraSdkAssembly", txtMvsSdk.Text.Trim());
                Set(settings, "CameraExpectedModel", txtCameraModel.Text.Trim());
                Set(settings, "LogDirectory", txtLogDirectory.Text.Trim());
                Set(settings, "SaveOriginalImage", chkSaveOriginal.Checked ? "true" : "false");
                Set(settings, "SaveMarkedImage", chkSaveMarked.Checked ? "true" : "false");

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                lblStatus.Text = "设置已保存并重新加载。";
                EventHandler handler = SettingsSaved;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存设置失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void AddSection(TableLayoutPanel form, ref int row, string text)
        {
            form.RowCount = row + 1;
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            Label title = new Label();
            title.Dock = DockStyle.Fill;
            title.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            title.ForeColor = UiTheme.TextPrimary;
            title.Text = text;
            title.TextAlign = ContentAlignment.BottomLeft;
            form.Controls.Add(title, 0, row);
            form.SetColumnSpan(title, 3);
            row++;
        }

        private static void AddPathRow(TableLayoutPanel form, ref int row,
            string caption, TextBox editor, bool folder)
        {
            Button browse = CreateButton("浏览...", false);
            browse.Dock = DockStyle.Fill;
            browse.Margin = new Padding(4, 9, 0, 9);
            browse.Click += delegate
            {
                if (folder)
                {
                    using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                    {
                        dialog.SelectedPath = editor.Text;
                        if (dialog.ShowDialog() == DialogResult.OK)
                            editor.Text = dialog.SelectedPath;
                    }
                }
                else
                {
                    using (OpenFileDialog dialog = new OpenFileDialog())
                    {
                        dialog.FileName = editor.Text;
                        dialog.Filter = "所有文件|*.*";
                        if (dialog.ShowDialog() == DialogResult.OK)
                            editor.Text = dialog.FileName;
                    }
                }
            };
            AddRow(form, ref row, caption, editor, browse);
        }

        private static void AddRow(TableLayoutPanel form, ref int row,
            string caption, Control editor, Control action)
        {
            form.RowCount = row + 1;
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.ForeColor = UiTheme.TextSecondary;
            label.Text = caption;
            label.TextAlign = ContentAlignment.MiddleLeft;

            form.Controls.Add(label, 0, row);
            form.Controls.Add(editor, 1, row);
            if (action != null)
                form.Controls.Add(action, 2, row);
            row++;
        }

        private static TextBox CreateTextBox()
        {
            TextBox box = new TextBox();
            box.BorderStyle = BorderStyle.FixedSingle;
            box.Dock = DockStyle.Fill;
            box.Font = new Font("Microsoft YaHei UI", 9F);
            box.Margin = new Padding(0, 10, 8, 10);
            return box;
        }

        private static Button CreateButton(string text, bool primary)
        {
            Button button = new Button();
            button.BackColor = primary ? UiTheme.PrimaryButton : UiTheme.Surface;
            button.ForeColor = primary ? Color.White : UiTheme.TextPrimary;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor =
                primary ? UiTheme.PrimaryButton : UiTheme.BorderStrong;
            button.Font = new Font("Microsoft YaHei UI", 8.5F,
                primary ? FontStyle.Bold : FontStyle.Regular);
            button.Text = text;
            return button;
        }

        private static string Get(string key, string fallback)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        private static bool GetBool(string key, bool fallback)
        {
            bool value;
            return bool.TryParse(ConfigurationManager.AppSettings[key], out value)
                ? value
                : fallback;
        }

        private static void Set(KeyValueConfigurationCollection settings,
            string key, string value)
        {
            if (settings[key] == null)
                settings.Add(key, value ?? string.Empty);
            else
                settings[key].Value = value ?? string.Empty;
        }
    }
}
