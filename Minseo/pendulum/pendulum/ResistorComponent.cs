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
        public ResistorComponent(Point position) : base(position)
        {
            Value = 10.0; // 默认电阻值为 100Ω
        }

        public override void Draw(Graphics g)
        {
            // 绘制电阻本体（使用浅黄色的矩形）
            g.FillRectangle(Brushes.LightYellow, Bounds);
            g.DrawRectangle(Pens.Black, Bounds);
            g.DrawString("R", SystemFonts.DefaultFont, Brushes.Black,
                Position.X + 20, Position.Y + 8);

            // 显示电阻的值
            g.DrawString($"{Value}Ω", SystemFonts.DefaultFont, Brushes.Black,
                Position.X, Position.Y - 15);
        }

        // 继承并重写 GetPins() 来确定电阻两端的引脚
        public override Point[] GetPins()
        {
            return new Point[]
            {
                new Point(Position.X, Position.Y + Size.Height / 2),               // 左引脚
                new Point(Position.X + Size.Width, Position.Y + Size.Height / 2)  // 右引脚
            };
        }
    }
}
    