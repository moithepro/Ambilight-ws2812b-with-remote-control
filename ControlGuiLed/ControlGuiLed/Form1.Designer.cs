namespace ControlGuiLed {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.ColorButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OffButton = new System.Windows.Forms.Button();
            this.BrightnessBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.RainbowButton = new System.Windows.Forms.Button();
            this.AmbilightButton = new System.Windows.Forms.Button();
            this.AmbilightTimer = new System.Windows.Forms.Timer(this.components);
            this.RainbowTimer = new System.Windows.Forms.Timer(this.components);
            this.ColorTimer = new System.Windows.Forms.Timer(this.components);
            this.SpectogramButton = new System.Windows.Forms.Button();
            this.PartyButton = new System.Windows.Forms.Button();
            this.PartyTimer = new System.Windows.Forms.Timer(this.components);
            this.ResetAudioB = new System.Windows.Forms.Button();
            this.AmbilightTurboC = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.BrightnessBar)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Control";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(232, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Not Started";
            // 
            // colorDialog1
            // 
            this.colorDialog1.Color = System.Drawing.Color.Red;
            // 
            // ColorButton
            // 
            this.ColorButton.BackColor = System.Drawing.Color.LightGray;
            this.ColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ColorButton.Location = new System.Drawing.Point(221, 256);
            this.ColorButton.Name = "ColorButton";
            this.ColorButton.Size = new System.Drawing.Size(123, 38);
            this.ColorButton.TabIndex = 1;
            this.ColorButton.Text = "Choose Color";
            this.ColorButton.UseVisualStyleBackColor = false;
            this.ColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(221, 310);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(123, 34);
            this.panel1.TabIndex = 2;
            // 
            // OffButton
            // 
            this.OffButton.BackColor = System.Drawing.Color.LightGray;
            this.OffButton.Location = new System.Drawing.Point(236, 188);
            this.OffButton.Name = "OffButton";
            this.OffButton.Size = new System.Drawing.Size(98, 36);
            this.OffButton.TabIndex = 3;
            this.OffButton.Text = "Turn Off";
            this.OffButton.UseVisualStyleBackColor = false;
            this.OffButton.Click += new System.EventHandler(this.OffButton_Click);
            // 
            // BrightnessBar
            // 
            this.BrightnessBar.Location = new System.Drawing.Point(1, 433);
            this.BrightnessBar.Maximum = 255;
            this.BrightnessBar.Name = "BrightnessBar";
            this.BrightnessBar.Size = new System.Drawing.Size(584, 45);
            this.BrightnessBar.TabIndex = 4;
            this.BrightnessBar.ValueChanged += new System.EventHandler(this.BrightnessBar_ValueChanged);
            this.BrightnessBar.MouseCaptureChanged += new System.EventHandler(this.BrightnessBar_MouseCaptureChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 417);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Brightness:";
            // 
            // RainbowButton
            // 
            this.RainbowButton.BackColor = System.Drawing.Color.LightGray;
            this.RainbowButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RainbowButton.Location = new System.Drawing.Point(377, 264);
            this.RainbowButton.Name = "RainbowButton";
            this.RainbowButton.Size = new System.Drawing.Size(100, 23);
            this.RainbowButton.TabIndex = 6;
            this.RainbowButton.Text = "Rainbow";
            this.RainbowButton.UseVisualStyleBackColor = false;
            this.RainbowButton.Click += new System.EventHandler(this.RainbowButton_Click);
            // 
            // AmbilightButton
            // 
            this.AmbilightButton.BackColor = System.Drawing.Color.LightGray;
            this.AmbilightButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AmbilightButton.Location = new System.Drawing.Point(91, 264);
            this.AmbilightButton.Name = "AmbilightButton";
            this.AmbilightButton.Size = new System.Drawing.Size(100, 23);
            this.AmbilightButton.TabIndex = 7;
            this.AmbilightButton.Text = "Ambilight";
            this.AmbilightButton.UseVisualStyleBackColor = false;
            this.AmbilightButton.Click += new System.EventHandler(this.AmbilightButton_Click);
            // 
            // AmbilightTimer
            // 
            this.AmbilightTimer.Interval = 80;
            this.AmbilightTimer.Tick += new System.EventHandler(this.AmbilightTimer_Tick);
            // 
            // RainbowTimer
            // 
            this.RainbowTimer.Interval = 60;
            this.RainbowTimer.Tick += new System.EventHandler(this.RainbowTimer_Tick);
            // 
            // ColorTimer
            // 
            this.ColorTimer.Interval = 40;
            this.ColorTimer.Tick += new System.EventHandler(this.ColorTimer_Tick);
            // 
            // SpectogramButton
            // 
            this.SpectogramButton.BackColor = System.Drawing.Color.LightGray;
            this.SpectogramButton.Location = new System.Drawing.Point(236, 375);
            this.SpectogramButton.Name = "SpectogramButton";
            this.SpectogramButton.Size = new System.Drawing.Size(98, 23);
            this.SpectogramButton.TabIndex = 8;
            this.SpectogramButton.Text = "\"Spectogram\"";
            this.SpectogramButton.UseVisualStyleBackColor = false;
            this.SpectogramButton.Click += new System.EventHandler(this.SpectogramButton_Click);
            // 
            // PartyButton
            // 
            this.PartyButton.BackColor = System.Drawing.Color.LightGray;
            this.PartyButton.Location = new System.Drawing.Point(377, 225);
            this.PartyButton.Name = "PartyButton";
            this.PartyButton.Size = new System.Drawing.Size(100, 23);
            this.PartyButton.TabIndex = 9;
            this.PartyButton.Text = "Party";
            this.PartyButton.UseVisualStyleBackColor = false;
            this.PartyButton.Click += new System.EventHandler(this.Party_Click);
            // 
            // PartyTimer
            // 
            this.PartyTimer.Interval = 60;
            this.PartyTimer.Tick += new System.EventHandler(this.PartyTimer_Tick);
            // 
            // ResetAudioB
            // 
            this.ResetAudioB.Location = new System.Drawing.Point(454, 0);
            this.ResetAudioB.Name = "ResetAudioB";
            this.ResetAudioB.Size = new System.Drawing.Size(131, 24);
            this.ResetAudioB.TabIndex = 10;
            this.ResetAudioB.Text = "Reset Sound Device";
            this.ResetAudioB.UseVisualStyleBackColor = true;
            this.ResetAudioB.Click += new System.EventHandler(this.ResetAudioB_Click);
            // 
            // AmbilightTurboC
            // 
            this.AmbilightTurboC.AutoSize = true;
            this.AmbilightTurboC.Location = new System.Drawing.Point(91, 229);
            this.AmbilightTurboC.Name = "AmbilightTurboC";
            this.AmbilightTurboC.Size = new System.Drawing.Size(99, 17);
            this.AmbilightTurboC.TabIndex = 11;
            this.AmbilightTurboC.Text = "Ambilight Turbo";
            this.AmbilightTurboC.UseVisualStyleBackColor = true;
            this.AmbilightTurboC.CheckedChanged += new System.EventHandler(this.AmbilightTurboC_CheckedChanged);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(583, 475);
            this.Controls.Add(this.AmbilightTurboC);
            this.Controls.Add(this.ResetAudioB);
            this.Controls.Add(this.PartyButton);
            this.Controls.Add(this.SpectogramButton);
            this.Controls.Add(this.AmbilightButton);
            this.Controls.Add(this.RainbowButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BrightnessBar);
            this.Controls.Add(this.OffButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ColorButton);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BrightnessBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button ColorButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OffButton;
        private System.Windows.Forms.TrackBar BrightnessBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RainbowButton;
        private System.Windows.Forms.Button AmbilightButton;
        private System.Windows.Forms.Timer AmbilightTimer;
        private System.Windows.Forms.Timer RainbowTimer;
        private System.Windows.Forms.Timer ColorTimer;
        private System.Windows.Forms.Button SpectogramButton;
        private System.Windows.Forms.Button PartyButton;
        private System.Windows.Forms.Timer PartyTimer;
        private System.Windows.Forms.Button ResetAudioB;
        private System.Windows.Forms.CheckBox AmbilightTurboC;
    }
}

