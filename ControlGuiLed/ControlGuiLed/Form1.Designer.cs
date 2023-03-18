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
            this.SpectogramButton = new System.Windows.Forms.Button();
            this.PartyButton = new System.Windows.Forms.Button();
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
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(0, 0);
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
            this.ColorButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.ColorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ColorButton.Location = new System.Drawing.Point(0, 24);
            this.ColorButton.Name = "ColorButton";
            this.ColorButton.Size = new System.Drawing.Size(583, 56);
            this.ColorButton.TabIndex = 1;
            this.ColorButton.Text = "Choose Color";
            this.ColorButton.UseVisualStyleBackColor = false;
            this.ColorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(583, 52);
            this.panel1.TabIndex = 2;
            // 
            // OffButton
            // 
            this.OffButton.BackColor = System.Drawing.Color.LightGray;
            this.OffButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.OffButton.Location = new System.Drawing.Point(0, 132);
            this.OffButton.Name = "OffButton";
            this.OffButton.Size = new System.Drawing.Size(583, 54);
            this.OffButton.TabIndex = 3;
            this.OffButton.Text = "Turn Off";
            this.OffButton.UseVisualStyleBackColor = false;
            this.OffButton.Click += new System.EventHandler(this.OffButton_Click);
            // 
            // BrightnessBar
            // 
            this.BrightnessBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.BrightnessBar.Location = new System.Drawing.Point(0, 199);
            this.BrightnessBar.Maximum = 255;
            this.BrightnessBar.Name = "BrightnessBar";
            this.BrightnessBar.Size = new System.Drawing.Size(583, 45);
            this.BrightnessBar.TabIndex = 5;
            this.BrightnessBar.ValueChanged += new System.EventHandler(this.BrightnessBar_ValueChanged);
            this.BrightnessBar.MouseCaptureChanged += new System.EventHandler(this.BrightnessBar_MouseCaptureChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Brightness:";
            // 
            // RainbowButton
            // 
            this.RainbowButton.BackColor = System.Drawing.Color.LightGray;
            this.RainbowButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.RainbowButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RainbowButton.Location = new System.Drawing.Point(0, 244);
            this.RainbowButton.Name = "RainbowButton";
            this.RainbowButton.Size = new System.Drawing.Size(583, 41);
            this.RainbowButton.TabIndex = 6;
            this.RainbowButton.Text = "Rainbow";
            this.RainbowButton.UseVisualStyleBackColor = false;
            this.RainbowButton.Click += new System.EventHandler(this.RainbowButton_Click);
            // 
            // AmbilightButton
            // 
            this.AmbilightButton.BackColor = System.Drawing.Color.LightGray;
            this.AmbilightButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.AmbilightButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AmbilightButton.Location = new System.Drawing.Point(0, 285);
            this.AmbilightButton.Name = "AmbilightButton";
            this.AmbilightButton.Size = new System.Drawing.Size(583, 41);
            this.AmbilightButton.TabIndex = 7;
            this.AmbilightButton.Text = "Ambilight";
            this.AmbilightButton.UseVisualStyleBackColor = false;
            this.AmbilightButton.Click += new System.EventHandler(this.AmbilightButton_Click);
            // 
            // SpectogramButton
            // 
            this.SpectogramButton.BackColor = System.Drawing.Color.LightGray;
            this.SpectogramButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.SpectogramButton.Location = new System.Drawing.Point(0, 343);
            this.SpectogramButton.Name = "SpectogramButton";
            this.SpectogramButton.Size = new System.Drawing.Size(583, 41);
            this.SpectogramButton.TabIndex = 8;
            this.SpectogramButton.Text = "\"Spectogram\"";
            this.SpectogramButton.UseVisualStyleBackColor = false;
            this.SpectogramButton.Click += new System.EventHandler(this.SpectogramButton_Click);
            // 
            // PartyButton
            // 
            this.PartyButton.BackColor = System.Drawing.Color.LightGray;
            this.PartyButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.PartyButton.Location = new System.Drawing.Point(0, 384);
            this.PartyButton.Name = "PartyButton";
            this.PartyButton.Size = new System.Drawing.Size(583, 41);
            this.PartyButton.TabIndex = 9;
            this.PartyButton.Text = "Party";
            this.PartyButton.UseVisualStyleBackColor = false;
            this.PartyButton.Click += new System.EventHandler(this.Party_Click);
            // 
            // ResetAudioB
            // 
            this.ResetAudioB.Dock = System.Windows.Forms.DockStyle.Top;
            this.ResetAudioB.Location = new System.Drawing.Point(0, 425);
            this.ResetAudioB.Name = "ResetAudioB";
            this.ResetAudioB.Size = new System.Drawing.Size(583, 42);
            this.ResetAudioB.TabIndex = 10;
            this.ResetAudioB.Text = "Reset Sound Device";
            this.ResetAudioB.UseVisualStyleBackColor = true;
            this.ResetAudioB.Click += new System.EventHandler(this.ResetAudioB_Click);
            // 
            // AmbilightTurboC
            // 
            this.AmbilightTurboC.AutoSize = true;
            this.AmbilightTurboC.Dock = System.Windows.Forms.DockStyle.Top;
            this.AmbilightTurboC.Location = new System.Drawing.Point(0, 326);
            this.AmbilightTurboC.Name = "AmbilightTurboC";
            this.AmbilightTurboC.Size = new System.Drawing.Size(583, 17);
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
            this.Controls.Add(this.ResetAudioB);
            this.Controls.Add(this.PartyButton);
            this.Controls.Add(this.SpectogramButton);
            this.Controls.Add(this.AmbilightTurboC);
            this.Controls.Add(this.AmbilightButton);
            this.Controls.Add(this.RainbowButton);
            this.Controls.Add(this.BrightnessBar);
            this.Controls.Add(this.label2);
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

        private System.Windows.Forms.Button SpectogramButton;
        private System.Windows.Forms.Button PartyButton;

        private System.Windows.Forms.Button ResetAudioB;
        private System.Windows.Forms.CheckBox AmbilightTurboC;
    }
}

