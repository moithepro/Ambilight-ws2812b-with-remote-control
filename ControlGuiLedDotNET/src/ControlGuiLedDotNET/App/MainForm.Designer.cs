#nullable disable

namespace ControlGuiLed;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        rootLayout = new TableLayoutPanel();
        statusLabel = new Label();
        serialPortLabel = new Label();
        serialPortComboBox = new ComboBox();
        colorButton = new Button();
        colorPreviewPanel = new Panel();
        offButton = new Button();
        brightnessLabel = new Label();
        brightnessTrackBar = new TrackBar();
        rainbowButton = new Button();
        ambilightButton = new Button();
        ambilightIntervalLabel = new Label();
        ambilightIntervalTrackBar = new TrackBar();
        soundReactiveButton = new Button();
        partyButton = new Button();
        resetAudioButton = new Button();
        notifyIcon = new NotifyIcon(components);
        trayMenu = new ContextMenuStrip(components);
        exitMenuItem = new ToolStripMenuItem();
        colorDialog = new ColorDialog();
        components.Add(colorDialog);

        rootLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)brightnessTrackBar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)ambilightIntervalTrackBar).BeginInit();
        SuspendLayout();

        // rootLayout
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(statusLabel, 0, 0);
        rootLayout.Controls.Add(serialPortLabel, 0, 1);
        rootLayout.Controls.Add(serialPortComboBox, 0, 2);
        rootLayout.Controls.Add(colorButton, 0, 3);
        rootLayout.Controls.Add(colorPreviewPanel, 0, 4);
        rootLayout.Controls.Add(offButton, 0, 5);
        rootLayout.Controls.Add(brightnessLabel, 0, 6);
        rootLayout.Controls.Add(brightnessTrackBar, 0, 7);
        rootLayout.Controls.Add(rainbowButton, 0, 8);
        rootLayout.Controls.Add(ambilightButton, 0, 9);
        rootLayout.Controls.Add(ambilightIntervalLabel, 0, 10);
        rootLayout.Controls.Add(ambilightIntervalTrackBar, 0, 11);
        rootLayout.Controls.Add(soundReactiveButton, 0, 12);
        rootLayout.Controls.Add(partyButton, 0, 13);
        rootLayout.Controls.Add(resetAudioButton, 0, 14);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(8);
        rootLayout.RowCount = 15;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        rootLayout.Size = new Size(680, 650);
        rootLayout.TabIndex = 0;

        // statusLabel
        statusLabel.Anchor = AnchorStyles.Left;
        statusLabel.AutoSize = true;
        statusLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
        statusLabel.Margin = new Padding(4, 0, 4, 8);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(116, 25);
        statusLabel.TabIndex = 0;
        statusLabel.Text = "Disconnected";

        // serialPortLabel
        serialPortLabel.Anchor = AnchorStyles.Left;
        serialPortLabel.AutoSize = true;
        serialPortLabel.Margin = new Padding(4, 8, 4, 0);
        serialPortLabel.Name = "serialPortLabel";
        serialPortLabel.Size = new Size(92, 15);
        serialPortLabel.TabIndex = 1;
        serialPortLabel.Text = "Serial port: COM3";

        // serialPortComboBox
        serialPortComboBox.Dock = DockStyle.Fill;
        serialPortComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        serialPortComboBox.FormattingEnabled = true;
        serialPortComboBox.Margin = new Padding(4, 3, 4, 3);
        serialPortComboBox.Name = "serialPortComboBox";
        serialPortComboBox.Size = new Size(664, 23);
        serialPortComboBox.TabIndex = 2;
        serialPortComboBox.DropDown += serialPortComboBox_DropDown;
        serialPortComboBox.SelectedIndexChanged += serialPortComboBox_SelectedIndexChanged;

        // colorButton
        colorButton.BackColor = Color.LightGray;
        colorButton.Dock = DockStyle.Fill;
        colorButton.Margin = new Padding(4, 3, 4, 3);
        colorButton.Name = "colorButton";
        colorButton.TabIndex = 3;
        colorButton.Text = "Choose Color";
        colorButton.UseVisualStyleBackColor = false;
        colorButton.Click += colorButton_Click;

        // colorPreviewPanel
        colorPreviewPanel.BackColor = Color.Black;
        colorPreviewPanel.BorderStyle = BorderStyle.FixedSingle;
        colorPreviewPanel.Dock = DockStyle.Fill;
        colorPreviewPanel.Margin = new Padding(4, 3, 4, 3);
        colorPreviewPanel.Name = "colorPreviewPanel";
        colorPreviewPanel.TabIndex = 4;

        // offButton
        offButton.BackColor = Color.LightGray;
        offButton.Dock = DockStyle.Fill;
        offButton.Margin = new Padding(4, 3, 4, 3);
        offButton.Name = "offButton";
        offButton.TabIndex = 5;
        offButton.Text = "Turn Off";
        offButton.UseVisualStyleBackColor = false;
        offButton.Click += offButton_Click;

        // brightnessLabel
        brightnessLabel.Anchor = AnchorStyles.Left;
        brightnessLabel.AutoSize = true;
        brightnessLabel.Margin = new Padding(4, 6, 4, 0);
        brightnessLabel.Name = "brightnessLabel";
        brightnessLabel.Size = new Size(86, 15);
        brightnessLabel.TabIndex = 6;
        brightnessLabel.Text = "Brightness: 200";

        // brightnessTrackBar
        brightnessTrackBar.Dock = DockStyle.Fill;
        brightnessTrackBar.LargeChange = 10;
        brightnessTrackBar.Margin = new Padding(4, 3, 4, 3);
        brightnessTrackBar.Maximum = 255;
        brightnessTrackBar.Name = "brightnessTrackBar";
        brightnessTrackBar.Size = new Size(664, 39);
        brightnessTrackBar.TabIndex = 7;
        brightnessTrackBar.TickFrequency = 25;
        brightnessTrackBar.ValueChanged += brightnessTrackBar_ValueChanged;

        // rainbowButton
        rainbowButton.BackColor = Color.LightGray;
        rainbowButton.Dock = DockStyle.Fill;
        rainbowButton.Margin = new Padding(4, 3, 4, 3);
        rainbowButton.Name = "rainbowButton";
        rainbowButton.TabIndex = 8;
        rainbowButton.Text = "Rainbow";
        rainbowButton.UseVisualStyleBackColor = false;
        rainbowButton.Click += rainbowButton_Click;

        // ambilightButton
        ambilightButton.BackColor = Color.LightGray;
        ambilightButton.Dock = DockStyle.Fill;
        ambilightButton.Margin = new Padding(4, 3, 4, 3);
        ambilightButton.Name = "ambilightButton";
        ambilightButton.TabIndex = 9;
        ambilightButton.Text = "Ambilight";
        ambilightButton.UseVisualStyleBackColor = false;
        ambilightButton.Click += ambilightButton_Click;

        // ambilightIntervalLabel
        ambilightIntervalLabel.Anchor = AnchorStyles.Left;
        ambilightIntervalLabel.AutoSize = true;
        ambilightIntervalLabel.Margin = new Padding(4, 6, 4, 0);
        ambilightIntervalLabel.Name = "ambilightIntervalLabel";
        ambilightIntervalLabel.Size = new Size(176, 15);
        ambilightIntervalLabel.TabIndex = 10;
        ambilightIntervalLabel.Text = "Ambilight interval (ms): 60";

        // ambilightIntervalTrackBar
        ambilightIntervalTrackBar.Dock = DockStyle.Fill;
        ambilightIntervalTrackBar.LargeChange = 10;
        ambilightIntervalTrackBar.Margin = new Padding(4, 3, 4, 3);
        ambilightIntervalTrackBar.Maximum = 200;
        ambilightIntervalTrackBar.Minimum = 10;
        ambilightIntervalTrackBar.Name = "ambilightIntervalTrackBar";
        ambilightIntervalTrackBar.Size = new Size(664, 39);
        ambilightIntervalTrackBar.TabIndex = 11;
        ambilightIntervalTrackBar.TickFrequency = 10;
        ambilightIntervalTrackBar.ValueChanged += ambilightIntervalTrackBar_ValueChanged;

        // soundReactiveButton
        soundReactiveButton.BackColor = Color.LightGray;
        soundReactiveButton.Dock = DockStyle.Fill;
        soundReactiveButton.Margin = new Padding(4, 3, 4, 3);
        soundReactiveButton.Name = "soundReactiveButton";
        soundReactiveButton.TabIndex = 12;
        soundReactiveButton.Text = "Sound Reactive (Spectrogram)";
        soundReactiveButton.UseVisualStyleBackColor = false;
        soundReactiveButton.Click += soundReactiveButton_Click;

        // partyButton
        partyButton.BackColor = Color.LightGray;
        partyButton.Dock = DockStyle.Fill;
        partyButton.Margin = new Padding(4, 3, 4, 3);
        partyButton.Name = "partyButton";
        partyButton.TabIndex = 13;
        partyButton.Text = "Party";
        partyButton.UseVisualStyleBackColor = false;
        partyButton.Click += partyButton_Click;

        // resetAudioButton
        resetAudioButton.Dock = DockStyle.Fill;
        resetAudioButton.Margin = new Padding(4, 3, 4, 3);
        resetAudioButton.Name = "resetAudioButton";
        resetAudioButton.TabIndex = 14;
        resetAudioButton.Text = "Reset Sound Device";
        resetAudioButton.UseVisualStyleBackColor = true;
        resetAudioButton.Click += resetAudioButton_Click;

        // trayMenu
        trayMenu.Items.AddRange(new ToolStripItem[] { exitMenuItem });
        trayMenu.Name = "trayMenu";
        trayMenu.Size = new Size(93, 26);

        // exitMenuItem
        exitMenuItem.Name = "exitMenuItem";
        exitMenuItem.Size = new Size(92, 22);
        exitMenuItem.Text = "Exit";
        exitMenuItem.Click += exitMenuItem_Click;

        // notifyIcon
        notifyIcon.ContextMenuStrip = trayMenu;
        notifyIcon.Text = "ControlGuiLed";
        notifyIcon.MouseClick += notifyIcon_MouseClick;

        // colorDialog
        colorDialog.Color = Color.Black;

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(680, 650);
        Controls.Add(rootLayout);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ControlGuiLed - Disconnected";
        FormClosing += MainForm_FormClosing;

        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)brightnessTrackBar).EndInit();
        ((System.ComponentModel.ISupportInitialize)ambilightIntervalTrackBar).EndInit();
        ResumeLayout(false);
    }

    private TableLayoutPanel rootLayout;
    private Label statusLabel;
    private Label serialPortLabel;
    private ComboBox serialPortComboBox;
    private Button colorButton;
    private Panel colorPreviewPanel;
    private Button offButton;
    private Label brightnessLabel;
    private TrackBar brightnessTrackBar;
    private Button rainbowButton;
    private Button ambilightButton;
    private Label ambilightIntervalLabel;
    private TrackBar ambilightIntervalTrackBar;
    private Button soundReactiveButton;
    private Button partyButton;
    private Button resetAudioButton;
    private NotifyIcon notifyIcon;
    private ContextMenuStrip trayMenu;
    private ToolStripMenuItem exitMenuItem;
    private ColorDialog colorDialog;
}
