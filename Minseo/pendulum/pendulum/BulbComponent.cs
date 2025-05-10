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
        public float Brightness = 0f; // 0~1 的亮度值（由电路逻辑计算设置）

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
