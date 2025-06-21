using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pendulum
{
    internal class Wire
    {
        public Point P1, P2;
        public Wire(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
}
