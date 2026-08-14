using System.Windows.Forms;

namespace _180Detection
{
    /// <summary>
    /// 可接收鼠标滚轮焦点并启用双缓冲的图片视口。
    /// </summary>
    public sealed class ZoomViewportPanel : Panel
    {
        public ZoomViewportPanel()
        {
            SetStyle(ControlStyles.Selectable, true);
            DoubleBuffered = true;
            TabStop = true;
        }
    }
}
