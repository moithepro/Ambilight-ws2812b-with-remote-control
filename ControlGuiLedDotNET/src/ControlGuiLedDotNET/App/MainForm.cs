using System.Drawing;
using ControlGuiLed.App;

namespace ControlGuiLed;

public sealed partial class MainForm : Form
{
    private readonly LedApplicationController _controller;
    private bool _exitRequested;

    public MainForm(LedApplicationController controller)
    {
        _controller = controller;
        InitializeComponent();

        var appIcon = LoadAppIcon();
        Icon = appIcon;
        notifyIcon.Icon = appIcon;
        notifyIcon.Visible = true;

        brightnessTrackBar.Value = Math.Clamp(
            _controller.Brightness,
            brightnessTrackBar.Minimum,
            brightnessTrackBar.Maximum);
        ambilightIntervalTrackBar.Value = Math.Clamp(
            _controller.AmbilightIntervalMs,
            ambilightIntervalTrackBar.Minimum,
            ambilightIntervalTrackBar.Maximum);

        UpdateBrightnessLabel();
        UpdateAmbilightIntervalLabel();
        UpdateConnectionState(_controller.IsConnected);
        UpdateModeButtons(_controller.Mode);
        UpdateSoundReactiveButton();

        _controller.ConnectionChanged += OnConnectionChanged;
        _controller.ModeChanged += OnModeChanged;
        _controller.SoundReactiveChanged += OnSoundReactiveChanged;
    }

    protected override void SetVisibleCore(bool value)
    {
        if (!IsHandleCreated)
        {
            CreateHandle();
            value = false;
        }

        base.SetVisibleCore(value);
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        _controller.Start();
    }

    private static Icon LoadAppIcon()
    {
        using var stream = typeof(MainForm).Assembly.GetManifestResourceStream("ControlGuiLed.Icon.ico");
        return stream is not null ? new Icon(stream) : SystemIcons.Application;
    }

    private void OnConnectionChanged(object? sender, bool connected)
    {
        if (IsDisposed || Disposing || !IsHandleCreated)
        {
            return;
        }

        if (InvokeRequired)
        {
            try
            {
                BeginInvoke(new Action(() => UpdateConnectionState(connected)));
            }
            catch (InvalidOperationException)
            {
                // The form is closing.
            }

            return;
        }

        UpdateConnectionState(connected);
    }

    private void OnModeChanged(object? sender, LedMode mode) => UpdateModeButtons(mode);

    private void OnSoundReactiveChanged(object? sender, bool soundReactive) =>
        UpdateSoundReactiveButton();

    private void UpdateConnectionState(bool connected)
    {
        var state = connected ? "Connected" : "Disconnected";

        statusLabel.Text = state;
        Text = $"ControlGuiLed - {state}";
        notifyIcon.Text = $"ControlGuiLed ({state})";
    }

    private void UpdateModeButtons(LedMode mode)
    {
        SetButtonActive(colorButton, mode == LedMode.Color);
        SetButtonActive(rainbowButton, mode == LedMode.Rainbow);
        SetButtonActive(partyButton, mode == LedMode.Party);
        SetButtonActive(ambilightButton, mode == LedMode.Ambilight);
        SetButtonActive(offButton, mode == LedMode.Off);
    }

    private void UpdateSoundReactiveButton()
    {
        soundReactiveButton.BackColor = _controller.SoundReactive ? Color.Gray : Color.LightGray;
    }

    private void UpdateBrightnessLabel()
    {
        brightnessLabel.Text = $"Brightness: {brightnessTrackBar.Value}";
    }

    private void UpdateAmbilightIntervalLabel()
    {
        ambilightIntervalLabel.Text = $"Ambilight interval (ms): {ambilightIntervalTrackBar.Value}";
    }

    private static void SetButtonActive(Button button, bool active)
    {
        button.BackColor = active ? Color.Gray : Color.LightGray;
    }

    private void colorButton_Click(object? sender, EventArgs e)
    {
        colorDialog.Color = colorPreviewPanel.BackColor;

        if (colorDialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        colorPreviewPanel.BackColor = colorDialog.Color;
        _controller.SelectColor(colorDialog.Color);
    }

    private void offButton_Click(object? sender, EventArgs e)
    {
        colorPreviewPanel.BackColor = Color.Black;
        _controller.TurnOff();
    }

    private void rainbowButton_Click(object? sender, EventArgs e) =>
        _controller.SetMode(LedMode.Rainbow);

    private void ambilightButton_Click(object? sender, EventArgs e) =>
        _controller.SetMode(LedMode.Ambilight);

    private void partyButton_Click(object? sender, EventArgs e) =>
        _controller.SetMode(LedMode.Party);

    private void brightnessTrackBar_ValueChanged(object? sender, EventArgs e)
    {
        _controller.Brightness = (byte)brightnessTrackBar.Value;
        UpdateBrightnessLabel();
    }

    private void ambilightIntervalTrackBar_ValueChanged(object? sender, EventArgs e)
    {
        _controller.AmbilightIntervalMs = ambilightIntervalTrackBar.Value;
        UpdateAmbilightIntervalLabel();
    }

    private void soundReactiveButton_Click(object? sender, EventArgs e) =>
        _controller.SoundReactive = !_controller.SoundReactive;

    private void resetAudioButton_Click(object? sender, EventArgs e) =>
        _controller.ResetAudioDevice();

    private void notifyIcon_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        if (Visible)
        {
            Hide();
        }
        else
        {
            Show();
            Activate();
        }
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing && !_exitRequested)
        {
            e.Cancel = true;
            Hide();
        }
    }

    private void exitMenuItem_Click(object? sender, EventArgs e)
    {
        _exitRequested = true;
        _controller.Shutdown();
        notifyIcon.Visible = false;
        Application.Exit();
    }
}
