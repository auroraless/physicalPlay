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
        private Rectangle sliderArea => new Rectangle(Position.X, Position.Y, Size.Width, Size.Height);
        private bool dragging = false;
        public bool IsDragging => dragging;

        public SliderResistorComponent(Point position) : base(position)
        {
            Value = 100; // 默认电阻值（Ω）
        }

        public override void Draw(Graphics g)
        {
            using (Brush fillBrush = new SolidBrush(Color.BurlyWood)) // 可以改为其他颜色
            {
                g.FillRectangle(fillBrush, sliderArea);
            }
            g.DrawRectangle(Pens.Brown, sliderArea);

            // 显示当前电阻值
            g.DrawString($"{Value:F0}Ω", SystemFonts.DefaultFont, Brushes.Black, Position.X, Position.Y - 15);

            // 画滑块位置（根据电阻值）
            int sliderX = Position.X + (int)(Value * Size.Width / 200);
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
                // 限制值在 1 到 200Ω 之间
                Value = Math.Max(1, Math.Min(200, relativeX * 200.0 / Size.Width));
            }
        }

        public void OnMouseUp()
        {
            dragging = false;
        }
    }
}
