namespace ControlGuiLed
{
    partial class MainApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
            notifyIcon1 = new NotifyIcon(components);
            label1 = new Label();
            colorDialog1 = new ColorDialog();
            ColorButton = new Button();
            panel1 = new Panel();
            OffButton = new Button();
            BrightnessBar = new TrackBar();
            label2 = new Label();
            RainbowButton = new Button();
            AmbilightButton = new Button();
            SpectogramButton = new Button();
            PartyButton = new Button();
            ResetAudioB = new Button();
            ambilightInterval = new TrackBar();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)BrightnessBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ambilightInterval).BeginInit();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Control";
            notifyIcon1.Visible = true;
            notifyIcon1.Click += notifyIcon1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(102, 24);
            label1.TabIndex = 0;
            label1.Text = "Not Started";
            // 
            // colorDialog1
            // 
            colorDialog1.Color = Color.Red;
            // 
            // ColorButton
            // 
            ColorButton.BackColor = Color.LightGray;
            ColorButton.Dock = DockStyle.Top;
            ColorButton.ForeColor = SystemColors.ControlText;
            ColorButton.Location = new Point(0, 24);
            ColorButton.Margin = new Padding(4, 3, 4, 3);
            ColorButton.Name = "ColorButton";
            ColorButton.Size = new Size(680, 65);
            ColorButton.TabIndex = 1;
            ColorButton.Text = "Choose Color";
            ColorButton.UseVisualStyleBackColor = false;
            ColorButton.Click += ColorButton_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Red;
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 89);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(680, 60);
            panel1.TabIndex = 2;
            // 
            // OffButton
            // 
            OffButton.BackColor = Color.LightGray;
            OffButton.Dock = DockStyle.Top;
            OffButton.Location = new Point(0, 149);
            OffButton.Margin = new Padding(4, 3, 4, 3);
            OffButton.Name = "OffButton";
            OffButton.Size = new Size(680, 62);
            OffButton.TabIndex = 3;
            OffButton.Text = "Turn Off";
            OffButton.UseVisualStyleBackColor = false;
            OffButton.Click += OffButton_Click;
            // 
            // BrightnessBar
            // 
            BrightnessBar.Dock = DockStyle.Top;
            BrightnessBar.Location = new Point(0, 226);
            BrightnessBar.Margin = new Padding(4, 3, 4, 3);
            BrightnessBar.Maximum = 255;
            BrightnessBar.Name = "BrightnessBar";
            BrightnessBar.Size = new Size(680, 45);
            BrightnessBar.TabIndex = 5;
            BrightnessBar.ValueChanged += BrightnessBar_ValueChanged;
            BrightnessBar.MouseCaptureChanged += BrightnessBar_MouseCaptureChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(0, 211);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 4;
            label2.Text = "Brightness:";
            // 
            // RainbowButton
            // 
            RainbowButton.BackColor = Color.LightGray;
            RainbowButton.Dock = DockStyle.Top;
            RainbowButton.ForeColor = SystemColors.ControlText;
            RainbowButton.Location = new Point(0, 271);
            RainbowButton.Margin = new Padding(4, 3, 4, 3);
            RainbowButton.Name = "RainbowButton";
            RainbowButton.Size = new Size(680, 47);
            RainbowButton.TabIndex = 6;
            RainbowButton.Text = "Rainbow";
            RainbowButton.UseVisualStyleBackColor = false;
            RainbowButton.Click += RainbowButton_Click;
            // 
            // AmbilightButton
            // 
            AmbilightButton.BackColor = Color.LightGray;
            AmbilightButton.Dock = DockStyle.Top;
            AmbilightButton.ForeColor = SystemColors.ControlText;
            AmbilightButton.Location = new Point(0, 318);
            AmbilightButton.Margin = new Padding(4, 3, 4, 3);
            AmbilightButton.Name = "AmbilightButton";
            AmbilightButton.Size = new Size(680, 47);
            AmbilightButton.TabIndex = 7;
            AmbilightButton.Text = "Ambilight";
            AmbilightButton.UseVisualStyleBackColor = false;
            AmbilightButton.Click += AmbilightButton_Click;
            // 
            // SpectogramButton
            // 
            SpectogramButton.BackColor = Color.LightGray;
            SpectogramButton.Dock = DockStyle.Top;
            SpectogramButton.Location = new Point(0, 425);
            SpectogramButton.Margin = new Padding(4, 3, 4, 3);
            SpectogramButton.Name = "SpectogramButton";
            SpectogramButton.Size = new Size(680, 47);
            SpectogramButton.TabIndex = 8;
            SpectogramButton.Text = "\"Spectogram\"";
            SpectogramButton.UseVisualStyleBackColor = false;
            SpectogramButton.Click += SpectogramButton_Click;
            // 
            // PartyButton
            // 
            PartyButton.BackColor = Color.LightGray;
            PartyButton.Dock = DockStyle.Top;
            PartyButton.Location = new Point(0, 472);
            PartyButton.Margin = new Padding(4, 3, 4, 3);
            PartyButton.Name = "PartyButton";
            PartyButton.Size = new Size(680, 47);
            PartyButton.TabIndex = 9;
            PartyButton.Text = "Party";
            PartyButton.UseVisualStyleBackColor = false;
            PartyButton.Click += Party_Click;
            // 
            // ResetAudioB
            // 
            ResetAudioB.Dock = DockStyle.Top;
            ResetAudioB.Location = new Point(0, 519);
            ResetAudioB.Margin = new Padding(4, 3, 4, 3);
            ResetAudioB.Name = "ResetAudioB";
            ResetAudioB.Size = new Size(680, 48);
            ResetAudioB.TabIndex = 10;
            ResetAudioB.Text = "Reset Sound Device";
            ResetAudioB.UseVisualStyleBackColor = true;
            ResetAudioB.Click += ResetAudioB_Click;
            // 
            // ambilightInterval
            // 
            ambilightInterval.Dock = DockStyle.Top;
            ambilightInterval.Location = new Point(0, 380);
            ambilightInterval.Margin = new Padding(4, 3, 4, 3);
            ambilightInterval.Maximum = 200;
            ambilightInterval.Name = "ambilightInterval";
            ambilightInterval.Size = new Size(680, 45);
            ambilightInterval.TabIndex = 11;
            ambilightInterval.Value = 60;
            ambilightInterval.Scroll += ambilightInterval_Scroll;
            ambilightInterval.ValueChanged += ambilightInterval_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(0, 365);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(147, 15);
            label3.TabIndex = 12;
            label3.Text = "Ambilight Interval (ms): 60";
            // 
            // MainApp
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(680, 594);
            Controls.Add(ResetAudioB);
            Controls.Add(PartyButton);
            Controls.Add(SpectogramButton);
            Controls.Add(ambilightInterval);
            Controls.Add(label3);
            Controls.Add(AmbilightButton);
            Controls.Add(RainbowButton);
            Controls.Add(BrightnessBar);
            Controls.Add(label2);
            Controls.Add(OffButton);
            Controls.Add(panel1);
            Controls.Add(ColorButton);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainApp";
            Text = "Control";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)BrightnessBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)ambilightInterval).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private Label label1;
        private ColorDialog colorDialog1;
        private Button ColorButton;
        private Panel panel1;
        private Button OffButton;
        private TrackBar BrightnessBar;
        private Label label2;
        private Button RainbowButton;
        private Button AmbilightButton;

        private Button SpectogramButton;
        private Button PartyButton;

        private Button ResetAudioB;
        private TrackBar ambilightInterval;
        private Label label3;
    }
}

