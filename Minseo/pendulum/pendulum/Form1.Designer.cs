namespace pendulum
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.addResistorButton = new System.Windows.Forms.Button();
            this.addVoltageButton = new System.Windows.Forms.Button();
            this.bulbButton = new System.Windows.Forms.Button();
            this.sliderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addResistorButton
            // 
            this.addResistorButton.Location = new System.Drawing.Point(27, 13);
            this.addResistorButton.Name = "addResistorButton";
            this.addResistorButton.Size = new System.Drawing.Size(75, 23);
            this.addResistorButton.TabIndex = 0;
            this.addResistorButton.Text = "添加电阻";
            this.addResistorButton.UseVisualStyleBackColor = true;
            this.addResistorButton.Click += new System.EventHandler(this.addResistorButton_Click);
            // 
            // addVoltageButton
            // 
            this.addVoltageButton.Location = new System.Drawing.Point(27, 58);
            this.addVoltageButton.Name = "addVoltageButton";
            this.addVoltageButton.Size = new System.Drawing.Size(75, 23);
            this.addVoltageButton.TabIndex = 1;
            this.addVoltageButton.Text = "添加电源";
            this.addVoltageButton.UseVisualStyleBackColor = true;
            this.addVoltageButton.Click += new System.EventHandler(this.addVoltageButton_Click);
            // 
            // bulbButton
            // 
            this.bulbButton.Location = new System.Drawing.Point(27, 105);
            this.bulbButton.Name = "bulbButton";
            this.bulbButton.Size = new System.Drawing.Size(75, 23);
            this.bulbButton.TabIndex = 2;
            this.bulbButton.Text = "灯泡";
            this.bulbButton.UseVisualStyleBackColor = true;
            this.bulbButton.Click += new System.EventHandler(this.bulbButton_Click);
            // 
            // sliderButton
            // 
            this.sliderButton.Location = new System.Drawing.Point(27, 157);
            this.sliderButton.Name = "sliderButton";
            this.sliderButton.Size = new System.Drawing.Size(75, 23);
            this.sliderButton.TabIndex = 3;
            this.sliderButton.Text = "滑动变阻器";
            this.sliderButton.UseVisualStyleBackColor = true;
            this.sliderButton.Click += new System.EventHandler(this.sliderButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 591);
            this.Controls.Add(this.sliderButton);
            this.Controls.Add(this.bulbButton);
            this.Controls.Add(this.addVoltageButton);
            this.Controls.Add(this.addResistorButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addResistorButton;
        private System.Windows.Forms.Button addVoltageButton;
        private System.Windows.Forms.Button bulbButton;
        private System.Windows.Forms.Button sliderButton;
    }
}

