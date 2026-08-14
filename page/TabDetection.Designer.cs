using System.Drawing;
using System.Windows.Forms;

namespace _180Detection
{
    partial class TabDetection
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel rootLayout;
        private Panel panelToolbar;
        private ComboBox cmbProduct;
        private Label lblModelState;
        private Label lblCameraState;
        private Button btnCameraConnect;
        private Button btnChooseImage;
        private Button btnDetect;
        private Button btnOpenDirectory;
        private SmoothZoomPictureBox pictureResult;
        private Label lblViewerHint;
        private Button btnFitImage;
        private Label lblResultState;
        private Label lblDefectValue;
        private Label lblScoreValue;
        private Label lblSimilarityValue;
        private Label lblTimeValue;
        private Label lblFileValue;
        private Label lblServiceHint;
        private Label lblStatus;
        private Label lblElapsed;
        private Label lblStatusFile;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            rootLayout = new TableLayoutPanel();
            panelToolbar = new Panel();
            cmbProduct = new ComboBox();
            lblModelState = new Label();
            lblCameraState = new Label();
            btnCameraConnect = CreateToolbarButton("连接相机", 86, false);
            btnChooseImage = CreateToolbarButton("选择图片", 96, false);
            btnDetect = CreateToolbarButton("开始检测", 96, true);
            btnOpenDirectory = CreateToolbarButton("打开目录", 96, false);
            pictureResult = new SmoothZoomPictureBox();
            lblViewerHint = new Label();
            btnFitImage = CreateToolbarButton("适应窗口", 82, false);
            lblResultState = new Label();
            lblDefectValue = CreateValueLabel(false);
            lblScoreValue = CreateValueLabel(true);
            lblSimilarityValue = CreateValueLabel(true);
            lblTimeValue = CreateValueLabel(false);
            lblFileValue = CreateValueLabel(false);
            lblServiceHint = new Label();
            lblStatus = CreateStatusLabel(ContentAlignment.MiddleLeft);
            lblElapsed = CreateStatusLabel(ContentAlignment.MiddleCenter);
            lblStatusFile = CreateStatusLabel(ContentAlignment.MiddleRight);

