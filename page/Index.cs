using System;
using System.Drawing;
using System.Windows.Forms;

namespace _180Detection
{
    public partial class Index : Form
    {
        private readonly Color _activeNavigationColor = Color.FromArgb(42, 103, 218);
        private readonly Color _inactiveNavigationColor = Color.FromArgb(29, 39, 52);
        private readonly Color _navigationHoverColor = Color.FromArgb(38, 50, 66);

        public TabDetection TabDetectionPage
        {
            get { return tabDetection; }
        }

        public Index()
        {
            InitializeComponent();
            ConfigureNavigationButtons();
            ShowDetectionPage();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        private void ConfigureNavigationButtons()
        {
            ConfigureNavigationButton(btnDetection);
            ConfigureNavigationButton(btnRecords);
            ConfigureNavigationButton(btnProduct);
            ConfigureNavigationButton(btnSettings);
        }

        private void ConfigureNavigationButton(Button button)
        {
            button.MouseEnter += delegate
            {
                if (button.BackColor != _activeNavigationColor)
                    button.BackColor = _navigationHoverColor;
            };

            button.MouseLeave += delegate
            {
                if (button.BackColor != _activeNavigationColor)
                    button.BackColor = _inactiveNavigationColor;
            };
        }

        private void btnDetection_Click(object sender, EventArgs e)
        {
            ShowDetectionPage();
        }

        private void btnRecords_Click(object sender, EventArgs e)
        {
            ShowPlaceholderPage(
                btnRecords,
                "检测记录",
                "下一阶段将接入 results.json / results.csv / marked 目录，支持历史检测结果查询与回看。");
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            ShowPlaceholderPage(
                btnProduct,
                "产品配置",
                "下一阶段用于配置产品名称、PatchCore 模型目录、Defect Bank 与检测参数。此处不提供模型训练功能。");
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowPlaceholderPage(
                btnSettings,
                "系统设置",
                "下一阶段用于配置 Python 解释器、推理程序目录、结果目录、保存策略与日志目录。");
        }

        private void ShowDetectionPage()
        {
            SelectNavigationButton(btnDetection);
            lblPageTitle.Text = "检测工作台";
            placeholderPage.Visible = false;
            tabDetection.Visible = true;
            tabDetection.BringToFront();
        }

        private void ShowPlaceholderPage(Button selectedButton, string title, string description)
        {
            SelectNavigationButton(selectedButton);
            lblPageTitle.Text = title;
            lblPlaceholderTitle.Text = title;
            lblPlaceholderDescription.Text = description;
            tabDetection.Visible = false;
            placeholderPage.Visible = true;
            placeholderPage.BringToFront();
        }

        private void SelectNavigationButton(Button activeButton)
        {
            Button[] buttons = { btnDetection, btnRecords, btnProduct, btnSettings };
            foreach (Button button in buttons)
            {
                button.BackColor = button == activeButton
                    ? _activeNavigationColor
                    : _inactiveNavigationColor;
                button.ForeColor = Color.White;
            }
        }

        public void SetCurrentProduct(string productName)
        {
            lblCurrentProduct.Text = "当前产品：" +
                (string.IsNullOrWhiteSpace(productName) ? "--" : productName.Trim());
        }

        public void SetModelStatus(string statusText, bool ready)
        {
            lblModelStatus.Text = "● " +
                (string.IsNullOrWhiteSpace(statusText) ? "模型状态未知" : statusText.Trim());
            lblModelStatus.ForeColor = ready
                ? Color.FromArgb(46, 173, 107)
                : Color.FromArgb(229, 152, 52);
        }
    }
}
