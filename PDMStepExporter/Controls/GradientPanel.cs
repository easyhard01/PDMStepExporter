using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PDMStepExporter
{
    /// <summary>
    /// 顶部紫色渐变横幅。
    /// </summary>
    public class GradientPanel : Panel
    {
        public Color ColorTop { get; set; }
        public Color ColorBottom { get; set; }

        public GradientPanel()
        {
            ColorTop = Theme.Primary;
            ColorBottom = Theme.PrimaryLight;
            BackColor = Theme.Primary;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 不绘制默认背景，由 OnPaint 绘制渐变
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = ClientRectangle;
            if (rc.Width <= 0 || rc.Height <= 0) return;
            using (LinearGradientBrush b = new LinearGradientBrush(rc, ColorTop, ColorBottom, 0f))
            {
                e.Graphics.FillRectangle(b, rc);
            }
            // 底部浅色装饰线
            using (Pen p = new Pen(Color.FromArgb(90, 233, 216, 245)))
            {
                e.Graphics.DrawLine(p, 0, rc.Height - 1, rc.Width, rc.Height - 1);
            }
        }
    }
}