            SuspendLayout();
            BuildRootLayout();
            BuildToolbar();
            BuildMainArea();
            BuildStatusBar();

            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiTheme.WindowBackground;
            Controls.Add(rootLayout);
            Font = new Font(
                "Microsoft YaHei UI",
                9F,
                FontStyle.Regular,
                GraphicsUnit.Point,
                134);
            Margin = Padding.Empty;
            Name = "TabDetection";
            Size = new Size(1194, 796);
            ResumeLayout(false);
        }

        private void BuildRootLayout()
        {
            rootLayout.BackColor = UiTheme.WindowBackground;
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowCount = 3;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Margin = Padding.Empty;
            rootLayout.Controls.Add(panelToolbar, 0, 0);
        }

        private void BuildToolbar()
        {
            panelToolbar.BackColor = UiTheme.Surface;
            panelToolbar.Dock = DockStyle.Fill;
            panelToolbar.Margin = new Padding(0, 0, 0, 8);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.BackColor = UiTheme.Surface;
            layout.ColumnCount = 3;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            layout.Dock = DockStyle.Fill;
            layout.Margin = Padding.Empty;
            layout.Padding = new Padding(12, 6, 10, 5);

            FlowLayoutPanel productFlow = new FlowLayoutPanel();
            productFlow.BackColor = UiTheme.Surface;
            productFlow.Dock = DockStyle.Fill;
            productFlow.Margin = Padding.Empty;
            productFlow.WrapContents = false;

            Label productCaption = CreateToolbarCaption("产品");
            productCaption.Margin = new Padding(0, 7, 8, 0);

            cmbProduct.BackColor = Color.White;
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.FlatStyle = FlatStyle.Flat;
            cmbProduct.Font = new Font("Microsoft YaHei UI", 9F);
            cmbProduct.ForeColor = UiTheme.TextPrimary;
            cmbProduct.Margin = new Padding(0, 2, 18, 0);
            cmbProduct.Size = new Size(145, 28);
            cmbProduct.SelectedIndexChanged +=
                new System.EventHandler(cmbProduct_SelectedIndexChanged);

            lblModelState.AutoSize = true;
            lblModelState.Font = new Font("Microsoft YaHei UI", 8F);
            lblModelState.ForeColor = UiTheme.TextMuted;
            lblModelState.Margin = new Padding(0, 7, 0, 0);
            lblModelState.Text = "○ 推理脚本未配置";

            productFlow.Controls.AddRange(
                new Control[] { productCaption, cmbProduct, lblModelState });

            FlowLayoutPanel cameraFlow = new FlowLayoutPanel();
            cameraFlow.BackColor = UiTheme.Surface;
            cameraFlow.Dock = DockStyle.Fill;
            cameraFlow.Margin = Padding.Empty;
            cameraFlow.WrapContents = false;

            Label cameraCaption = CreateToolbarCaption("相机");
            cameraCaption.Margin = new Padding(0, 7, 8, 0);

            lblCameraState.AutoSize = true;
            lblCameraState.Font = new Font("Microsoft YaHei UI", 8F);
            lblCameraState.ForeColor = UiTheme.TextMuted;
            lblCameraState.Margin = new Padding(0, 7, 10, 0);
            lblCameraState.Text = "○ MV-CS200-10GM 未连接";

            btnCameraConnect.Margin = new Padding(0, 0, 0, 0);
            btnCameraConnect.Click +=
                new System.EventHandler(btnCameraConnect_Click);

            cameraFlow.Controls.AddRange(
                new Control[] { cameraCaption, lblCameraState, btnCameraConnect });

            FlowLayoutPanel actionFlow = new FlowLayoutPanel();
            actionFlow.BackColor = UiTheme.Surface;
            actionFlow.Dock = DockStyle.Fill;
            actionFlow.FlowDirection = FlowDirection.RightToLeft;
            actionFlow.Margin = Padding.Empty;
            actionFlow.WrapContents = false;

            btnOpenDirectory.Margin = new Padding(6, 0, 0, 0);
            btnDetect.Margin = new Padding(6, 0, 0, 0);
            btnChooseImage.Margin = new Padding(6, 0, 0, 0);

            btnOpenDirectory.Click +=
                new System.EventHandler(btnOpenDirectory_Click);
            btnDetect.Click +=
                new System.EventHandler(btnDetect_Click);
            btnChooseImage.Click +=
                new System.EventHandler(btnChooseImage_Click);

            actionFlow.Controls.AddRange(
                new Control[] { btnOpenDirectory, btnDetect, btnChooseImage });

            layout.Controls.Add(productFlow, 0, 0);
            layout.Controls.Add(cameraFlow, 1, 0);
            layout.Controls.Add(actionFlow, 2, 0);
            panelToolbar.Controls.Add(layout);
        }

        private static Label CreateToolbarCaption(string text)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Microsoft YaHei UI", 9F);
            label.ForeColor = UiTheme.TextSecondary;
            label.Text = text;
            return label;
        }

        private void BuildMainArea()
        {
            TableLayoutPanel main = new TableLayoutPanel();
            main.BackColor = UiTheme.WindowBackground;
            main.ColumnCount = 2;
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320F));
            main.RowCount = 1;
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            main.Dock = DockStyle.Fill;
            main.Margin = Padding.Empty;
            main.Controls.Add(BuildViewerPanel(), 0, 0);
            main.Controls.Add(BuildResultPanel(), 1, 0);
            rootLayout.Controls.Add(main, 0, 1);
        }

        private Control BuildViewerPanel()
        {
            Panel panel = new Panel();
            panel.BackColor = UiTheme.Surface;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(0, 0, 8, 0);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.BackColor = UiTheme.Surface;
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowCount = 2;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.Dock = DockStyle.Fill;
            layout.Margin = Padding.Empty;

            Panel header = new Panel();
            header.BackColor = UiTheme.Surface;
            header.Dock = DockStyle.Fill;

            Label title = new Label();
            title.AutoSize = true;
            title.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            title.ForeColor = UiTheme.TextPrimary;
            title.Location = new Point(12, 10);
            title.Text = "检测图像";

            Label subtitle = new Label();
            subtitle.AutoSize = true;
            subtitle.Font = new Font("Microsoft YaHei UI", 8F);
            subtitle.ForeColor = UiTheme.TextMuted;
            subtitle.Location = new Point(88, 11);
            subtitle.Text = "滚轮缩放 / 中键拖动 / 双击复位";

            btnFitImage.Dock = DockStyle.Right;
            btnFitImage.Width = 88;
            btnFitImage.Click += new System.EventHandler(btnFitImage_Click);

            header.Controls.AddRange(new Control[] { title, subtitle, btnFitImage });

            Panel viewport = new Panel();
            viewport.BackColor = UiTheme.Viewer;
            viewport.Dock = DockStyle.Fill;
            viewport.Margin = Padding.Empty;
            viewport.Padding = new Padding(6);

            pictureResult.BackColor = UiTheme.Viewer;
            pictureResult.Dock = DockStyle.Fill;

            lblViewerHint.BackColor = Color.Transparent;
            lblViewerHint.Dock = DockStyle.Fill;
            lblViewerHint.Font = new Font("Microsoft YaHei UI", 10F);
            lblViewerHint.ForeColor = UiTheme.ViewerText;
            lblViewerHint.Text =
                "请选择待检测图片\r\n相机接入后可直接使用采集图像";
            lblViewerHint.TextAlign = ContentAlignment.MiddleCenter;

            viewport.Controls.Add(pictureResult);
            viewport.Controls.Add(lblViewerHint);
            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(viewport, 0, 1);
            panel.Controls.Add(layout);
            return panel;
        }

        private Control BuildResultPanel()
        {
            Panel panel = new Panel();
            panel.BackColor = UiTheme.Surface;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Dock = DockStyle.Fill;
            panel.Margin = Padding.Empty;

            TableLayoutPanel grid = new TableLayoutPanel();
            grid.BackColor = UiTheme.Surface;
            grid.ColumnCount = 2;
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            grid.RowCount = 10;
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 74F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            grid.Dock = DockStyle.Fill;
            grid.Padding = new Padding(14, 0, 14, 0);

            Label title = new Label();
            title.Dock = DockStyle.Fill;
            title.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            title.ForeColor = UiTheme.TextPrimary;
            title.Text = "检测结果";
            title.TextAlign = ContentAlignment.MiddleLeft;

            lblResultState.Dock = DockStyle.Fill;
            lblResultState.Font = new Font("Microsoft YaHei UI", 23F, FontStyle.Bold);
            lblResultState.ForeColor = UiTheme.TextSecondary;
            lblResultState.Text = "--";
            lblResultState.TextAlign = ContentAlignment.MiddleLeft;

            Panel separator = new Panel();
            separator.BackColor = UiTheme.Border;
            separator.Dock = DockStyle.Fill;
            separator.Margin = Padding.Empty;

            grid.Controls.Add(title, 0, 0);
            grid.SetColumnSpan(title, 2);
            grid.Controls.Add(lblResultState, 0, 1);
            grid.SetColumnSpan(lblResultState, 2);
            grid.Controls.Add(separator, 0, 2);
            grid.SetColumnSpan(separator, 2);

            AddResultRow(grid, 3, "异常类型", lblDefectValue);
            AddResultRow(grid, 4, "PatchCore Score", lblScoreValue);
            AddResultRow(grid, 5, "分类相似度", lblSimilarityValue);
            AddResultRow(grid, 6, "检测时间", lblTimeValue);
            AddResultRow(grid, 7, "文件名", lblFileValue);

            lblServiceHint.AutoEllipsis = true;
            lblServiceHint.Dock = DockStyle.Fill;
            lblServiceHint.Font = new Font("Microsoft YaHei UI", 8F);
            lblServiceHint.ForeColor = UiTheme.TextMuted;
            lblServiceHint.Text = "Python 推理接口已接入";
            lblServiceHint.TextAlign = ContentAlignment.MiddleLeft;
            grid.Controls.Add(lblServiceHint, 0, 9);
            grid.SetColumnSpan(lblServiceHint, 2);

            panel.Controls.Add(grid);
            return panel;
        }

        private void BuildStatusBar()
        {
            Panel status = new Panel();
            status.BackColor = UiTheme.Surface;
            status.Dock = DockStyle.Fill;
            status.Margin = new Padding(0, 8, 0, 0);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.BackColor = UiTheme.Surface;
            layout.ColumnCount = 3;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            layout.RowCount = 1;
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(10, 0, 10, 0);
            layout.Controls.Add(lblStatus, 0, 0);
            layout.Controls.Add(lblElapsed, 1, 0);
            layout.Controls.Add(lblStatusFile, 2, 0);
            status.Controls.Add(layout);
            rootLayout.Controls.Add(status, 0, 2);
        }

        private static Button CreateToolbarButton(string text, int width, bool primary)
        {
            Button button = new Button();
            button.BackColor = primary ? UiTheme.PrimaryButton : UiTheme.Surface;
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = primary
                ? UiTheme.PrimaryButton
                : UiTheme.BorderStrong;
            button.FlatAppearance.MouseOverBackColor = primary
                ? UiTheme.PrimaryButtonHover
                : UiTheme.NavigationHover;
            button.FlatAppearance.MouseDownBackColor = primary
                ? Color.FromArgb(30, 30, 30)
                : UiTheme.NavigationPressed;
            button.Font = new Font(
                "Microsoft YaHei UI",
                8.5F,
                primary ? FontStyle.Bold : FontStyle.Regular);
            button.ForeColor = primary ? Color.White : UiTheme.TextPrimary;
            button.Size = new Size(width, 30);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
            return button;
        }

        private static Label CreateValueLabel(bool monospace)
        {
            Label label = new Label();
            label.AutoEllipsis = true;
            label.Dock = DockStyle.Fill;
            label.Font = monospace
                ? new Font("Consolas", 10F, FontStyle.Regular)
                : new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
            label.ForeColor = UiTheme.TextPrimary;
            label.Text = "--";
            label.TextAlign = ContentAlignment.MiddleRight;
            return label;
        }

        private static Label CreateStatusLabel(ContentAlignment alignment)
        {
            Label label = new Label();
            label.AutoEllipsis = true;
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Microsoft YaHei UI", 8F);
            label.ForeColor = UiTheme.TextSecondary;
            label.TextAlign = alignment;
            return label;
        }

        private static void AddResultRow(
            TableLayoutPanel grid,
            int row,
            string caption,
            Label value)
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Microsoft YaHei UI", 8.5F);
            label.ForeColor = UiTheme.TextSecondary;
            label.Text = caption;
            label.TextAlign = ContentAlignment.MiddleLeft;
            grid.Controls.Add(label, 0, row);
            grid.Controls.Add(value, 1, row);
        }
    }
}
