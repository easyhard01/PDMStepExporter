using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PDMStepExporter
{
    /// <summary>
    /// 紫色渐变进度条（自绘）。
    /// </summary>
    public class PurpleProgressBar : Control
    {
        private int _min;
        private int _max;
        private int _value;

        public bool ShowText { get; set; }

        public PurpleProgressBar()
        {
            _min = 0;
            _max = 100;
            _value = 0;
            ShowText = true;
            Font = new Font("Microsoft YaHei UI", 8.5F);
            Size = new Size(300, 20);
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        public int Minimum
        {
            get { return _min; }
            set { _min = value; if (_max <= _min) _max = _min + 1; Invalidate(); }
        }

        public int Maximum
        {
            get { return _max; }
            set { _max = Math.Max(value, _min + 1); Invalidate(); }
        }

        public int Value
        {
            get { return _value; }
            set { _value = Math.Max(_min, Math.Min(_max, value)); Invalidate(); }
        }

        public void SetRange(int min, int max)
        {
            _min = min;
            _max = Math.Max(max, min + 1);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rc = new Rectangle(0, 0, Width - 1, Height - 1);
            int d = Math.Max(Height - 2, 2);

            GraphicsPath track = Rounded(rc, d);
            using (SolidBrush b = new SolidBrush(Color.FromArgb(237, 227, 245)))
            {
                e.Graphics.FillPath(b, track);
            }

            float ratio = (_max - _min) == 0 ? 0f : (float)(_value - _min) / (float)(_max - _min);
            int fillW = (int)((rc.Width - 2) * ratio);
            if (fillW > 2)
            {
                Rectangle frc = new Rectangle(rc.X + 1, rc.Y + 1, fillW, rc.Height - 2);
                using (LinearGradientBrush b = new LinearGradientBrush(frc, Theme.Primary, Theme.PrimaryLight, 0f))
                {
                    using (GraphicsPath fp = Rounded(frc, d))
                    {
                        e.Graphics.FillPath(b, fp);
                    }
                }
            }

            if (ShowText)
            {
                string txt = _value + " / " + _max;
                TextRenderer.DrawText(e.Graphics, txt, Font, rc, Color.FromArgb(90, 60, 110),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private static GraphicsPath Rounded(Rectangle r, int d)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
