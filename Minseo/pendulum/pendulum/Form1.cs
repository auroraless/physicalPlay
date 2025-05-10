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





            x_pos = pictureBox1.Location.X;
            pictureBox2.SendToBack();
            timer1.Start();
        }

        public Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();
            return bmp;
        }


        private static float num = 0;
        private static float speed = 0;
        private const float gravity = -0.5f;
        private static float v_pos = 0;
        private static int x_pos;
        private static float t = 0.3f;

        private const float line = 300;
        private const float friction = 0.01f;
        private const float _V = 2f;
        private const float size = 30;

        private void timer1_Tick(object sender, EventArgs e)
        {
            int y_pos = pictureBox1.Location.Y;
            float r_pos = 500 - y_pos + v_pos;

            if (r_pos < 0 && speed < 0)
            {
                speed *= -1;
            }



            float d = line - r_pos;
            float force;

            if (d > size / 2)
            {
                force = _V + gravity;
            }
            else if (d < size / 2)
            {
                force = gravity;
            }
            else
            {
                force = _V * (d + size / 2) + gravity;
            }

            force -= speed * friction;

            speed += force * t;
            r_pos = r_pos + speed * t;


            listBox1.Items.Add("speed:" + (speed).ToString() +
                ",force:" + (force).ToString() +
                ",pos:" + (r_pos).ToString());
            listBox1.SelectedIndex = listBox1.Items.Count - 1;

            int i_pos = (int)r_pos;
            v_pos = r_pos - i_pos;
            pictureBox1.Location = new Point(x_pos, 500 - i_pos);
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
