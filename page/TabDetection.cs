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

        public event EventHandler DetectRequested;
        public event EventHandler ProductChanged;

        public string SelectedImagePath
        {
            get { return _selectedImagePath; }
        }

        public string SelectedProduct
        {
            get { return cmbProduct.SelectedItem == null ? string.Empty : cmbProduct.SelectedItem.ToString(); }
        }

        public TabDetection()
        {
            InitializeComponent();
            lblViewerHint.BringToFront();

            cmbProduct.Items.Add("Phone");
            cmbProduct.SelectedIndex = 0;
            btnDetect.Enabled = false;
            Disposed += TabDetection_Disposed;
            SetWaitingState();
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
                    if (string.Equals(cmbProduct.Items[i].ToString(), selectedProduct,
                        StringComparison.OrdinalIgnoreCase))
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

        public void SetModelStatus(string text, bool ready)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, bool>(SetModelStatus), text, ready);
                return;
            }

            lblModelState.Text = "● " + (string.IsNullOrWhiteSpace(text) ? "模型状态未知" : text.Trim());
            lblModelState.ForeColor = ready
                ? Color.FromArgb(46, 173, 107)
                : Color.FromArgb(229, 152, 52);
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

            lblResultState.Text = result.IsNg ? "NG" : "PASS";
            lblResultState.ForeColor = result.IsNg
                ? Color.FromArgb(220, 68, 68)
                : Color.FromArgb(31, 157, 85);

            lblDefectValue.Text = result.IsNg
                ? (string.IsNullOrWhiteSpace(result.DefectClass) ? "Unknown" : result.DefectClass)
                : "Normal";
            lblScoreValue.Text = result.AnomalyScore.ToString("0.0000");
            lblSimilarityValue.Text = FormatSimilarity(result.Similarity);
            lblTimeValue.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string sourcePath = string.IsNullOrWhiteSpace(result.ImagePath)
                ? _selectedImagePath
                : result.ImagePath;
            lblFileValue.Text = string.IsNullOrWhiteSpace(sourcePath)
                ? "--"
                : Path.GetFileName(sourcePath);

            lblStatus.Text = "状态：检测完成";
            lblElapsed.Text = "耗时：" + Math.Max(0L, result.ElapsedMilliseconds) + " ms";
            lblStatusFile.Text = string.IsNullOrWhiteSpace(sourcePath)
                ? "文件：--"
                : "文件：" + Path.GetFileName(sourcePath);
            lblViewerHint.Visible = pictureResult.Image == null;
        }

        public void ClearResult()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ClearResult));
                return;
            }

            lblResultState.Text = "--";
            lblResultState.ForeColor = Color.FromArgb(105, 116, 130);
            lblDefectValue.Text = "--";
            lblScoreValue.Text = "--";
            lblSimilarityValue.Text = "--";
            lblTimeValue.Text = "--";
            lblFileValue.Text = string.IsNullOrWhiteSpace(_selectedImagePath)
                ? "--"
                : Path.GetFileName(_selectedImagePath);
            lblElapsed.Text = "耗时：-- ms";
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
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
                    lblResultState.ForeColor = Color.FromArgb(42, 103, 218);
                    lblStatus.Text = "状态：图片已选择，等待检测";
                    lblStatusFile.Text = "文件：" + Path.GetFileName(_selectedImagePath);
                    btnDetect.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "无法加载图片：" + ex.Message,
                        "图片加载失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnDetect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedImagePath) || !File.Exists(_selectedImagePath))
            {
                MessageBox.Show("请先选择一张待检测图片。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lblStatus.Text = "状态：正在请求检测...";
            lblElapsed.Text = "耗时：-- ms";

            EventHandler handler = DetectRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
                return;
            }

            lblStatus.Text = "状态：推理服务尚未接入，当前已完成 UI 骨架";
            lblResultState.Text = "等待服务";
            lblResultState.ForeColor = Color.FromArgb(229, 152, 52);
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
                MessageBox.Show("无法打开目录：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFitImage_Click(object sender, EventArgs e)
        {
            pictureResult.FitToWindow();
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            EventHandler handler = ProductChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
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
            if (previous != null)
                previous.Dispose();

            pictureResult.FitToWindow();
            lblViewerHint.Visible = false;
        }

        private void SetWaitingState()
        {
            lblResultState.Text = "--";
            lblResultState.ForeColor = Color.FromArgb(105, 116, 130);
            lblDefectValue.Text = "--";
            lblScoreValue.Text = "--";
            lblSimilarityValue.Text = "--";
            lblTimeValue.Text = "--";
            lblFileValue.Text = "--";
            lblStatus.Text = "状态：等待图片";
            lblElapsed.Text = "耗时：-- ms";
            lblStatusFile.Text = "文件：--";
            lblViewerHint.Visible = true;
        }

        private static string FormatSimilarity(double similarity)
        {
            double percent = similarity;
            if (percent >= 0D && percent <= 1D)
                percent *= 100D;
            return Math.Max(0D, percent).ToString("0.00") + "%";
        }

        private void TabDetection_Disposed(object sender, EventArgs e)
        {
            Image image = pictureResult.Image;
            pictureResult.Image = null;
            if (image != null)
                image.Dispose();
        }
    }
}
