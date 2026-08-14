using System;
using System.Drawing;
using System.Windows.Forms;
using _180Detection.Models;
using _180Detection.Services;

namespace _180Detection
{
    public partial class Index : Form
    {
        private readonly Color _activeNavigationColor = UiTheme.SidebarActive;
        private readonly Color _inactiveNavigationColor = UiTheme.Sidebar;
        private readonly Color _navigationHoverColor = UiTheme.SidebarHover;
        private readonly InferenceService _inferenceService;

        public TabDetection TabDetectionPage
        {
            get { return tabDetection; }
        }

        public Index()
        {
            InitializeComponent();

            _inferenceService = InferenceService.FromConfiguration();
            ConfigureNavigationButtons();

            tabDetection.DetectRequested += TabDetection_DetectRequested;
            tabDetection.ProductChanged += TabDetection_ProductChanged;

            ShowDetectionPage();
            RefreshInferenceStatus();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            RefreshInferenceStatus();
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
                "下一阶段接入 results.json / results.csv / marked 目录，支持历史检测结果查询、筛选和回看。");
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            ShowPlaceholderPage(
                btnProduct,
                "产品配置",
                "用于配置产品名称、PatchCore 模型目录、Defect Bank 与检测参数。此处不提供模型训练功能。");
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowPlaceholderPage(
                btnSettings,
                "系统设置",
                "Python 推理接口已经接入。下一阶段将在此提供 Python 解释器、推理脚本、结果目录、保存策略和日志目录的可视化配置。");
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
                bool active = button == activeButton;
                button.BackColor = active ? _activeNavigationColor : _inactiveNavigationColor;
                button.ForeColor = active ? UiTheme.TextPrimary : Color.White;
                button.Font = new Font(
                    "Microsoft YaHei UI",
                    10.5F,
                    active ? FontStyle.Bold : FontStyle.Regular);
                button.FlatAppearance.MouseOverBackColor = active
                    ? Color.FromArgb(245, 245, 245)
                    : UiTheme.SidebarHover;
                button.FlatAppearance.MouseDownBackColor = active
                    ? Color.FromArgb(232, 232, 232)
                    : UiTheme.SidebarHover;
            }
        }

        private async void TabDetection_DetectRequested(object sender, EventArgs e)
        {
            if (!_inferenceService.IsConfigured)
            {
                string status = _inferenceService.GetConfigurationStatus();
                tabDetection.DisplayError(
                    status + "。请先在 App.config 中配置 PythonExecutable 和 InferenceScript。");
                RefreshInferenceStatus();
                return;
            }

            tabDetection.SetBusy(true);
            SetModelStatus("推理运行中", false);
            tabDetection.SetModelStatus("推理运行中", false);

            try
            {
                DetectionResult result = await _inferenceService.InspectAsync(
                    tabDetection.SelectedImagePath,
                    tabDetection.SelectedProduct);

                tabDetection.DisplayResult(result);
                SetModelStatus("模型就绪", true);
                tabDetection.SetModelStatus("模型就绪", true);
            }
            catch (Exception ex)
            {
                tabDetection.DisplayError(ex.Message);
                SetModelStatus("推理异常", false);
                tabDetection.SetModelStatus("推理异常", false);
            }
            finally
            {
                tabDetection.SetBusy(false);
            }
        }

        private void TabDetection_ProductChanged(object sender, EventArgs e)
        {
            SetCurrentProduct(tabDetection.SelectedProduct);
        }

        private void RefreshInferenceStatus()
        {
            string status = _inferenceService.GetConfigurationStatus();
            bool configured = _inferenceService.IsConfigured;

            SetCurrentProduct(tabDetection.SelectedProduct);
            SetModelStatus(status, configured);
            tabDetection.SetModelStatus(status, configured);
            tabDetection.SetInferenceAvailable(configured);
        }

        public void SetCurrentProduct(string productName)
        {
            lblCurrentProduct.Text = "当前产品：" +
                (string.IsNullOrWhiteSpace(productName) ? "--" : productName.Trim());
        }

        public void SetModelStatus(string statusText, bool ready)
        {
            lblModelStatus.Text = (ready ? "● " : "○ ") +
                (string.IsNullOrWhiteSpace(statusText) ? "模型状态未知" : statusText.Trim());
            lblModelStatus.ForeColor = ready
                ? UiTheme.TextPrimary
                : UiTheme.TextSecondary;
        }
    }
}
