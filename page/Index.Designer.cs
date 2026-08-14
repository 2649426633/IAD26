using System.Drawing;
using System.Windows.Forms;

namespace _180Detection
{
    partial class Index
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel rootLayout;
        private Panel panelHeader;
        private Panel panelContent;
        private Label lblBrand;
        private Label lblCurrentProduct;
        private Label lblCameraStatus;
        private Label lblModelStatus;
        private Button btnDetection;
        private Button btnRecords;
        private Button btnProduct;
        private Button btnSettings;
        private TabDetection tabDetection;
        private Panel placeholderPage;
        private Label lblPlaceholderTitle;
        private Label lblPlaceholderDescription;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(Index));

            rootLayout = new TableLayoutPanel();
            panelHeader = new Panel();
            panelContent = new Panel();
            lblBrand = new Label();
            lblCurrentProduct = new Label();
            lblCameraStatus = new Label();
            lblModelStatus = new Label();
            btnDetection = CreateTopNavigationButton("检测工作台", true);
            btnRecords = CreateTopNavigationButton("检测记录", false);
            btnProduct = CreateTopNavigationButton("产品配置", false);
            btnSettings = CreateTopNavigationButton("系统设置", false);
            tabDetection = new TabDetection();
            placeholderPage = new Panel();
            lblPlaceholderTitle = new Label();
            lblPlaceholderDescription = new Label();

            SuspendLayout();

            rootLayout.BackColor = UiTheme.WindowBackground;
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowCount = 2;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Margin = Padding.Empty;
            rootLayout.Controls.Add(panelHeader, 0, 0);
            rootLayout.Controls.Add(panelContent, 0, 1);

            BuildHeader();
            BuildContent();

            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = UiTheme.WindowBackground;
            ClientSize = new Size(1440, 900);
            Controls.Add(rootLayout);
            Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1180, 720);
            Name = "Index";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "科准 IAD 智能异常检测系统";
            WindowState = FormWindowState.Maximized;
            Load += new System.EventHandler(Index_Load);

            ResumeLayout(false);
        }

        private void BuildHeader()
        {
            panelHeader.BackColor = UiTheme.Surface;
            panelHeader.Dock = DockStyle.Fill;
            panelHeader.Margin = Padding.Empty;

            TableLayoutPanel headerLayout = new TableLayoutPanel();
            headerLayout.BackColor = UiTheme.Surface;
            headerLayout.ColumnCount = 5;
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            headerLayout.RowCount = 2;
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 1F));
            headerLayout.Dock = DockStyle.Fill;
            headerLayout.Margin = Padding.Empty;

            lblBrand.Dock = DockStyle.Fill;
            lblBrand.Font = new Font("Microsoft YaHei UI", 12.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblBrand.ForeColor = UiTheme.TextPrimary;
            lblBrand.Padding = new Padding(18, 0, 0, 0);
            lblBrand.Text = "科准 IAD";
            lblBrand.TextAlign = ContentAlignment.MiddleLeft;

            FlowLayoutPanel navigation = new FlowLayoutPanel();
            navigation.BackColor = UiTheme.Surface;
            navigation.Dock = DockStyle.Fill;
            navigation.Margin = Padding.Empty;
            navigation.Padding = Padding.Empty;
            navigation.WrapContents = false;

            btnDetection.Click += new System.EventHandler(btnDetection_Click);
            btnRecords.Click += new System.EventHandler(btnRecords_Click);
            btnProduct.Click += new System.EventHandler(btnProduct_Click);
            btnSettings.Click += new System.EventHandler(btnSettings_Click);

            navigation.Controls.AddRange(new Control[] {
                btnDetection, btnRecords, btnProduct, btnSettings
            });

            ConfigureHeaderStatusLabel(lblCurrentProduct, "当前产品：Phone");
            ConfigureHeaderStatusLabel(lblCameraStatus, "相机：未连接");
            ConfigureHeaderStatusLabel(lblModelStatus, "○ 推理脚本未配置");
            lblModelStatus.Padding = new Padding(0, 0, 18, 0);

            Panel separator = new Panel();
            separator.BackColor = UiTheme.Border;
            separator.Dock = DockStyle.Fill;
            separator.Margin = Padding.Empty;

            headerLayout.Controls.Add(lblBrand, 0, 0);
            headerLayout.Controls.Add(navigation, 1, 0);
            headerLayout.Controls.Add(lblCurrentProduct, 2, 0);
            headerLayout.Controls.Add(lblCameraStatus, 3, 0);
            headerLayout.Controls.Add(lblModelStatus, 4, 0);
            headerLayout.Controls.Add(separator, 0, 1);
            headerLayout.SetColumnSpan(separator, 5);
            panelHeader.Controls.Add(headerLayout);
        }

        private static void ConfigureHeaderStatusLabel(Label label, string text)
        {
            label.Dock = DockStyle.Fill;
            label.Font = new Font("Microsoft YaHei UI", 8.5F);
            label.ForeColor = UiTheme.TextSecondary;
            label.Padding = new Padding(0, 0, 12, 0);
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleRight;
        }

        private void BuildContent()
        {
            panelContent.BackColor = UiTheme.WindowBackground;
            panelContent.Dock = DockStyle.Fill;
            panelContent.Margin = Padding.Empty;
            panelContent.Padding = new Padding(12);

            tabDetection.BackColor = UiTheme.WindowBackground;
            tabDetection.Dock = DockStyle.Fill;
            tabDetection.Margin = Padding.Empty;

            placeholderPage.BackColor = UiTheme.Surface;
            placeholderPage.BorderStyle = BorderStyle.FixedSingle;
            placeholderPage.Dock = DockStyle.Fill;
            placeholderPage.Visible = false;

            TableLayoutPanel placeholderLayout = new TableLayoutPanel();
            placeholderLayout.BackColor = UiTheme.Surface;
            placeholderLayout.ColumnCount = 1;
            placeholderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            placeholderLayout.RowCount = 4;
            placeholderLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 43F));
            placeholderLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            placeholderLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            placeholderLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 57F));
            placeholderLayout.Dock = DockStyle.Fill;

            lblPlaceholderTitle.Dock = DockStyle.Fill;
            lblPlaceholderTitle.Font = new Font("Microsoft YaHei UI", 17F, FontStyle.Bold);
            lblPlaceholderTitle.ForeColor = UiTheme.TextPrimary;
            lblPlaceholderTitle.Text = "检测记录";
            lblPlaceholderTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblPlaceholderDescription.Dock = DockStyle.Fill;
            lblPlaceholderDescription.Font = new Font("Microsoft YaHei UI", 9F);
            lblPlaceholderDescription.ForeColor = UiTheme.TextSecondary;
            lblPlaceholderDescription.Padding = new Padding(120, 0, 120, 0);
            lblPlaceholderDescription.Text = "功能将在下一阶段实现。";
            lblPlaceholderDescription.TextAlign = ContentAlignment.TopCenter;

            placeholderLayout.Controls.Add(new Panel(), 0, 0);
            placeholderLayout.Controls.Add(lblPlaceholderTitle, 0, 1);
            placeholderLayout.Controls.Add(lblPlaceholderDescription, 0, 2);
            placeholderLayout.Controls.Add(new Panel(), 0, 3);
            placeholderPage.Controls.Add(placeholderLayout);

            panelContent.Controls.Add(tabDetection);
            panelContent.Controls.Add(placeholderPage);
        }

        private static Button CreateTopNavigationButton(string text, bool active)
        {
            Button button = new Button();
            button.AutoSize = false;
            button.BackColor = active ? UiTheme.NavigationActive : UiTheme.Surface;
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = UiTheme.NavigationHover;
            button.FlatAppearance.MouseDownBackColor = UiTheme.NavigationPressed;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font(
                "Microsoft YaHei UI",
                9.5F,
                active ? FontStyle.Bold : FontStyle.Regular);
            button.ForeColor = active ? UiTheme.TextPrimary : UiTheme.TextSecondary;
            button.Margin = Padding.Empty;
            button.Size = new Size(112, 61);
            button.Text = text;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.UseVisualStyleBackColor = false;
            return button;
        }
    }
}
