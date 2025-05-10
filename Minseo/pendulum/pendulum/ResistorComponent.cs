using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pendulum
{
    internal class ResistorComponent : CircuitComponent
    {
        public ResistorComponent(Point position) : base(position) { }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.LightYellow, Bounds);
            g.DrawRectangle(Pens.Black, Bounds);
            g.DrawString("R", SystemFonts.DefaultFont, Brushes.Black,
                Position.X + 20, Position.Y + 8);
        }
    }
}
