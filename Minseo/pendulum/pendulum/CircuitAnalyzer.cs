using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pendulum
{
    public class CircuitAnalyzer
    {
        private List<CircuitComponent> components;

        public CircuitAnalyzer(List<CircuitComponent> components)
        {
            this.components = components;
        }

        public void Compute()
        {
            // 获取电源
            var power = components.OfType<VoltageSourceComponent>().FirstOrDefault();
            if (power == null)
            {
                MessageBox.Show("未找到电源！");
                return;
            }

            // 获取所有有效的电阻器件（电阻、滑动变阻器等）
            double totalResistance = 0;
            foreach (var comp in components)
            {
                if (comp is ResistorComponent || comp is SliderResistorComponent)
                {
                    totalResistance += comp.Value;
                }
            }

            if (totalResistance <= 0)
            {
                MessageBox.Show("电阻为 0，可能短路！");
                return;
            }

            double current = power.Value / totalResistance;

            // 通知灯泡组件更新亮度
            foreach (var comp in components)
            {
                if (comp is BulbComponent bulb)
                {
                    bulb.UpdateBrightness(current); // 你需要在 BulbComponent 中实现这个方法
                }
            }
        }
    }
}
