using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pendulum
{
    public abstract class CircuitComponent
    {
        public Point Position;
        public Size Size = new Size(60, 30);

        public CircuitComponent(Point position)
        {
            Position = position;
        }

        public Rectangle Bounds => new Rectangle(Position, Size);

        public abstract void Draw(Graphics g);

        public virtual Point[] GetPins()
        {
            return new Point[]
            {
                new Point(Position.X, Position.Y + Size.Height / 2), // 左引脚
                new Point(Position.X + Size.Width, Position.Y + Size.Height / 2) // 右引脚
            };
        }

        public Point? GetNearestPin(Point mouse)
        {
            foreach (var p in GetPins())
            {
                if (GetDistance(mouse, p) < 10)
                    return p;
            }
            return null;
        }

        private double GetDistance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
