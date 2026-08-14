using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using _180Detection.Models;

namespace _180Detection
{
    public partial class TabDetection : UserControl
    {
        private string _selectedImagePath;
        private bool _inferenceAvailable;
        private bool _busy;
        private bool _cameraBusy;
        private bool _cameraConnected;

        public event EventHandler DetectRequested;
        public event EventHandler ProductChanged;
        public event EventHandler CameraConnectionRequested;

        public string SelectedImagePath { get { return _selectedImagePath; } }
        public string SelectedProduct
        {
            get
            {
                return cmbProduct.SelectedItem == null
                    ? string.Empty
                    : cmbProduct.SelectedItem.ToString();
            }
        }

        public TabDetection()
        {
            InitializeComponent();
            lblViewerHint.BringToFront();
            cmbProduct.Items.Add("Phone");
            cmbProduct.SelectedIndex = 0;
            Disposed += TabDetection_Disposed;
            SetWaitingState();
            SetCameraStatus("未连接", false);
        }

        public void SetProducts(string[] products, string selectedProduct)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string[], string>(SetProducts), products, selectedProduct);
                return;
            }

            cmbProduct.BeginUpdate();
            try
            {
                cmbProduct.Items.Clear();
                if (products != null)
                {
                    foreach (string product in products)
                    {
                        if (!string.IsNullOrWhiteSpace(product))
                            cmbProduct.Items.Add(product.Trim());
                    }
                }

                int selectedIndex = -1;
                for (int i = 0; i < cmbProduct.Items.Count; i++)
                {
                    if (string.Equals(cmbProduct.Items[i].ToString(), selectedProduct, StringComparison.OrdinalIgnoreCase))
                    {
                        selectedIndex = i;
                        break;
                    }
                }

                if (selectedIndex >= 0)
                    cmbProduct.SelectedIndex = selectedIndex;
                else if (cmbProduct.Items.Count > 0)
                    cmbProduct.SelectedIndex = 0;
            }
            finally
            {
                cmbProduct.EndUpdate();
            }
        }

        public void SetInferenceAvailable(bool available)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(SetInferenceAvailable), available);
                return;
            }

            _inferenceAvailable = available;
            UpdateDetectButtonState();
            lblServiceHint.Text = available
                ? "ONNX Runtime 已配置 · 选择图片后可直接检测"
                : "ONNX Runtime 未就绪 · 请检查 Engine / 产品模型";
        }

        public void SetModelStatus(string text, bool ready)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, bool>(SetModelStatus), text, ready);
                return;
            }

            lblModelState.Text = (ready ? "● " : "○ ") +
                (string.IsNullOrWhiteSpace(text) ? "ONNX 状态未知" : text.Trim());
            lblModelState.ForeColor = ready ? UiTheme.TextPrimary : UiTheme.TextSecondary;
        }

        public void SetCameraStatus(string text, bool connected)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, bool>(SetCameraStatus), text, connected);
                return;
            }

            _cameraConnected = connected;
            string status = string.IsNullOrWhiteSpace(text) ? "未连接" : text.Trim();
            lblCameraState.Text = (connected ? "● " : "○ ") + status;
            lblCameraState.ForeColor = connected ? UiTheme.TextPrimary : UiTheme.TextMuted;
            btnCameraConnect.Text = connected ? "断开相机" : "连接相机";
            UpdateCameraButtonState();
        }

        public void SetCameraBusy(bool busy)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(SetCameraBusy), busy);
                return;
            }

            _cameraBusy = busy;
            UpdateCameraButtonState();
            if (busy)
            {
                btnCameraConnect.Text = "处理中...";
                lblCameraState.Text = "○ 正在搜索/连接相机";
            }
        }

        public void ShowCameraError(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ShowCameraError), message);
                return;
            }

            lblCameraState.Text = "○ 相机连接失败";
            lblCameraState.ForeColor = UiTheme.TextSecondary;
            MessageBox.Show(
                string.IsNullOrWhiteSpace(message) ? "相机连接失败。" : message,
                "海康相机",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public void SetBusy(bool busy)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(SetBusy), busy);
                return;
            }

            _busy = busy;
            cmbProduct.Enabled = !busy;
            btnChooseImage.Enabled = !busy;
            btnOpenDirectory.Enabled = !busy;
            btnDetect.Text = busy ? "检测中..." : "开始检测";
            UpdateDetectButtonState();
            UpdateCameraButtonState();

            if (busy)
            {
                lblStatus.Text = "状态：ONNX Runtime 推理中...";
                lblElapsed.Text = "耗时：-- ms";
                lblResultState.Text = "检测中";
                lblResultState.ForeColor = UiTheme.TextPrimary;
                lblServiceHint.Text = "ONNX Session 常驻 · 正在执行当前图像";
            }
        }

        public void DisplayResult(DetectionResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (InvokeRequired)
            {
                BeginInvoke(new Action<DetectionResult>(DisplayResult), result);
                return;
            }

            string displayImagePath = !string.IsNullOrWhiteSpace(result.MarkedImagePath)
                ? result.MarkedImagePath
                : result.ImagePath;
            if (!string.IsNullOrWhiteSpace(displayImagePath) && File.Exists(displayImagePath))
                LoadImageIntoViewer(displayImagePath);

            string decision = string.IsNullOrWhiteSpace(result.Decision)
                ? (result.IsNg ? "NG" : "PASS")
                : result.Decision.Trim().ToUpperInvariant();

            lblResultState.Text = decision == "UNCALIBRATED"
                ? "未标定"
                : decision;
            lblResultState.ForeColor = UiTheme.TextPrimary;

            lblDefectValue.Text = decision == "PASS"
                ? "Normal"
                : string.IsNullOrWhiteSpace(result.DefectClass) ? "--" : result.DefectClass;
            lblScoreValue.Text = result.AnomalyScore.ToString("0.000000");
            lblSimilarityValue.Text = string.IsNullOrWhiteSpace(result.DefectClass)
                ? "--"
                : FormatSimilarity(result.Similarity);
            lblTimeValue.Text = (result.RecordTime == default ? DateTime.Now : result.RecordTime)
                .ToString("yyyy-MM-dd HH:mm:ss");

            string sourcePath = string.IsNullOrWhiteSpace(result.ImagePath)
                ? _selectedImagePath
                : result.ImagePath;
            lblFileValue.Text = string.IsNullOrWhiteSpace(sourcePath)
                ? "--"
                : Path.GetFileName(sourcePath);

            lblStatus.Text = decision == "UNCALIBRATED"
                ? "状态：检测完成 · PASS/NG 阈值未标定"
                : "状态：检测完成";
            lblElapsed.Text = "耗时：" + Math.Max(0L, result.ElapsedMilliseconds) + " ms";
            lblStatusFile.Text = string.IsNullOrWhiteSpace(sourcePath)
                ? "文件：--"
                : "文件：" + Path.GetFileName(sourcePath);
            lblServiceHint.Text = "ONNX Runtime 推理完成 · " +
                (string.IsNullOrWhiteSpace(result.FinalResult) ? decision : result.FinalResult);
            lblViewerHint.Visible = pictureResult.Image == null;
        }

        public void DisplayError(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(DisplayError), message);
                return;
            }

            lblResultState.Text = "ERROR";
            lblResultState.ForeColor = UiTheme.TextPrimary;
            lblStatus.Text = "状态：检测失败";
            lblElapsed.Text = "耗时：-- ms";
            lblServiceHint.Text = string.IsNullOrWhiteSpace(message)
                ? "ONNX 推理发生未知错误"
                : message.Trim();
        }

        public void ClearResult()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ClearResult));
                return;
            }

            lblResultState.Text = "--";
            lblResultState.ForeColor = UiTheme.TextSecondary;
            lblDefectValue.Text = "--";
            lblScoreValue.Text = "--";
            lblSimilarityValue.Text = "--";
            lblTimeValue.Text = "--";
            lblFileValue.Text = string.IsNullOrWhiteSpace(_selectedImagePath)
                ? "--"
                : Path.GetFileName(_selectedImagePath);
            lblElapsed.Text = "耗时：-- ms";
        }

        private void btnCameraConnect_Click(object sender, EventArgs e)
        {
            CameraConnectionRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "选择待检测图片";
            dialog.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|所有文件|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                _selectedImagePath = dialog.FileName;
                LoadImageIntoViewer(_selectedImagePath);
                ClearResult();
                lblResultState.Text = "待检测";
                lblResultState.ForeColor = UiTheme.TextPrimary;
                lblStatus.Text = _inferenceAvailable
                    ? "状态：图片已选择，等待检测"
                    : "状态：图片已选择，但 ONNX 模型尚未就绪";
                lblStatusFile.Text = "文件：" + Path.GetFileName(_selectedImagePath);
                UpdateDetectButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法加载图片：" + ex.Message, "图片加载失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDetect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedImagePath) || !File.Exists(_selectedImagePath))
            {
                MessageBox.Show("请先选择一张待检测图片。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!_inferenceAvailable)
            {
                DisplayError("ONNX 推理服务未就绪。请检查 Engine Directory 和产品模型目录。");
                return;
            }
            DetectRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnOpenDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_selectedImagePath) && File.Exists(_selectedImagePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = "/select,\"" + _selectedImagePath + "\"",
                        UseShellExecute = true
                    });
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = Application.StartupPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法打开目录：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFitImage_Click(object sender, EventArgs e)
        {
            pictureResult.FitToWindow();
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductChanged?.Invoke(this, EventArgs.Empty);
        }

        private void LoadImageIntoViewer(string imagePath)
        {
            Image loadedImage;
            using (Image source = Image.FromFile(imagePath))
            {
                loadedImage = new Bitmap(source);
            }

            Image previous = pictureResult.Image;
            pictureResult.Image = loadedImage;
            previous?.Dispose();
            pictureResult.FitToWindow();
            lblViewerHint.Visible = false;
        }

        private void SetWaitingState()
        {
            lblResultState.Text = "--";
            lblResultState.ForeColor = UiTheme.TextSecondary;
            lblDefectValue.Text = "--";
            lblScoreValue.Text = "--";
            lblSimilarityValue.Text = "--";
            lblTimeValue.Text = "--";
            lblFileValue.Text = "--";
            lblStatus.Text = "状态：等待图片";
            lblElapsed.Text = "耗时：-- ms";
            lblStatusFile.Text = "文件：--";
            lblServiceHint.Text = "ONNX Runtime · 等待 Engine / 产品模型";
            lblViewerHint.Visible = true;
            UpdateDetectButtonState();
            UpdateCameraButtonState();
        }

        private void UpdateDetectButtonState()
        {
            bool hasImage = !string.IsNullOrWhiteSpace(_selectedImagePath) && File.Exists(_selectedImagePath);
            btnDetect.Enabled = !_busy && _inferenceAvailable && hasImage;
            btnDetect.BackColor = btnDetect.Enabled ? UiTheme.PrimaryButton : UiTheme.Disabled;
            btnDetect.ForeColor = Color.White;
        }

        private void UpdateCameraButtonState()
        {
            btnCameraConnect.Enabled = !_busy && !_cameraBusy;
            if (!_cameraBusy)
                btnCameraConnect.Text = _cameraConnected ? "断开相机" : "连接相机";
        }

        private static string FormatSimilarity(double similarity)
        {
            double percent = similarity;
            if (percent >= 0D && percent <= 1D)
                percent *= 100D;
            return percent.ToString("0.00") + "%";
        }

        private void TabDetection_Disposed(object sender, EventArgs e)
        {
            Image image = pictureResult.Image;
            pictureResult.Image = null;
            image?.Dispose();
        }
    }
}
