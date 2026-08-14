using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using _180Detection.Camera;
using _180Detection.Models;
using _180Detection.Services;

namespace _180Detection
{
    public partial class Index : Form
    {
        private readonly Color _activeNavigationColor = UiTheme.NavigationActive;
        private readonly Color _inactiveNavigationColor = UiTheme.Surface;
        private readonly Color _navigationHoverColor = UiTheme.NavigationHover;
        private readonly InferenceService _inferenceService;
        private readonly HikCameraService _cameraService;

        public TabDetection TabDetectionPage
        {
            get { return tabDetection; }
        }

        public Index()
        {
            InitializeComponent();

            _inferenceService = InferenceService.FromConfiguration();
            _cameraService = new HikCameraService();

            ConfigureNavigationButtons();

            tabDetection.DetectRequested += TabDetection_DetectRequested;
            tabDetection.ProductChanged += TabDetection_ProductChanged;
            tabDetection.CameraConnectionRequested += TabDetection_CameraConnectionRequested;

            ShowDetectionPage();
            RefreshInferenceStatus();
            RefreshCameraStatus();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            RefreshInferenceStatus();
            RefreshCameraStatus();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _cameraService.Dispose();
            base.OnFormClosed(e);
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
                "用于配置 Python 推理、海康 MVS SDK、相机型号、结果目录、保存策略和日志目录。");
        }

        private void ShowDetectionPage()
        {
            SelectNavigationButton(btnDetection);
            placeholderPage.Visible = false;
            tabDetection.Visible = true;
            tabDetection.BringToFront();
        }

        private void ShowPlaceholderPage(Button selectedButton, string title, string description)
        {
            SelectNavigationButton(selectedButton);
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
                button.ForeColor = active ? UiTheme.TextPrimary : UiTheme.TextSecondary;
                button.Font = new Font(
                    "Microsoft YaHei UI",
                    9.5F,
                    active ? FontStyle.Bold : FontStyle.Regular);
                button.FlatAppearance.MouseOverBackColor = active
                    ? UiTheme.NavigationActive
                    : UiTheme.NavigationHover;
                button.FlatAppearance.MouseDownBackColor = UiTheme.NavigationPressed;
            }
        }

        private async void TabDetection_CameraConnectionRequested(object sender, EventArgs e)
        {
            tabDetection.SetCameraBusy(true);

            try
            {
                if (_cameraService.IsConnected)
                {
                    await Task.Run(() => _cameraService.Disconnect());
                    SetCameraStatus("未连接", false);
                    return;
                }

                string expectedModel = ConfigurationManager.AppSettings["CameraExpectedModel"];
                if (string.IsNullOrWhiteSpace(expectedModel))
                    expectedModel = "MV-CS200-10GM";

                IList<HikCameraDevice> devices =
                    await Task.Run(() => _cameraService.RefreshDevices());

                if (devices == null || devices.Count == 0)
                    throw new InvalidOperationException(_cameraService.GetStatusText());

                int selectedIndex = -1;
                for (int i = 0; i < devices.Count; i++)
                {
                    string displayName = devices[i].DisplayName ?? string.Empty;
                    if (displayName.IndexOf(expectedModel, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        selectedIndex = i;
                        break;
                    }
                }

                if (selectedIndex < 0)
                {
                    var names = new List<string>();
                    foreach (HikCameraDevice device in devices)
                        names.Add(device.DisplayName);

                    throw new InvalidOperationException(
                        "未找到指定相机 " + expectedModel +
                        "。当前发现：" + string.Join("，", names.ToArray()));
                }

                await Task.Run(() => _cameraService.Connect(selectedIndex));

                string connectedName = string.IsNullOrWhiteSpace(_cameraService.ConnectedCameraName)
                    ? expectedModel
                    : _cameraService.ConnectedCameraName;

                SetCameraStatus(connectedName, true);
            }
            catch (Exception ex)
            {
                SetCameraStatus("连接失败", false);
                tabDetection.ShowCameraError(ex.Message);
            }
            finally
            {
                tabDetection.SetCameraBusy(false);
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

        private void RefreshCameraStatus()
        {
            if (_cameraService.IsConnected)
            {
                SetCameraStatus(_cameraService.ConnectedCameraName, true);
                return;
            }

            SetCameraStatus(
                _cameraService.IsSdkAvailable ? "未连接" : "MVS SDK 未就绪",
                false);
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

        public void SetCameraStatus(string statusText, bool connected)
        {
            string text = string.IsNullOrWhiteSpace(statusText)
                ? "未连接"
                : statusText.Trim();

            lblCameraStatus.Text = "相机：" + text;
            lblCameraStatus.ForeColor = connected
                ? UiTheme.TextPrimary
                : UiTheme.TextSecondary;

            tabDetection.SetCameraStatus(text, connected);
        }
    }
}
