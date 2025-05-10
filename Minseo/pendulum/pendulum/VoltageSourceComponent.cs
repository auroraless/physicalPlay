using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pendulum
{
    public class VoltageSourceComponent : CircuitComponent
    {
        public VoltageSourceComponent(Point position) : base(position)
        {
            Value = 5.0; // 默认电压值为 5V
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.LightBlue, Bounds);
            g.DrawRectangle(Pens.Black, Bounds);
            g.DrawString("V", SystemFonts.DefaultFont, Brushes.Black,
                Position.X + 20, Position.Y + 8);
            g.DrawString($"{Value}V", SystemFonts.DefaultFont, Brushes.Black,
                Position.X, Position.Y - 15);
        }

        public override Point[] GetPins()
        {
            return new Point[]
            {
            new Point(Position.X, Position.Y + Size.Height / 2),               // 左极
            new Point(Position.X + Size.Width, Position.Y + Size.Height / 2)  // 右极
            };
        }
    }
}
