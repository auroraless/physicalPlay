using System;
using System.Drawing;
using System.Windows.Forms;

namespace SinglePendulum
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private double angle;                   // 현재 각도 (라디안)
        private double angle0 = 0;              // 초기 각도
        private double time = 0;
        private double gravity = 9.81;
        private double damping = 0.03;          // 감쇠 계수: 느리게 감쇠됨

        private double lengthMeters = 1.0;      // 물리적 줄 길이 (m)
        private double lengthPixels = 200;      // 화면상 길이 (픽셀)

        private double originX, originY;

        private bool isDragging = false;
        private double dragAngle = 0;

        private double lastZeroCrossTime = -1;
        private bool passedZero = false;
        private double prevAngle = 0;

        private ListBox logBox;
        private Label labelAngle;

        // 드래그 속도 측정용
        private DateTime lastMoveTime;
        private double lastMoveAngle;
        private double initialAngularVelocity = 0;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 600;
            this.Height = 600;

            originX = this.Width / 2;
            originY = 100;

            logBox = new ListBox();
            logBox.Width = 200;
            logBox.Height = 100;
            logBox.Location = new Point(10, 10);
            this.Controls.Add(logBox);

            labelAngle = new Label();
            labelAngle.Location = new Point(10, 120);
            labelAngle.AutoSize = true;
            labelAngle.Text = "각도: 0.00°";
            this.Controls.Add(labelAngle);

            timer = new Timer();
            timer.Interval = 16;
            timer.Tick += Timer_Tick;

            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                time += 0.016;
                double omega = Math.Sqrt(gravity / lengthMeters);

                // 감쇠 진자 공식 적용
                angle = angle0 * Math.Exp(-damping * time) * Math.Cos(omega * time);

                // 중심 통과 감지
                if (prevAngle * angle < 0 && !passedZero)
                {
                    passedZero = true;

                    if (lastZeroCrossTime >= 0)
                    {
                        double amplitude = angle0 * Math.Exp(-damping * time); // 현재 진폭
                        double degrees = amplitude * (180.0 / Math.PI);
                        logBox.Items.Add($"진폭: {degrees:F2}°");
                        logBox.TopIndex = logBox.Items.Count - 1;

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

            // 🔹 지지대 기둥
            Pen supportPen = new Pen(Color.DarkSlateGray, 8);
            g.DrawLine(supportPen, (float)originX, 0, (float)originX, (float)originY);

            // 🔹 고정 고리 반원
            RectangleF arcRect = new RectangleF((float)originX - 20, -20, 40, 40);
            g.DrawArc(new Pen(Color.DimGray, 3), arcRect, 0, 180);

            // 🔹 진자 줄과 추
            Pen pen = new Pen(Color.Black, 2);
            Brush bobBrush = Brushes.DarkRed;

            double drawAngle = isDragging ? dragAngle : angle;
            double x = originX + lengthPixels * Math.Sin(drawAngle);
            double y = originY + lengthPixels * Math.Cos(drawAngle);

            g.DrawLine(pen, (float)originX, (float)originY, (float)x, (float)y);

            float radius = 20;
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

                // 초기화
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

                // 실시간 각도 표시
                double degrees = dragAngle * (180.0 / Math.PI);
                labelAngle.Text = $"각도: {degrees:F2}°";

                // 각속도 추정
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
                prevAngle = angle0 * Math.Cos(0);  // 초기 각도 저장

                double degrees = angle0 * (180.0 / Math.PI);
                logBox.Items.Add($"초기 각도: {degrees:F2}°");
                logBox.Items.Add($"초기 속도: {initialAngularVelocity:F2} rad/s");
                logBox.Items.Add("----------------------------");

                timer.Start();
            }
        }
    }
}
