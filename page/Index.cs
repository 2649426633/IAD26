using System;
using System.Collections.Generic;
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

        private readonly AppSettingsService _settingsService;
        private AppSettings _settings;
        private OnnxInferenceService _inferenceService;
        private HikCameraService _cameraService;

        public TabDetection TabDetectionPage
        {
            get { return tabDetection; }
        }

        public Index()
        {
            InitializeComponent();

            _settingsService = new AppSettingsService();
            _settings = _settingsService.Load();
            _inferenceService = new OnnxInferenceService(_settingsService);
            _cameraService = new HikCameraService(
                _settingsService.ResolvePath(_settings.HikCameraSdkAssembly));

            ConfigureNavigationButtons();

            tabDetection.DetectRequested += TabDetection_DetectRequested;
            tabDetection.ProductChanged += TabDetection_ProductChanged;
            tabDetection.CameraConnectionRequested +=
                TabDetection_CameraConnectionRequested;
            tabProductConfig.ConfigurationsChanged +=
                TabProductConfig_ConfigurationsChanged;
            tabSystemSettings.SettingsSaved +=
                TabSystemSettings_SettingsSaved;

            RefreshProductList();
            ShowDetectionPage();
            RefreshInferenceStatus();
            RefreshCameraStatus();
        }

        private void Index_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            RefreshProductList();
            RefreshInferenceStatus();
            RefreshCameraStatus();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _inferenceService?.Dispose();
            _cameraService?.Dispose();
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
            SelectNavigationButton(btnRecords);
            HideAllPages();
            tabRecords.RefreshRecords();
            tabRecords.Visible = true;
            tabRecords.BringToFront();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            SelectNavigationButton(btnProduct);
            HideAllPages();
            tabProductConfig.ReloadConfigurations();
            tabProductConfig.Visible = true;
            tabProductConfig.BringToFront();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SelectNavigationButton(btnSettings);
            HideAllPages();
            tabSystemSettings.ReloadSettings();
            tabSystemSettings.Visible = true;
            tabSystemSettings.BringToFront();
        }

        private void ShowDetectionPage()
        {
            SelectNavigationButton(btnDetection);
            HideAllPages();
            tabDetection.Visible = true;
            tabDetection.BringToFront();
        }

        private void HideAllPages()
        {
            tabDetection.Visible = false;
            tabRecords.Visible = false;
            tabProductConfig.Visible = false;
            tabSystemSettings.Visible = false;
        }

        private void SelectNavigationButton(Button activeButton)
        {
            Button[] buttons =
            {
                btnDetection,
                btnRecords,
                btnProduct,
                btnSettings
            };

            foreach (Button button in buttons)
            {
                bool active = button == activeButton;
                button.BackColor =
                    active ? _activeNavigationColor : _inactiveNavigationColor;
                button.ForeColor =
                    active ? UiTheme.TextPrimary : UiTheme.TextSecondary;
                button.Font = new Font(
                    "Microsoft YaHei UI",
                    9.5F,
                    active ? FontStyle.Bold : FontStyle.Regular);
                button.FlatAppearance.MouseOverBackColor =
                    active
                        ? UiTheme.NavigationActive
                        : UiTheme.NavigationHover;
                button.FlatAppearance.MouseDownBackColor =
                    UiTheme.NavigationPressed;
            }
        }

        private async void TabDetection_CameraConnectionRequested(
            object sender,
            EventArgs e)
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

                string expectedModel = string.IsNullOrWhiteSpace(
                    _settings.CameraExpectedModel)
                    ? "MV-CS200-10GM"
                    : _settings.CameraExpectedModel.Trim();

                IList<HikCameraDevice> devices =
                    await Task.Run(() => _cameraService.RefreshDevices());

                if (devices == null || devices.Count == 0)
                    throw new InvalidOperationException(
                        _cameraService.GetStatusText());

                int selectedIndex = -1;
                for (int i = 0; i < devices.Count; i++)
                {
                    string displayName =
                        devices[i].DisplayName ?? string.Empty;

                    if (displayName.IndexOf(
                        expectedModel,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        selectedIndex = i;
                        break;
                    }
                }

                if (selectedIndex < 0)
                {
                    List<string> names = new List<string>();
                    foreach (HikCameraDevice device in devices)
                        names.Add(device.DisplayName);

                    throw new InvalidOperationException(
                        "未找到指定相机 " + expectedModel +
                        "。当前发现：" +
                        string.Join("，", names.ToArray()));
                }

                await Task.Run(
                    () => _cameraService.Connect(selectedIndex));

                string connectedName =
                    string.IsNullOrWhiteSpace(
                        _cameraService.ConnectedCameraName)
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

        private async void TabDetection_DetectRequested(
            object sender,
            EventArgs e)
        {
            ProductConfig product =
                tabProductConfig.GetProductByName(
                    tabDetection.SelectedProduct);

            if (!_inferenceService.CanInspect(product, out string status))
            {
                tabDetection.DisplayError(
                    status +
                    "。请检查“系统设置”的 Engine Directory " +
                    "以及“产品配置”的产品模型目录。");
                RefreshInferenceStatus();
                return;
            }

            tabDetection.SetBusy(true);
            SetModelStatus("ONNX 推理运行中", false);
            tabDetection.SetModelStatus("ONNX 推理运行中", false);

            try
            {
                DetectionResult result =
                    await _inferenceService.InspectAsync(
                        tabDetection.SelectedImagePath,
                        product);

                tabDetection.DisplayResult(result);
                SetModelStatus("ONNX 模型就绪", true);
                tabDetection.SetModelStatus("ONNX 模型就绪", true);
                tabRecords.RefreshRecords();
            }
            catch (Exception ex)
            {
                tabDetection.DisplayError(ex.Message);
                SetModelStatus("ONNX 推理异常", false);
                tabDetection.SetModelStatus("ONNX 推理异常", false);
            }
            finally
            {
                tabDetection.SetBusy(false);
            }
        }

        private void TabDetection_ProductChanged(
            object sender,
            EventArgs e)
        {
            SetCurrentProduct(tabDetection.SelectedProduct);
            RefreshInferenceStatus();
        }

        private void TabProductConfig_ConfigurationsChanged(
            object sender,
            EventArgs e)
        {
            RefreshProductList();
            RefreshInferenceStatus();
        }

        private void TabSystemSettings_SettingsSaved(
            object sender,
            EventArgs e)
        {
            try
            {
                _cameraService?.Dispose();
                _inferenceService?.Dispose();

                _settings = _settingsService.Load();
                _inferenceService =
                    new OnnxInferenceService(_settingsService);
                _cameraService =
                    new HikCameraService(
                        _settingsService.ResolvePath(
                            _settings.HikCameraSdkAssembly));

                RefreshProductList();
                RefreshInferenceStatus();
                RefreshCameraStatus();
                tabRecords.RefreshRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "重新加载 ONNX / 设备配置失败：" + ex.Message,
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RefreshProductList()
        {
            string selected = tabDetection.SelectedProduct;
            string[] products =
                tabProductConfig.GetEnabledProductNames();

            tabDetection.SetProducts(products, selected);
            SetCurrentProduct(tabDetection.SelectedProduct);
        }

        private void RefreshInferenceStatus()
        {
            ProductConfig product =
                tabProductConfig.GetProductByName(
                    tabDetection.SelectedProduct);

            bool configured =
                _inferenceService.CanInspect(product, out string status);

            SetCurrentProduct(tabDetection.SelectedProduct);
            SetModelStatus(status, configured);
            tabDetection.SetModelStatus(status, configured);
            tabDetection.SetInferenceAvailable(configured);
        }

        private void RefreshCameraStatus()
        {
            if (_cameraService.IsConnected)
            {
                SetCameraStatus(
                    _cameraService.ConnectedCameraName,
                    true);
                return;
            }

            SetCameraStatus(
                _cameraService.IsSdkAvailable
                    ? "未连接"
                    : "MVS SDK 未就绪",
                false);
        }

        public void SetCurrentProduct(string productName)
        {
            lblCurrentProduct.Text =
                "当前产品：" +
                (string.IsNullOrWhiteSpace(productName)
                    ? "--"
                    : productName.Trim());
        }

        public void SetModelStatus(
            string statusText,
            bool ready)
        {
            lblModelStatus.Text =
                (ready ? "● " : "○ ") +
                (string.IsNullOrWhiteSpace(statusText)
                    ? "ONNX 状态未知"
                    : statusText.Trim());

            lblModelStatus.ForeColor =
                ready
                    ? UiTheme.TextPrimary
                    : UiTheme.TextSecondary;
        }

        public void SetCameraStatus(
            string statusText,
            bool connected)
        {
            string text =
                string.IsNullOrWhiteSpace(statusText)
                    ? "未连接"
                    : statusText.Trim();

            lblCameraStatus.Text = "相机：" + text;
            lblCameraStatus.ForeColor =
                connected
                    ? UiTheme.TextPrimary
                    : UiTheme.TextSecondary;

            tabDetection.SetCameraStatus(text, connected);
        }
    }
}
