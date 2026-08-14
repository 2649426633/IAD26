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

            rootLayout.BackColor = Color.FromArgb(244, 247, 250);
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
            BackColor = Color.FromArgb(244, 247, 250);
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
            panelHeader.BackColor = Color.White;
            panelHeader.Dock = DockStyle.Fill;
            panelHeader.Margin = Padding.Empty;

            lblBrand.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblBrand.ForeColor = Color.FromArgb(30, 41, 55);
            lblBrand.Location = new Point(20, 17);
            lblBrand.Size = new Size(176, 34);
            lblBrand.Text = "科准 IAD";
            lblBrand.TextAlign = ContentAlignment.MiddleLeft;

            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblPageTitle.ForeColor = Color.FromArgb(54, 65, 78);
            lblPageTitle.Location = new Point(226, 22);
            lblPageTitle.Text = "检测工作台";

            lblCurrentProduct.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCurrentProduct.Font = new Font("Microsoft YaHei UI", 10F);
            lblCurrentProduct.ForeColor = Color.FromArgb(82, 92, 105);
            lblCurrentProduct.Location = new Point(1044, 23);
            lblCurrentProduct.Size = new Size(176, 24);
            lblCurrentProduct.Text = "当前产品：Phone";
            lblCurrentProduct.TextAlign = ContentAlignment.MiddleRight;

            lblModelStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblModelStatus.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            lblModelStatus.ForeColor = Color.FromArgb(229, 152, 52);
            lblModelStatus.Location = new Point(1240, 23);
            lblModelStatus.Size = new Size(174, 24);
            lblModelStatus.Text = "● 模型待接入";
            lblModelStatus.TextAlign = ContentAlignment.MiddleRight;

            panelHeader.Controls.AddRange(new Control[] {
                lblBrand, lblPageTitle, lblCurrentProduct, lblModelStatus
            });
        }

        private void BuildSidebar()
        {
            panelSidebar.BackColor = Color.FromArgb(29, 39, 52);
            panelSidebar.Dock = DockStyle.Fill;
            panelSidebar.Margin = Padding.Empty;

            Label caption = new Label();
            caption.AutoSize = true;
            caption.Font = new Font("Microsoft YaHei UI", 8.5F);
            caption.ForeColor = Color.FromArgb(128, 145, 164);
            caption.Location = new Point(18, 24);
            caption.Text = "功能导航";

            Label version = new Label();
            version.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            version.Font = new Font("Microsoft YaHei UI", 8.5F);
            version.ForeColor = Color.FromArgb(128, 145, 164);
            version.Location = new Point(18, 786);
            version.Size = new Size(174, 24);
            version.Text = "Inference UI v0.1";

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
            panelContent.BackColor = Color.FromArgb(244, 247, 250);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Margin = Padding.Empty;
            panelContent.Padding = new Padding(18);

            tabDetection.BackColor = Color.FromArgb(244, 247, 250);
            tabDetection.Dock = DockStyle.Fill;
            tabDetection.Margin = Padding.Empty;

            placeholderPage.BackColor = Color.White;
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
            lblPlaceholderTitle.ForeColor = Color.FromArgb(45, 57, 70);
            lblPlaceholderTitle.Text = "检测记录";
            lblPlaceholderTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblPlaceholderDescription.Dock = DockStyle.Fill;
            lblPlaceholderDescription.Font = new Font("Microsoft YaHei UI", 10F);
            lblPlaceholderDescription.ForeColor = Color.FromArgb(108, 119, 132);
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
            button.BackColor = active ? Color.FromArgb(42, 103, 218) : Color.FromArgb(29, 39, 52);
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Microsoft YaHei UI", 10.5F, active ? FontStyle.Bold : FontStyle.Regular);
            button.ForeColor = Color.White;
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
