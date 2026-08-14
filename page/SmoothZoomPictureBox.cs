using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _180Detection
{
    /// <summary>
    /// 使用绘制变换而不是改变控件尺寸的平滑缩放图片控件。
    /// </summary>
    public sealed class SmoothZoomPictureBox : PictureBox
    {
        private const double MinZoom = 0.2D;
        private const double MaxZoom = 12D;
        private const double WheelZoomStep = 1.15D;
        private const double AnimationBlend = 0.34D;

        private readonly System.Windows.Forms.Timer _animationTimer;
        private Image _trackedImage;
        private double _currentZoom = 1D;
        private double _targetZoom = 1D;
        private double _centerImageX;
        private double _centerImageY;
        private double _anchorImageX;
        private double _anchorImageY;
        private Point _anchorClientPoint;
        private bool _hasZoomAnchor;
        private bool _interactiveDrawing;
        private bool _dragging;
        private Point _dragStartPoint;
        private double _dragStartCenterX;
        private double _dragStartCenterY;

        public SmoothZoomPictureBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.UserPaint,
                true);
            UpdateStyles();

            BackColor = Color.FromArgb(40, 40, 40);
            TabStop = true;

            _animationTimer = new System.Windows.Forms.Timer();
            _animationTimer.Interval = 15;
            _animationTimer.Tick += AnimationTimer_Tick;
        }

        [Browsable(false)]
        public double ZoomFactor
        {
            get { return _currentZoom; }
        }

        public void FitToWindow()
        {
            _animationTimer.Stop();
            TrackCurrentImage();
            _currentZoom = 1D;
            _targetZoom = 1D;
            _hasZoomAnchor = false;
            _interactiveDrawing = false;
            CenterImage();
            Invalidate();
        }

        public bool TryClientToImagePoint(Point clientPoint, out Point imagePoint)
        {
            imagePoint = Point.Empty;
            Image image = Image;
            if (image == null || ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return false;

            TrackCurrentImage();
            double scale = GetDisplayScale(_currentZoom);
            if (scale <= 0D)
                return false;

            double imageX = _centerImageX +
                (clientPoint.X - ClientSize.Width / 2D) / scale;
            double imageY = _centerImageY +
                (clientPoint.Y - ClientSize.Height / 2D) / scale;

            if (imageX < 0D || imageY < 0D ||
                imageX >= image.Width || imageY >= image.Height)
                return false;

            imagePoint = new Point(
                Math.Max(0, Math.Min(image.Width - 1, (int)Math.Round(imageX))),
                Math.Max(0, Math.Min(image.Height - 1, (int)Math.Round(imageY))));
            return true;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (Image == null)
                return;

            TrackCurrentImage();

            double factor = e.Delta > 0 ? WheelZoomStep : 1D / WheelZoomStep;
            double nextZoom = ClampZoom(_targetZoom * factor);
            if (Math.Abs(nextZoom - _targetZoom) < 0.0001D)
                return;

            double scale = GetDisplayScale(_currentZoom);
            if (scale > 0D)
            {
                _anchorImageX = _centerImageX +
                    (e.X - ClientSize.Width / 2D) / scale;
                _anchorImageY = _centerImageY +
                    (e.Y - ClientSize.Height / 2D) / scale;
                _anchorClientPoint = e.Location;
                _hasZoomAnchor = true;
            }

            _targetZoom = nextZoom;
            _interactiveDrawing = true;
            if (!_animationTimer.Enabled)
                _animationTimer.Start();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();

            if (Image == null || e.Button != MouseButtons.Middle)
                return;

            TrackCurrentImage();
            _animationTimer.Stop();
            _currentZoom = _targetZoom;
            _hasZoomAnchor = false;
            _interactiveDrawing = true;
            _dragging = true;
            _dragStartPoint = e.Location;
            _dragStartCenterX = _centerImageX;
            _dragStartCenterY = _centerImageY;
            Cursor = Cursors.SizeAll;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!_dragging || Image == null)
                return;

            double scale = GetDisplayScale(_currentZoom);
            if (scale <= 0D)
                return;

            _centerImageX = _dragStartCenterX -
                (e.X - _dragStartPoint.X) / scale;
            _centerImageY = _dragStartCenterY -
                (e.Y - _dragStartPoint.Y) / scale;
            ClampCenter();
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Middle)
                return;

            _dragging = false;
            Cursor = Cursors.Default;
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            FitToWindow();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            TrackCurrentImage();
            ClampCenter();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (Image == null)
            {
                base.OnPaint(pe);
                return;
            }

            TrackCurrentImage();

            pe.Graphics.Clear(BackColor);
            pe.Graphics.InterpolationMode = _interactiveDrawing
                ? InterpolationMode.HighQualityBilinear
                : InterpolationMode.HighQualityBicubic;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            pe.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            pe.Graphics.SmoothingMode = SmoothingMode.None;

            double scale = GetDisplayScale(_currentZoom);
            float width = (float)(Image.Width * scale);
            float height = (float)(Image.Height * scale);
            float left = (float)(ClientSize.Width / 2D - _centerImageX * scale);
            float top = (float)(ClientSize.Height / 2D - _centerImageY * scale);

            pe.Graphics.DrawImage(Image, left, top, width, height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer.Stop();
                _animationTimer.Dispose();
            }
            base.Dispose(disposing);
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            double difference = _targetZoom - _currentZoom;
            if (Math.Abs(difference) < 0.002D)
            {
                _currentZoom = _targetZoom;
                ApplyAnchor();
                ClampCenter();
                _animationTimer.Stop();
                _interactiveDrawing = false;
                _hasZoomAnchor = false;
                Invalidate();
                return;
            }

            _currentZoom += difference * AnimationBlend;
            ApplyAnchor();
            ClampCenter();
            Invalidate();
        }

        private void ApplyAnchor()
        {
            if (!_hasZoomAnchor || Image == null)
                return;

            double scale = GetDisplayScale(_currentZoom);
            if (scale <= 0D)
                return;

            _centerImageX = _anchorImageX -
                (_anchorClientPoint.X - ClientSize.Width / 2D) / scale;
            _centerImageY = _anchorImageY -
                (_anchorClientPoint.Y - ClientSize.Height / 2D) / scale;
        }

        private void TrackCurrentImage()
        {
            if (ReferenceEquals(_trackedImage, Image))
                return;

            _trackedImage = Image;
            _currentZoom = 1D;
            _targetZoom = 1D;
            _hasZoomAnchor = false;
            _interactiveDrawing = false;
            CenterImage();
        }

        private void CenterImage()
        {
            Image image = Image;
            if (image == null)
            {
                _centerImageX = 0D;
                _centerImageY = 0D;
                return;
            }

            _centerImageX = image.Width / 2D;
            _centerImageY = image.Height / 2D;
        }

        private void ClampCenter()
        {
            Image image = Image;
            if (image == null || ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return;

            double scale = GetDisplayScale(_currentZoom);
            if (scale <= 0D)
                return;

            double halfVisibleWidth = ClientSize.Width / (2D * scale);
            double halfVisibleHeight = ClientSize.Height / (2D * scale);

            if (halfVisibleWidth >= image.Width / 2D)
                _centerImageX = image.Width / 2D;
            else
                _centerImageX = Math.Max(
                    halfVisibleWidth,
                    Math.Min(image.Width - halfVisibleWidth, _centerImageX));

            if (halfVisibleHeight >= image.Height / 2D)
                _centerImageY = image.Height / 2D;
            else
                _centerImageY = Math.Max(
                    halfVisibleHeight,
                    Math.Min(image.Height - halfVisibleHeight, _centerImageY));
        }

        private double GetDisplayScale(double zoom)
        {
            Image image = Image;
            if (image == null || ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return 0D;

            double fitScale = Math.Min(
                ClientSize.Width / (double)image.Width,
                ClientSize.Height / (double)image.Height);
            return fitScale * zoom;
        }

        private static double ClampZoom(double value)
        {
            return Math.Max(MinZoom, Math.Min(MaxZoom, value));
        }
    }
}
