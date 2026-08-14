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

        private readonly Timer _animationTimer;
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

            _animationTimer = new Timer();
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

            RectangleF imageRectangle = GetImageRectangle(scale);
            if (!imageRectangle.Contains(clientPoint))
                return false;

            int column = (int)Math.Floor((clientPoint.X - imageRectangle.Left) / scale);
            int row = (int)Math.Floor((clientPoint.Y - imageRectangle.Top) / scale);
            column = Math.Max(0, Math.Min(image.Width - 1, column));
            row = Math.Max(0, Math.Min(image.Height - 1, row));
            imagePoint = new Point(column, row);
            return true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Focus();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            QueueSmoothZoom(e.Delta, e.Location);
            HandledMouseEventArgs handledEvent = e as HandledMouseEventArgs;
            if (handledEvent != null)
                handledEvent.Handled = true;
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            FitToWindow();
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && Image != null)
            {
                _dragging = true;
                _dragStartPoint = e.Location;
                _dragStartCenterX = _centerImageX;
                _dragStartCenterY = _centerImageY;
                _interactiveDrawing = true;
                Capture = true;
                Cursor = Cursors.SizeAll;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_dragging && Image != null)
            {
                double scale = GetDisplayScale(_currentZoom);
                if (scale > 0D)
                {
                    _centerImageX = _dragStartCenterX - (e.X - _dragStartPoint.X) / scale;
                    _centerImageY = _dragStartCenterY - (e.Y - _dragStartPoint.Y) / scale;
                    ClampImageCenter(scale);
                    Invalidate();
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_dragging && e.Button == MouseButtons.Middle)
            {
                _dragging = false;
                _interactiveDrawing = false;
                Capture = false;
                Cursor = Cursors.Default;
                Invalidate();
            }
            base.OnMouseUp(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            TrackCurrentImage();
            ClampImageCenter(GetDisplayScale(_currentZoom));
            base.OnSizeChanged(e);
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.Clear(BackColor);
            Image image = Image;
            if (image == null || ClientSize.Width <= 0 || ClientSize.Height <= 0)
                return;

            TrackCurrentImage();
            double scale = GetDisplayScale(_currentZoom);
            if (scale <= 0D)
                return;

            RectangleF imageRectangle = GetImageRectangle(scale);
            RectangleF visibleRectangle = RectangleF.Intersect(
                imageRectangle,
                new RectangleF(0F, 0F, ClientSize.Width, ClientSize.Height));
            if (visibleRectangle.Width <= 0F || visibleRectangle.Height <= 0F)
                return;

            RectangleF sourceRectangle = new RectangleF(
                (float)((visibleRectangle.Left - imageRectangle.Left) / scale),
                (float)((visibleRectangle.Top - imageRectangle.Top) / scale),
                (float)(visibleRectangle.Width / scale),
                (float)(visibleRectangle.Height / scale));

            pe.Graphics.CompositingMode = CompositingMode.SourceCopy;
            pe.Graphics.CompositingQuality = _interactiveDrawing
                ? CompositingQuality.HighSpeed
                : CompositingQuality.AssumeLinear;
            pe.Graphics.InterpolationMode = _interactiveDrawing
                ? InterpolationMode.Bilinear
                : InterpolationMode.HighQualityBilinear;
            pe.Graphics.PixelOffsetMode = _interactiveDrawing
                ? PixelOffsetMode.HighSpeed
                : PixelOffsetMode.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.None;
            pe.Graphics.DrawImage(image, visibleRectangle, sourceRectangle, GraphicsUnit.Pixel);
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

        private void QueueSmoothZoom(int wheelDelta, Point clientPoint)
        {
            Image image = Image;
            if (image == null || wheelDelta == 0)
                return;

            TrackCurrentImage();
            double currentScale = GetDisplayScale(_currentZoom);
            if (currentScale <= 0D)
                return;

            _anchorImageX = _centerImageX +
                (clientPoint.X - ClientSize.Width / 2D) / currentScale;
            _anchorImageY = _centerImageY +
                (clientPoint.Y - ClientSize.Height / 2D) / currentScale;
            _anchorImageX = Math.Max(0D, Math.Min(image.Width, _anchorImageX));
            _anchorImageY = Math.Max(0D, Math.Min(image.Height, _anchorImageY));
            _anchorClientPoint = clientPoint;
            _hasZoomAnchor = true;

            double wheelSteps = (double)wheelDelta / SystemInformation.MouseWheelScrollDelta;
            _targetZoom *= Math.Pow(WheelZoomStep, wheelSteps);
            _targetZoom = Math.Max(MinZoom, Math.Min(MaxZoom, _targetZoom));
            _interactiveDrawing = true;
            if (!_animationTimer.Enabled)
                _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (Image == null)
            {
                _animationTimer.Stop();
                return;
            }

            double logarithmicDistance = Math.Log(_targetZoom / _currentZoom);
            bool finished = Math.Abs(logarithmicDistance) < 0.0015D;
            _currentZoom = finished
                ? _targetZoom
                : _currentZoom * Math.Exp(logarithmicDistance * AnimationBlend);

            double scale = GetDisplayScale(_currentZoom);
            if (_hasZoomAnchor && scale > 0D)
            {
                _centerImageX = _anchorImageX -
                    (_anchorClientPoint.X - ClientSize.Width / 2D) / scale;
                _centerImageY = _anchorImageY -
                    (_anchorClientPoint.Y - ClientSize.Height / 2D) / scale;
            }
            ClampImageCenter(scale);
            Invalidate();

            if (finished)
            {
                _animationTimer.Stop();
                _hasZoomAnchor = false;
                _interactiveDrawing = false;
                Invalidate();
            }
        }

        private void TrackCurrentImage()
        {
            if (ReferenceEquals(_trackedImage, Image))
                return;

            _trackedImage = Image;
            _animationTimer.Stop();
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

        private double GetDisplayScale(double zoom)
        {
            Image image = Image;
            if (image == null || image.Width <= 0 || image.Height <= 0 ||
                ClientSize.Width <= 0 || ClientSize.Height <= 0)
            {
                return 0D;
            }

            double fitScale = Math.Min(
                (double)ClientSize.Width / image.Width,
                (double)ClientSize.Height / image.Height);
            return fitScale * zoom;
        }

        private RectangleF GetImageRectangle(double scale)
        {
            Image image = Image;
            float width = (float)(image.Width * scale);
            float height = (float)(image.Height * scale);
            float left = (float)(ClientSize.Width / 2D - _centerImageX * scale);
            float top = (float)(ClientSize.Height / 2D - _centerImageY * scale);
            return new RectangleF(left, top, width, height);
        }

        private void ClampImageCenter(double scale)
        {
            Image image = Image;
            if (image == null || scale <= 0D)
                return;

            double displayWidth = image.Width * scale;
            if (displayWidth <= ClientSize.Width)
            {
                _centerImageX = image.Width / 2D;
            }
            else
            {
                double visibleHalfWidth = ClientSize.Width / (2D * scale);
                _centerImageX = Math.Max(
                    visibleHalfWidth,
                    Math.Min(image.Width - visibleHalfWidth, _centerImageX));
            }

            double displayHeight = image.Height * scale;
            if (displayHeight <= ClientSize.Height)
            {
                _centerImageY = image.Height / 2D;
            }
            else
            {
                double visibleHalfHeight = ClientSize.Height / (2D * scale);
                _centerImageY = Math.Max(
                    visibleHalfHeight,
                    Math.Min(image.Height - visibleHalfHeight, _centerImageY));
            }
        }
    }
}
