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
        public VoltageSourceComponent(Point position) : base(position) { }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.LightBlue, Bounds);
            g.DrawRectangle(Pens.Black, Bounds);
            g.DrawString("V", SystemFonts.DefaultFont, Brushes.Black,
                Position.X + 20, Position.Y + 8);
        }
    }
}
