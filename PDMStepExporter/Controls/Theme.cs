using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PDMStepExporter
{
    /// <summary>
    /// 清华紫主题：统一颜色与常用界面工具。
    /// </summary>
    public static class Theme
    {
        // 清华紫 主色 #660099
        public static readonly Color Primary      = Color.FromArgb(102, 0, 153);
        public static readonly Color PrimaryDark  = Color.FromArgb(74, 0, 114);
        public static readonly Color PrimaryLight = Color.FromArgb(155, 89, 182);
        public static readonly Color PrimaryHover = Color.FromArgb(122, 0, 184);
        public static readonly Color GhostHover   = Color.FromArgb(243, 232, 250);
        public static readonly Color PageBack     = Color.FromArgb(244, 238, 249);
        public static readonly Color Splitter     = Color.FromArgb(237, 227, 245);
        public static readonly Color TextMain     = Color.FromArgb(51, 51, 51);
        public static readonly Color TextSub      = Color.FromArgb(138, 111, 158);
        public static readonly Color GridHeader   = Color.FromArgb(102, 0, 153);
        public static readonly Color GridAlt      = Color.FromArgb(247, 242, 251);
        public static readonly Color GridSelect   = Color.FromArgb(233, 216, 245);
        public static readonly Color OkColor      = Color.FromArgb(39, 174, 96);
        public static readonly Color ErrColor     = Color.FromArgb(224, 63, 60);
        public static readonly Color Placeholder  = Color.FromArgb(165, 150, 178);

        /// <summary>为控件设置圆角区域。</summary>
        public static void MakeRounded(Control c, int radius)
        {
            if (c == null || c.Width <= 0 || c.Height <= 0) return;
            int d = radius * 2;
            Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            c.Region = new Region(p);
        }

        /// <summary>
        /// 为文本框设置占位符（masked=true 时密码框显示为圆点）。
        /// </summary>
        public static void SetPlaceholder(TextBox tb, string text, bool masked)
        {
            tb.Tag = text;
            tb.Text = text;
            tb.ForeColor = Theme.Placeholder;
            if (masked) tb.UseSystemPasswordChar = false;
            tb.GotFocus += delegate(object s, EventArgs e)
            {
                TextBox t = (TextBox)s;
                if (t.Text == (string)t.Tag)
                {
                    t.Text = "";
                    t.ForeColor = Theme.TextMain;
                    if (masked) t.UseSystemPasswordChar = true;
                }
            };
            tb.Leave += delegate(object s, EventArgs e)
            {
                TextBox t = (TextBox)s;
                if (t.Text.Trim().Length == 0)
                {
                    t.UseSystemPasswordChar = false;
                    t.ForeColor = Theme.Placeholder;
                    t.Text = (string)t.Tag;
                }
            };
        }

        /// <summary>判断文本框当前是否处于“占位符”状态。</summary>
        public static bool IsPlaceholder(TextBox tb)
        {
            object tag = tb.Tag;
            return tag != null && tb.Text == (string)tag;
        }
    }
}
