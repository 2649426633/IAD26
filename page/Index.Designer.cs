using System.Drawing;
using System.Windows.Forms;

namespace _180Detection
{
    partial class Index
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel rootLayout;
        private Panel panelHeader;
        private Panel panelSidebar;
        private Panel panelContent;
        private Label lblBrand;
        private Label lblPageTitle;
        private Label lblCurrentProduct;
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
            panelSidebar = new Panel();
            panelContent = new Panel();
            lblBrand = new Label();
            lblPageTitle = new Label();
            lblCurrentProduct = new Label();
            lblModelStatus = new Label();
            btnDetection = CreateNavigationButton("01   检测工作台", 74, true);
            btnRecords = CreateNavigationButton("02   检测记录", 126, false);
            btnProduct = CreateNavigationButton("03   产品配置", 178, false);
            btnSettings = CreateNavigationButton("04   系统设置", 230, false);
            tabDetection = new TabDetection();
            placeholderPage = new Panel();
            lblPlaceholderTitle = new Label();
            lblPlaceholderDescription = new Label();

            SuspendLayout();

            rootLayout.BackColor = UiTheme.WindowBackground;
            rootLayout.ColumnCount = 2;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowCount = 2;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Margin = Padding.Empty;
            rootLayout.Controls.Add(panelHeader, 0, 0);
            rootLayout.SetColumnSpan(panelHeader, 2);
            rootLayout.Controls.Add(panelSidebar, 0, 1);
            rootLayout.Controls.Add(panelContent, 1, 1);

            BuildHeader();
            BuildSidebar();
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
            panelHeader.BorderStyle = BorderStyle.FixedSingle;
            panelHeader.Dock = DockStyle.Fill;
            panelHeader.Margin = Padding.Empty;

            TableLayoutPanel headerLayout = new TableLayoutPanel();
            headerLayout.ColumnCount = 5;
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            headerLayout.Dock = DockStyle.Fill;
            headerLayout.Margin = Padding.Empty;

            lblBrand.Dock = DockStyle.Fill;
            lblBrand.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblBrand.ForeColor = UiTheme.TextPrimary;
            lblBrand.Padding = new Padding(20, 0, 0, 0);
            lblBrand.Text = "科准 IAD";
            lblBrand.TextAlign = ContentAlignment.MiddleLeft;

            lblPageTitle.Dock = DockStyle.Fill;
            lblPageTitle.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblPageTitle.ForeColor = UiTheme.TextPrimary;
            lblPageTitle.Padding = new Padding(16, 0, 0, 0);
            lblPageTitle.Text = "检测工作台";
            lblPageTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblCurrentProduct.Dock = DockStyle.Fill;
            lblCurrentProduct.Font = new Font("Microsoft YaHei UI", 10F);
            lblCurrentProduct.ForeColor = UiTheme.TextSecondary;
            lblCurrentProduct.Padding = new Padding(0, 0, 12, 0);
            lblCurrentProduct.Text = "当前产品：Phone";
            lblCurrentProduct.TextAlign = ContentAlignment.MiddleRight;

            lblModelStatus.Dock = DockStyle.Fill;
            lblModelStatus.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            lblModelStatus.ForeColor = UiTheme.TextSecondary;
            lblModelStatus.Padding = new Padding(0, 0, 22, 0);
            lblModelStatus.Text = "○ 推理脚本未配置";
            lblModelStatus.TextAlign = ContentAlignment.MiddleRight;

            headerLayout.Controls.Add(lblBrand, 0, 0);
            headerLayout.Controls.Add(lblPageTitle, 1, 0);
            headerLayout.Controls.Add(new Panel(), 2, 0);
            headerLayout.Controls.Add(lblCurrentProduct, 3, 0);
            headerLayout.Controls.Add(lblModelStatus, 4, 0);
            panelHeader.Controls.Add(headerLayout);
        }

        private void BuildSidebar()
        {
            panelSidebar.BackColor = UiTheme.Sidebar;
            panelSidebar.Dock = DockStyle.Fill;
            panelSidebar.Margin = Padding.Empty;

            Label caption = new Label();
            caption.AutoSize = true;
            caption.Font = new Font("Microsoft YaHei UI", 8.5F);
            caption.ForeColor = Color.FromArgb(170, 170, 170);
            caption.Location = new Point(18, 24);
            caption.Text = "功能导航";

            Label version = new Label();
            version.Dock = DockStyle.Bottom;
            version.Font = new Font("Microsoft YaHei UI", 8.5F);
            version.ForeColor = Color.FromArgb(145, 145, 145);
            version.Height = 48;
            version.Padding = new Padding(18, 0, 0, 14);
            version.Text = "Inference UI v0.2";

            btnDetection.Click += new System.EventHandler(btnDetection_Click);
            btnRecords.Click += new System.EventHandler(btnRecords_Click);
            btnProduct.Click += new System.EventHandler(btnProduct_Click);
            btnSettings.Click += new System.EventHandler(btnSettings_Click);

            panelSidebar.Controls.AddRange(new Control[] {
                caption, btnDetection, btnRecords, btnProduct, btnSettings, version
            });
        }

        private void BuildContent()
        {
            panelContent.BackColor = UiTheme.WindowBackground;
            panelContent.Dock = DockStyle.Fill;
            panelContent.Margin = Padding.Empty;
            panelContent.Padding = new Padding(18);

            tabDetection.BackColor = UiTheme.WindowBackground;
            tabDetection.Dock = DockStyle.Fill;
            tabDetection.Margin = Padding.Empty;

            placeholderPage.BackColor = UiTheme.Surface;
            placeholderPage.BorderStyle = BorderStyle.FixedSingle;
            placeholderPage.Dock = DockStyle.Fill;
            placeholderPage.Visible = false;

            TableLayoutPanel placeholderLayout = new TableLayoutPanel();
            placeholderLayout.ColumnCount = 1;
            placeholderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            placeholderLayout.RowCount = 4;
            placeholderLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            placeholderLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            placeholderLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            placeholderLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            placeholderLayout.Dock = DockStyle.Fill;

            lblPlaceholderTitle.Dock = DockStyle.Fill;
            lblPlaceholderTitle.Font = new Font("Microsoft YaHei UI", 22F, FontStyle.Bold);
            lblPlaceholderTitle.ForeColor = UiTheme.TextPrimary;
            lblPlaceholderTitle.Text = "检测记录";
            lblPlaceholderTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblPlaceholderDescription.Dock = DockStyle.Fill;
            lblPlaceholderDescription.Font = new Font("Microsoft YaHei UI", 10F);
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

        private static Button CreateNavigationButton(string text, int top, bool active)
        {
            Button button = new Button();
            button.BackColor = active ? UiTheme.SidebarActive : UiTheme.Sidebar;
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = active
                ? Color.FromArgb(232, 232, 232)
                : UiTheme.SidebarHover;
            button.FlatAppearance.MouseOverBackColor = active
                ? Color.FromArgb(245, 245, 245)
                : UiTheme.SidebarHover;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font(
                "Microsoft YaHei UI",
                10.5F,
                active ? FontStyle.Bold : FontStyle.Regular);
            button.ForeColor = active ? UiTheme.TextPrimary : Color.White;
            button.Location = new Point(0, top);
            button.Padding = new Padding(22, 0, 0, 0);
            button.Size = new Size(210, 52);
            button.Text = text;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.UseVisualStyleBackColor = false;
            return button;
        }
    }
}
