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
            btnChooseImage = CreateToolbarButton("选择图片", 112, false);
            btnDetect = CreateToolbarButton("检测", 90, true);
            btnOpenDirectory = CreateToolbarButton("打开目录", 112, false);
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
            BackColor = Color.FromArgb(244, 247, 250);
            Controls.Add(rootLayout);
            Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Margin = Padding.Empty;
            Name = "TabDetection";
            Size = new Size(1194, 796);
            ResumeLayout(false);
        }

        private void BuildRootLayout()
        {
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowCount = 3;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Margin = Padding.Empty;
            rootLayout.Controls.Add(panelToolbar, 0, 0);
        }

        private void BuildToolbar()
        {
            panelToolbar.BackColor = Color.White;
            panelToolbar.Dock = DockStyle.Fill;
            panelToolbar.Margin = new Padding(0, 0, 0, 12);
            panelToolbar.Padding = new Padding(16, 8, 12, 8);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.ColumnCount = 2;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            layout.Dock = DockStyle.Fill;
            layout.Margin = Padding.Empty;

            FlowLayoutPanel productFlow = new FlowLayoutPanel();
            productFlow.Dock = DockStyle.Fill;
            productFlow.Margin = Padding.Empty;
            productFlow.WrapContents = false;

            Label productCaption = new Label();
            productCaption.AutoSize = true;
            productCaption.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            productCaption.ForeColor = Color.FromArgb(60, 71, 84);
            productCaption.Margin = new Padding(0, 7, 8, 0);
            productCaption.Text = "产品";

            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.Font = new Font("Microsoft YaHei UI", 9.5F);
            cmbProduct.Margin = new Padding(0, 2, 18, 0);
            cmbProduct.Size = new Size(164, 29);
            cmbProduct.SelectedIndexChanged += new System.EventHandler(cmbProduct_SelectedIndexChanged);

            lblModelState.AutoSize = true;
            lblModelState.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            lblModelState.ForeColor = Color.FromArgb(229, 152, 52);
            lblModelState.Margin = new Padding(0, 7, 0, 0);
            lblModelState.Text = "● 模型待接入";

            productFlow.Controls.AddRange(new Control[] { productCaption, cmbProduct, lblModelState });

            FlowLayoutPanel actionFlow = new FlowLayoutPanel();
            actionFlow.Dock = DockStyle.Fill;
            actionFlow.FlowDirection = FlowDirection.RightToLeft;
            actionFlow.Margin = Padding.Empty;
            actionFlow.WrapContents = false;

            btnOpenDirectory.Margin = new Padding(8, 1, 0, 0);
            btnDetect.Margin = new Padding(8, 1, 0, 0);
            btnChooseImage.Margin = new Padding(8, 1, 0, 0);
            btnOpenDirectory.Click += new System.EventHandler(btnOpenDirectory_Click);
            btnDetect.Click += new System.EventHandler(btnDetect_Click);
            btnChooseImage.Click += new System.EventHandler(btnChooseImage_Click);
            actionFlow.Controls.AddRange(new Control[] { btnOpenDirectory, btnDetect, btnChooseImage });

            layout.Controls.Add(productFlow, 0, 0);
            layout.Controls.Add(actionFlow, 1, 0);
            panelToolbar.Controls.Add(layout);
        }

        private void BuildMainArea()
        {
            TableLayoutPanel main = new TableLayoutPanel();
            main.ColumnCount = 2;
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350F));
            main.RowCount = 1;
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            main.Dock = DockStyle.Fill;
            main.Margin = Padding.Empty;
            main.Controls.Add(BuildViewerCard(), 0, 0);
            main.Controls.Add(BuildResultCard(), 1, 0);
            rootLayout.Controls.Add(main, 0, 1);
        }

        private Control BuildViewerCard()
        {
            Panel card = new Panel();
            card.BackColor = Color.White;
            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(0, 0, 12, 0);
            card.Padding = new Padding(1);

            Panel header = new Panel();
            header.BackColor = Color.White;
            header.Dock = DockStyle.Top;
            header.Height = 46;

            Label title = new Label();
            title.AutoSize = true;
            title.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(50, 62, 75);
            title.Location = new Point(14, 12);
            title.Text = "最终检测结果";

            Label subtitle = new Label();
            subtitle.AutoSize = true;
            subtitle.Font = new Font("Microsoft YaHei UI", 8.5F);
            subtitle.ForeColor = Color.FromArgb(129, 140, 153);
            subtitle.Location = new Point(130, 14);
            subtitle.Text = "滚轮缩放 · 中键拖动 · 双击复位";

            btnFitImage.Dock = DockStyle.Right;
            btnFitImage.Width = 92;
            btnFitImage.Click += new System.EventHandler(btnFitImage_Click);
            header.Controls.AddRange(new Control[] { title, subtitle, btnFitImage });

            Panel viewport = new Panel();
            viewport.BackColor = Color.FromArgb(31, 35, 41);
            viewport.Dock = DockStyle.Fill;
            viewport.Padding = new Padding(10);

            pictureResult.BackColor = Color.FromArgb(31, 35, 41);
            pictureResult.Dock = DockStyle.Fill;

            lblViewerHint.BackColor = Color.Transparent;
            lblViewerHint.Dock = DockStyle.Fill;
            lblViewerHint.Font = new Font("Microsoft YaHei UI", 12F);
            lblViewerHint.ForeColor = Color.FromArgb(150, 160, 172);
            lblViewerHint.Text = "选择图片后在此显示检测结果\r\n正式推理接入后仅显示最终 marked 图片";
            lblViewerHint.TextAlign = ContentAlignment.MiddleCenter;

            viewport.Controls.Add(pictureResult);
            viewport.Controls.Add(lblViewerHint);
            card.Controls.Add(viewport);
            card.Controls.Add(header);
            return card;
        }

        private Control BuildResultCard()
        {
            Panel card = new Panel();
            card.BackColor = Color.White;
            card.Dock = DockStyle.Fill;
            card.Margin = Padding.Empty;
            card.Padding = new Padding(18, 12, 18, 12);

            TableLayoutPanel grid = new TableLayoutPanel();
            grid.ColumnCount = 2;
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            grid.RowCount = 9;
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            grid.Dock = DockStyle.Fill;

            Label title = new Label();
            title.Dock = DockStyle.Fill;
            title.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(48, 60, 74);
            title.Text = "检测结果";
            title.TextAlign = ContentAlignment.MiddleLeft;

            lblResultState.Dock = DockStyle.Fill;
            lblResultState.Font = new Font("Microsoft YaHei UI", 28F, FontStyle.Bold);
            lblResultState.ForeColor = Color.FromArgb(105, 116, 130);
            lblResultState.Text = "--";
            lblResultState.TextAlign = ContentAlignment.MiddleCenter;

            grid.Controls.Add(title, 0, 0);
            grid.SetColumnSpan(title, 2);
            grid.Controls.Add(lblResultState, 0, 1);
            grid.SetColumnSpan(lblResultState, 2);
            AddResultRow(grid, 2, "异常类型", lblDefectValue);
            AddResultRow(grid, 3, "PatchCore Score", lblScoreValue);
            AddResultRow(grid, 4, "分类相似度", lblSimilarityValue);
            AddResultRow(grid, 5, "检测时间", lblTimeValue);
            AddResultRow(grid, 6, "文件名", lblFileValue);

            lblServiceHint.Dock = DockStyle.Fill;
            lblServiceHint.Font = new Font("Microsoft YaHei UI", 8.5F);
            lblServiceHint.ForeColor = Color.FromArgb(137, 148, 160);
            lblServiceHint.Text = "第一阶段：UI 骨架已完成 · Python 推理服务待接入";
            lblServiceHint.TextAlign = ContentAlignment.MiddleCenter;
            grid.Controls.Add(lblServiceHint, 0, 8);
            grid.SetColumnSpan(lblServiceHint, 2);

            card.Controls.Add(grid);
            return card;
        }

        private void BuildStatusBar()
        {
            Panel status = new Panel();
            status.BackColor = Color.White;
            status.Dock = DockStyle.Fill;
            status.Margin = new Padding(0, 12, 0, 0);
            status.Padding = new Padding(14, 0, 14, 0);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.ColumnCount = 3;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            layout.RowCount = 1;
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.Dock = DockStyle.Fill;
            layout.Controls.Add(lblStatus, 0, 0);
            layout.Controls.Add(lblElapsed, 1, 0);
            layout.Controls.Add(lblStatusFile, 2, 0);
            status.Controls.Add(layout);
            rootLayout.Controls.Add(status, 0, 2);
        }

        private static Button CreateToolbarButton(string text, int width, bool primary)
        {
            Button button = new Button();
            button.BackColor = primary ? Color.FromArgb(42, 103, 218) : Color.White;
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(204, 213, 223);
            button.Font = new Font("Microsoft YaHei UI", 9F, primary ? FontStyle.Bold : FontStyle.Regular);
            button.ForeColor = primary ? Color.White : Color.FromArgb(60, 71, 84);
            button.Size = new Size(width, 34);
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
                ? new Font("Consolas", 11F, FontStyle.Bold)
                : new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            label.ForeColor = Color.FromArgb(48, 60, 74);
            label.Text = "--";
            label.TextAlign = ContentAlignment.MiddleRight;
            return label;
        }

        private static Label CreateStatusLabel(ContentAlignment alignment)
        {
            Label label = new Label();
            label.AutoEllipsis = true;
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Microsoft YaHei UI", 8.5F);
            label.ForeColor = Color.FromArgb(91, 103, 116);
            label.TextAlign = alignment;
            return label;
        }

        private static void AddResultRow(TableLayoutPanel grid, int row, string caption, Label value)
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Microsoft YaHei UI", 9.5F);
            label.ForeColor = Color.FromArgb(123, 134, 147);
            label.Text = caption;
            label.TextAlign = ContentAlignment.MiddleLeft;
            grid.Controls.Add(label, 0, row);
            grid.Controls.Add(value, 1, row);
        }
    }
}
