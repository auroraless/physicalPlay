using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pendulum
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            pictureBox2.SendToBack();
            timer1.Start();
        }


        private float force = 0;
        private static float speed = 0;
        private const float gravity = -0.5f;
        private static float position = 0;

        private static float delta_time = 0.3f;

        private const float line = 300;
        private const float friction_공기 = 0.01f;
        private const float friction_물 = 0.1f;
        private const float _V = 2f;
        private const float size = 30;

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (position < 0 && speed < 0)
            {
                speed *= -1;
            }

            force = 0;

            force += gravity;
            force += Force_buoyancy();
            force += speed * Friction();


            speed += force * delta_time;
            position = position + speed * delta_time;

            Print();

            pictureBox1.Location = new Point(pictureBox1.Location.X, 500 - (int)position);
        }

        private float Force_buoyancy()
        {
            float d = line - position;
            float force=0;

            if (d > size / 2)
            {
                force = _V;
            }
            else if (d < size / 2)
            {
                
            }
            else
            {
                force = _V * (d + size / 2) ;
            }

            return force;
        }

        private float Friction()
        {
            float friction;
            if (position + size / 2 < line)
            {
                friction = friction_물;
            }
            else if (position - size / 2 > line)
            {
                friction = friction_공기;
            }
            else if (speed > 0)
            {
                friction = friction_공기;
            }
            else
            {
                friction = friction_물;
            }

            return -friction;
        }

        private void Print()
        {

            listBox1.Items.Add("speed:" + (speed).ToString() +
                ",force:" + (force).ToString() +
                ",pos:" + (position).ToString());
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void timeStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timeReStart_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
