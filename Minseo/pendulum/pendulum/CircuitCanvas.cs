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
        private string formulaText = "";
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
                else if (placingType == "Switch")
                    components.Add(new SwitchComponent(e.Location));
                placingType = null;
            }
            else
            {
                foreach (var comp in components)
                {
                    if (comp is SwitchComponent sw && sw.Contains(e.Location))
                    {
                        sw.Toggle();
                        this.Invalidate();
                        return;
                    }
                }
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
            if (!string.IsNullOrEmpty(formulaText))
            {
                e.Graphics.DrawString(formulaText, Font, Brushes.Blue, 10, Height - 30);
            }
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
            base.OnMouseUp(e);

            foreach (var comp in components)
            {
                if (comp is SliderResistorComponent slider)
                    slider.OnMouseUp();
            }

            if (IsCircuitClosed())
            {
                var analyzer = new CircuitAnalyzer(components);
                formulaText = analyzer.Compute(); // 获取计算结果字符串
            }
            else
            {
                foreach (var comp in components)
                {
                    if (comp is BulbComponent bulb)
                        bulb.UpdateBrightness(0);
                }

                formulaText = "전기회로가 닫히지 않으면 전류를 계산할 수 없다。";
            }

            Invalidate(); // 触发重绘
        }
        public bool IsCircuitClosed()
        {
            // 1. 构建图：点 -> 相邻点列表
            Dictionary<Point, List<Point>> graph = new Dictionary<Point, List<Point>>();

            // 连接所有连线
            foreach (var wire in wires)
            {
                if (!graph.ContainsKey(wire.P1)) graph[wire.P1] = new List<Point>();
                if (!graph.ContainsKey(wire.P2)) graph[wire.P2] = new List<Point>();
                graph[wire.P1].Add(wire.P2);
                graph[wire.P2].Add(wire.P1);
            }

            // 2. 查找电源的正负极，电源的引脚不能直接连接
            Point? start = null, end = null;
            foreach (var comp in components)
            {
                if (comp is VoltageSourceComponent voltage)
                {
                    var pins = voltage.GetPins();
                    if (pins.Length >= 2)
                    {
                        start = pins[0]; // 正极
                        end = pins[1];   // 负极
                        break;
                    }
                }
            }

            // 如果电源正负极没有连通，则电路不能闭合
            if (start == null || end == null)
            {
                return false;
            }
            foreach (var comp in components)
            {
                if (comp is VoltageSourceComponent)
                {
                    // 跳过电源，不连接电源引脚
                    continue;
                }
                if (comp is SwitchComponent)
                {
                    // 跳过电源，不连接电源引脚
                    continue;
                }

                var pins = comp.GetPins();
                if (pins.Length >= 2)
                {
                    // 自动连接左右引脚
                    if (!graph.ContainsKey(pins[0])) graph[pins[0]] = new List<Point>();
                    if (!graph.ContainsKey(pins[1])) graph[pins[1]] = new List<Point>();
                    graph[pins[0]].Add(pins[1]);
                    graph[pins[1]].Add(pins[0]);
                }
            }

            // 3. 处理开关的状态
            foreach (var comp in components)
            {
                if (comp is SwitchComponent sw && sw.IsOn) // 判断开关是否开启
                {
                    // 开关是开启的，连接它的两个引脚
                    var pins = sw.GetPins(); // 获取开关的引脚
                    if (!graph.ContainsKey(pins[0])) graph[pins[0]] = new List<Point>();
                    if (!graph.ContainsKey(pins[1])) graph[pins[1]] = new List<Point>();
                    graph[pins[0]].Add(pins[1]);
                    graph[pins[1]].Add(pins[0]);
                }
            }

            // 4. 通过 DFS 或 BFS 查找从正极到负极的路径
            HashSet<Point> visited = new HashSet<Point>();
            Stack<Point> stack = new Stack<Point>();
            stack.Push(start.Value);

            while (stack.Count > 0)
            {
                Point current = stack.Pop();
                if (visited.Contains(current)) continue;
                visited.Add(current);

                // 如果到达负极，则电路闭合
                if (current == end.Value) return true;

                if (graph.TryGetValue(current, out var neighbors))
                {
                    foreach (var next in neighbors)
                    {
                        if (!visited.Contains(next))
                            stack.Push(next);
                    }
                }
            }

            return false;
        }

        public void ClearAll()
        {
            components.Clear();
            wires.Clear();
            formulaText = "";
            Invalidate(); // 重新绘制画布
        }

    }
}
