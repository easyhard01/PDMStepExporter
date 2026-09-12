using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PDMStepExporter
{
    /// <summary>
    /// 圆角扁平按钮。Primary=true 为紫色实心主按钮，false 为白底紫边次按钮。
    /// </summary>
    public class FlatButton : Button
    {
        private bool _hover;
        private bool _down;

        public bool Primary { get; set; }

        public FlatButton()
        {
            Primary = true;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
            Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
            ForeColor = Color.White;
            Size = new Size(120, 34);
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            _down = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _down = true;
            Invalidate();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _down = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rc = new Rectangle(0, 0, Width - 1, Height - 1);
            int d = 8;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rc.X, rc.Y, d, d, 180, 90);
            path.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
            path.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
            path.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            Color back;
            Color fore;
            Color border = Color.Transparent;
            if (Primary)
            {
                back = _down ? Theme.PrimaryDark : (_hover ? Theme.PrimaryHover : Theme.Primary);
                fore = Color.White;
            }
            else
            {
                back = _down ? Theme.GhostHover : (_hover ? Theme.GhostHover : Color.White);
                fore = Theme.Primary;
                border = _hover ? Theme.Primary : Theme.PrimaryLight;
            }

            using (SolidBrush b = new SolidBrush(back))
            {
                e.Graphics.FillPath(b, path);
            }
            if (border != Color.Transparent)
            {
                using (Pen p = new Pen(border, 1.2f))
                {
                    e.Graphics.DrawPath(p, path);
                }
            }

            Font f = Font;
            if (Primary) f = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, Text, f, rc, fore,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            if (Primary) f.Dispose();
        }
    }
}
