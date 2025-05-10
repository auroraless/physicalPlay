using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pendulum
{
    public class SliderResistorComponent : CircuitComponent
    {
        public int Resistance = 100; // 默认电阻值
        private Rectangle sliderArea => new Rectangle(Position.X, Position.Y, Size.Width, Size.Height);
        private bool dragging = false;
        public bool IsDragging => dragging;
        public SliderResistorComponent(Point position) : base(position) { }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(Pens.Brown, sliderArea);

            // 显示当前电阻值
            g.DrawString($"{Resistance}Ω", SystemFonts.DefaultFont, Brushes.Black, Position.X, Position.Y - 15);

            // 画滑块位置（根据阻值）
            int sliderX = Position.X + Resistance * Size.Width / 200;
            g.FillRectangle(Brushes.Gray, sliderX - 5, Position.Y, 10, Size.Height);
        }

        public void OnMouseDown(Point p)
        {
            if (sliderArea.Contains(p))
                dragging = true;
        }

        public void OnMouseMove(Point p)
        {
            if (dragging)
            {
                int relativeX = p.X - Position.X;
                Resistance = Math.Max(1, Math.Min(200, relativeX * 200 / Size.Width)); // 1~200Ω
            }
        }

        public void OnMouseUp()
        {
            dragging = false;
        }
    }
}
