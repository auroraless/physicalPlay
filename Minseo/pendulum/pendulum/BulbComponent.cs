using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pendulum
{
    public class BulbComponent : CircuitComponent
    {
        public double Brightness { get; private set; }
        public void UpdateBrightness(double current)
        {
            Brightness = Math.Min(current, 1.0); // 可根据需求缩放亮度
                                                 // 你可以在 Draw() 函数中根据 Brightness 改变颜色、透明度等
        }

        public BulbComponent(Point position) : base(position) { }

        public override void Draw(Graphics g)
        {
            var bulbRect = Bounds;

            // 灯泡外框
            g.DrawEllipse(Pens.Black, bulbRect);

            // 模拟亮度：黄色填充，透明度由 Brightness 决定
            int alpha = (int)(Brightness * 255);
            alpha = Math.Max(0, Math.Min(255, alpha));
            using (Brush b = new SolidBrush(Color.FromArgb(alpha, Color.Yellow)))
            {
                g.FillEllipse(b, bulbRect);
            }
        }
    }
}
