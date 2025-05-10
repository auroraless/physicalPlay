using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pendulum
{
    public class SwitchComponent : CircuitComponent
    {
        public bool IsOn { get; private set; } = true;

        private Rectangle body => new Rectangle(Position.X, Position.Y, Size.Width, Size.Height);

        // 开关的位置决定引脚的位置
        public Point Pin1 => Position;
        public Point Pin2 => new Point(Position.X + Size.Width, Position.Y);

        public SwitchComponent(Point position) : base(position) { }

        public override void Draw(Graphics g)
        {
            // 绘制开关本体
            g.FillRectangle(IsOn ? Brushes.LightGreen : Brushes.LightCoral, body);
            g.DrawRectangle(Pens.Black, body);
            g.DrawString(IsOn ? "ON" : "OFF", SystemFonts.DefaultFont, Brushes.Black, Position.X + 5, Position.Y + 5);
        }

        public void Toggle()
        {
            IsOn = !IsOn;
        }

        public bool Contains(Point p)
        {
            return body.Contains(p);
        }
    }
}
