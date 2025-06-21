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

        public string Compute()
        {
            // 获取电源
            var power = components.OfType<VoltageSourceComponent>().FirstOrDefault();
            if (power == null)
            {
                return "전원을 찾을 수 없음！";
            }

            // 获取所有有效的电阻器件
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
                return "저항이 0 이면 단락이 가능하다！";
            }

            double current = power.Value / totalResistance;

            // 通知灯泡更新亮度
            foreach (var comp in components)
            {
                if (comp is BulbComponent bulb)
                {
                    bulb.UpdateBrightness(current);
                }
            }

            // 返回公式字符串
            return $"I = U / R = {power.Value} / {totalResistance} = {Math.Round(current, 4)} A";
        }
    }
}
