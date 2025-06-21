using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeamProject
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private double angle;
        private double angle0 = 0;
        private double time = 0;
        private double gravity = 9.81;
        private double damping = 0.03;

        private double lengthMeters = 1.0;
        private double lengthPixels = 200;

        private double originX, originY;

        private bool isDragging = false;
        private double dragAngle = 0;

        private double lastZeroCrossTime = -1;
        private bool passedZero = false;
        private double prevAngle = 0;

        private Label labelInitAngle;
        private Label labelInitSpeed;
        private ListBox amplitudeBox;
        private Label labelAngle;
        private Label statusLabel;
        private Label physicsHintLabel;
        private Button resetButton;
        private Label massLabel;
        private NumericUpDown massControl;

        private DateTime lastMoveTime;
        private double lastMoveAngle;
        private double initialAngularVelocity = 0;

        private double mass = 1.0;
        private double previousMass = 1.0;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 600;
            this.Height = 560;

            originX = this.Width / 2;
            originY = 100;

            amplitudeBox = new ListBox();
            amplitudeBox.Width = 200;
            amplitudeBox.Height = 60;
            amplitudeBox.Location = new Point(10, 10);
            this.Controls.Add(amplitudeBox);

            labelInitAngle = new Label();
            labelInitAngle.Location = new Point(10, 80);
            labelInitAngle.AutoSize = true;
            labelInitAngle.Text = "초기 각도: 0.00°";
            this.Controls.Add(labelInitAngle);

            labelInitSpeed = new Label();
            labelInitSpeed.Location = new Point(10, 100);
            labelInitSpeed.AutoSize = true;
            labelInitSpeed.Text = "초기 속도: 0.00 rad/s";
            this.Controls.Add(labelInitSpeed);

            labelAngle = new Label();
            labelAngle.Location = new Point(10, 130);
            labelAngle.AutoSize = true;
            labelAngle.Text = "각도: 0.00°";
            this.Controls.Add(labelAngle);

            statusLabel = new Label();
            statusLabel.Location = new Point(10, 150);
            statusLabel.AutoSize = true;
            statusLabel.ForeColor = Color.DarkBlue;
            statusLabel.Text = "";
            this.Controls.Add(statusLabel);

            physicsHintLabel = new Label();
            physicsHintLabel.Location = new Point(10, 480);
            physicsHintLabel.AutoSize = true;
            physicsHintLabel.ForeColor = Color.DarkGreen;
            physicsHintLabel.Text = "";
            this.Controls.Add(physicsHintLabel);

            massLabel = new Label();
            massLabel.Location = new Point(470, 420);
            massLabel.Size = new Size(100, 20);
            massLabel.Text = "▼ 추 질량";
            this.Controls.Add(massLabel);

            massControl = new NumericUpDown();
            massControl.Minimum = 1;
            massControl.Maximum = 10;
            massControl.DecimalPlaces = 1;
            massControl.Increment = 0.1M;
            massControl.Value = 1.0M;
            massControl.Width = 100;
            massControl.Location = new Point(470, 440);
            massControl.ValueChanged += (s, e) =>
            {
                double newMass = (double)massControl.Value;
                mass = newMass;
                timer.Stop();
                angle = 0;
                dragAngle = 0;
                lastMoveTime = DateTime.Now;
                lastMoveAngle = angle;
                initialAngularVelocity = 0;

                labelAngle.Text = $"각도: 0.00°";
                statusLabel.Text = "질량이 변경되었습니다. 다시 드래그하세요.";

                if (newMass > previousMass)
                {
                    physicsHintLabel.Text = "질량이 무거워져도 진동 속도는 그대로지만, 오래 진동합니다.";
                }
                else if (newMass < previousMass)
                {
                    physicsHintLabel.Text = "질량이 가벼워져도 진동 속도는 그대로지만, 짧게 진동합니다.";
                }

                previousMass = newMass;
                this.Invalidate();
            };
            this.Controls.Add(massControl);

            resetButton = new Button();
            resetButton.Text = "재시작";
            resetButton.Width = 100;
            resetButton.Location = new Point(470, 470);
            resetButton.Click += ResetButton_Click;
            this.Controls.Add(resetButton);

            timer = new Timer();
            timer.Interval = 16;
            timer.Tick += Timer_Tick;

            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            angle = 0;
            angle0 = 0;
            time = 0;
            dragAngle = 0;
            isDragging = false;

            prevAngle = 0;
            lastZeroCrossTime = -1;
            passedZero = false;

            labelAngle.Text = "각도: 0.00°";
            statusLabel.Text = "재시작되었습니다. 다시 드래그하세요.";
            labelInitAngle.Text = "초기 각도: 0.00°";
            labelInitSpeed.Text = "초기 속도: 0.00 rad/s";
            amplitudeBox.Items.Clear();
            physicsHintLabel.Text = "";
            this.Invalidate();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                time += 0.016;
                double omega = Math.Sqrt(gravity / lengthMeters);
                double effectiveDamping = damping / mass;

                angle = angle0 * Math.Exp(-effectiveDamping * time) * Math.Cos(omega * time);

                if (prevAngle * angle < 0 && !passedZero)
                {
                    passedZero = true;

                    if (lastZeroCrossTime >= 0)
                    {
                        double amplitude = angle0 * Math.Exp(-damping * time);
                        double degrees = amplitude * (180.0 / Math.PI);
                        amplitudeBox.Items.Add($"진폭: {degrees:F2}°");
                        amplitudeBox.TopIndex = amplitudeBox.Items.Count - 1;
                    }

                    lastZeroCrossTime = time;
                }

                if (prevAngle * angle > 0)
                {
                    passedZero = false;
                }

                prevAngle = angle;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Pen supportPen = new Pen(Color.DarkSlateGray, 8);
            g.DrawLine(supportPen, (float)originX, 0, (float)originX, (float)originY);

            RectangleF arcRect = new RectangleF((float)originX - 20, -20, 40, 40);
            g.DrawArc(new Pen(Color.DimGray, 3), arcRect, 0, 180);

            Pen pen = new Pen(Color.Black, 2);
            Brush bobBrush = Brushes.DarkRed;

            double drawAngle = isDragging ? dragAngle : angle;
            double x = originX + lengthPixels * Math.Sin(drawAngle);
            double y = originY + lengthPixels * Math.Cos(drawAngle);

            g.DrawLine(pen, (float)originX, (float)originY, (float)x, (float)y);
            float radius = (float)(10 + mass * 5);
            g.FillEllipse(bobBrush, (float)(x - radius), (float)(y - radius), radius * 2, radius * 2);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            double x = originX + lengthPixels * Math.Sin(angle);
            double y = originY + lengthPixels * Math.Cos(angle);
            double dx = e.X - x;
            double dy = e.Y - y;

            double distance = Math.Sqrt(dx * dx + dy * dy);
            if (distance < 25)
            {
                isDragging = true;
                timer.Stop();

                lastMoveTime = DateTime.Now;
                lastMoveAngle = angle;
                initialAngularVelocity = 0;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                double dx = e.X - originX;
                double dy = e.Y - originY;
                dragAngle = Math.Atan2(dx, dy);
                this.Invalidate();

                double degrees = dragAngle * (180.0 / Math.PI);
                labelAngle.Text = $"각도: {degrees:F2}°";

                DateTime now = DateTime.Now;
                TimeSpan delta = now - lastMoveTime;
                if (delta.TotalMilliseconds > 0)
                {
                    double deltaAngle = dragAngle - lastMoveAngle;
                    initialAngularVelocity = deltaAngle / delta.TotalSeconds;
                }

                lastMoveTime = now;
                lastMoveAngle = dragAngle;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                angle0 = dragAngle;
                time = 0;
                lastZeroCrossTime = -1;
                passedZero = false;
                prevAngle = angle0 * Math.Cos(0);

                double degrees = angle0 * (180.0 / Math.PI);
                labelInitAngle.Text = $"초기 각도: {degrees:F2}°";
                labelInitSpeed.Text = $"초기 속도: {initialAngularVelocity:F2} rad/s";

                statusLabel.Text = "";
                timer.Start();
            }
        }
    }
}
