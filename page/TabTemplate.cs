using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace _180Detection
{
    public partial class TabTemplate : UserControl
    {
        private int _lastPixelColumn = -1;
        private int _lastPixelRow = -1;
        private string _currentImageName = "image";

        public TabTemplate()
        {
            InitializeComponent();

            if (cmbType.Items.Count > 0) cmbType.SelectedIndex = 0;
            if (cmbTransition.Items.Count > 0) cmbTransition.SelectedIndex = 0;
            if (cmbSelect.Items.Count > 0) cmbSelect.SelectedIndex = 0;

            this.TabStop = true;
            this.DoubleBuffered = true;
            this.Disposed += TabTemplate_Disposed;
            pictureBoxTemplate.MouseMove += PictureBoxTemplate_MouseMove;
            pictureBoxTemplate.MouseLeave += PictureBoxTemplate_MouseLeave;
            btnFitWindow.Click += BtnFitWindow_Click;
            btnFlipImage.Click += BtnFlipImage_Click;
            btnSaveImage.Click += BtnSaveImage_Click;
        }

        private void pictureBoxTemplate_Click(object sender, System.EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, System.EventArgs e)
        {

        }

        private void panelSettings_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTitle_Click_1(object sender, System.EventArgs e)
        {

        }
        //选择图片
        private void btnLoadImage_Click(object sender, System.EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择图片";
                openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.gif|所有文件|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    Image loadedImage;
                    using (Image sourceImage = Image.FromFile(openFileDialog.FileName))
                    {
                        loadedImage = new Bitmap(sourceImage);
                    }

                    Image previousImage = pictureBoxTemplate.Image;
                    pictureBoxTemplate.Image = loadedImage;
                    if (previousImage != null)
                        previousImage.Dispose();

                    _currentImageName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    if (string.IsNullOrWhiteSpace(_currentImageName))
                        _currentImageName = "image";

                    UpdateImageStatus();
                    pictureBoxTemplate.FitToWindow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法加载图片: " + ex.Message, "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnFitWindow_Click(object sender, EventArgs e)
        {
            pictureBoxTemplate.FitToWindow();
        }

        private void BtnFlipImage_Click(object sender, EventArgs e)
        {
            Image image = pictureBoxTemplate.Image;
            if (image == null)
                return;

            try
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBoxTemplate.FitToWindow();
                UpdateImageStatus();
                pictureBoxTemplate.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法翻转图片: " + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveImage_Click(object sender, EventArgs e)
        {
            Image image = pictureBoxTemplate.Image;
            if (image == null)
            {
                MessageBox.Show("请先加载图片。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "保存图片";
                saveFileDialog.Filter =
                    "PNG 图片|*.png|JPEG 图片|*.jpg;*.jpeg|BMP 位图|*.bmp|TIFF 图片|*.tif;*.tiff";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.DefaultExt = "png";
                saveFileDialog.AddExtension = true;
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = _currentImageName + "_result";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    SaveImage(image, saveFileDialog.FileName);
                    MessageBox.Show("图片保存成功。\r\n" + saveFileDialog.FileName, "保存完成",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法保存图片: " + ex.Message, "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void SaveImage(Image image, string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    SaveJpeg(image, fileName, 95L);
                    break;
                case ".bmp":
                    image.Save(fileName, ImageFormat.Bmp);
                    break;
                case ".tif":
                case ".tiff":
                    image.Save(fileName, ImageFormat.Tiff);
                    break;
                default:
                    image.Save(fileName, ImageFormat.Png);
                    break;
            }
        }

        private static void SaveJpeg(Image image, string fileName, long quality)
        {
            ImageCodecInfo jpegCodec = null;
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpegCodec = codec;
                    break;
                }
            }

            if (jpegCodec == null)
            {
                image.Save(fileName, ImageFormat.Jpeg);
                return;
            }

            using (EncoderParameters parameters = new EncoderParameters(1))
            {
                parameters.Param[0] = new EncoderParameter(
                    System.Drawing.Imaging.Encoder.Quality,
                    quality);
                image.Save(fileName, jpegCodec, parameters);
            }
        }

        private void PictureBoxTemplate_MouseMove(object sender, MouseEventArgs e)
        {
            Bitmap bitmap = pictureBoxTemplate.Image as Bitmap;
            if (bitmap == null)
            {
                ClearPixelStatus();
                return;
            }

            Point imagePoint;
            if (!pictureBoxTemplate.TryClientToImagePoint(e.Location, out imagePoint))
            {
                ClearPixelStatus();
                return;
            }

            int column = imagePoint.X;
            int row = imagePoint.Y;

            if (column == _lastPixelColumn && row == _lastPixelRow)
                return;

            Color pixel = bitmap.GetPixel(column, row);
            _lastPixelColumn = column;
            _lastPixelRow = row;
            lblPixelInfoStatus.Text = string.Format(
                "Column: {0:D4}  Row: {1:D4},  Val: ({2}, {3}, {4})",
                column,
                row,
                pixel.R,
                pixel.G,
                pixel.B);
        }

        private void PictureBoxTemplate_MouseLeave(object sender, EventArgs e)
        {
            ClearPixelStatus();
        }

        private void UpdateImageStatus()
        {
            Image image = pictureBoxTemplate.Image;
            lblImageSizeStatus.Text = image == null
                ? "图像: -- × --"
                : string.Format("图像: {0} × {1}", image.Width, image.Height);
            ClearPixelStatus();
        }

        private void ClearPixelStatus()
        {
            _lastPixelColumn = -1;
            _lastPixelRow = -1;
            lblPixelInfoStatus.Text = "Column: ----  Row: ----,  Val: (--, --, --)";
        }

        private void TabTemplate_Disposed(object sender, EventArgs e)
        {
            Image image = pictureBoxTemplate.Image;
            pictureBoxTemplate.Image = null;
            if (image != null)
                image.Dispose();
        }
    }
}
