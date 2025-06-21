using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pendulum
{
    public partial class Form1 : Form
    {
        private CircuitCanvas canvas;
        public Form1()
        {
            InitializeComponent();

            canvas = new CircuitCanvas
            {
                Location = new Point(150, 0),
                Size = new Size(800, 600),
                BackColor = Color.White
            };
            this.Controls.Add(canvas);
        }

        private void addResistorButton_Click(object sender, EventArgs e)
        {
            canvas.StartPlacing("Resistor");
        }

        private void addVoltageButton_Click(object sender, EventArgs e)
        {
            canvas.StartPlacing("Voltage");
        }

        private void bulbButton_Click(object sender, EventArgs e)
        {
            canvas.StartPlacing("Bulb");
        }

        private void sliderButton_Click(object sender, EventArgs e)
        {
            canvas.StartPlacing("SliderResistor");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            canvas.StartPlacing("Switch");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (canvas.IsCircuitClosed())
                statusLabel.Text = "회로가 이미 닫혔다";

            else
                statusLabel.Text = "회로가 닫히지 않았다";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            canvas.ClearAll();
        }
    }
}
