using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pendulum
{
    public class CircuitCanvas : Panel
    {
        private List<CircuitComponent> components = new List<CircuitComponent>();
        private List<Wire> wires = new List<Wire>();
        //private string placingType = null;
        private Point? wireStartPoint = null;
        public string placingType { get; private set; }
        public void StartPlacing(string type)
        {
            placingType = type;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (placingType != null)
            {
                if (placingType == "Resistor")
                    components.Add(new ResistorComponent(e.Location));
                else if (placingType == "Voltage")
                    components.Add(new VoltageSourceComponent(e.Location));
                else if (placingType == "Bulb")
                    components.Add(new BulbComponent(e.Location));
                else if (placingType == "SliderResistor")
                    components.Add(new SliderResistorComponent(e.Location));
                placingType = null;
            }
            else
            {
                // 如果正在尝试连线
                foreach (var comp in components)
                {
                    var pin = comp.GetNearestPin(e.Location);
                    if (pin.HasValue && GetDistance(e.Location, pin.Value) < 10)
                    {
                        if (wireStartPoint == null)
                            wireStartPoint = pin;
                        else
                        {
                            wires.Add(new Wire(wireStartPoint.Value, pin.Value));
                            wireStartPoint = null;
                        }
                        break;
                    }
                }
            }

            this.Invalidate(); // 触发重绘
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 画所有连线
            foreach (var w in wires)
                e.Graphics.DrawLine(Pens.Black, w.P1, w.P2);

            // 画所有元件
            foreach (var c in components)
                c.Draw(e.Graphics);
        }

        // 计算两点之间的欧几里得距离
        private double GetDistance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            foreach (var comp in components)
            {
                if (comp is SliderResistorComponent slider)
                    slider.OnMouseDown(e.Location);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool needsRedraw = false;

            foreach (var comp in components)
            {
                if (comp is SliderResistorComponent slider)
                {
                    slider.OnMouseMove(e.Location);
                    if (slider.IsDragging)
                        needsRedraw = true; // 只有在拖动时才标记为需要重绘
                }
            }

            if (needsRedraw)
                this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            foreach (var comp in components)
            {
                if (comp is SliderResistorComponent slider)
                    slider.OnMouseUp();
            }
        }

    }
}
