using System.Drawing;
using ControlGuiLed.Ambilight;
using ControlGuiLed.Effects;
using ControlGuiLed.Input;
using ControlGuiLed.Led;
using ControlGuiLed.Serial;
using ControlGuiLed.Timing;

namespace ControlGuiLed.App;

/// <summary>
/// Application-level use cases: selecting an effect, changing brightness and
/// sound reactivity, and coordinating serial input with the LED hardware.
/// </summary>
public sealed class LedApplicationController : IDisposable
{
    private const int ColorEffectIntervalMs = 40;
    private const int AnimationEffectIntervalMs = 60;
    private const int DefaultAmbilightIntervalMs = 60;
    private const int MinimumAmbilightIntervalMs = 1;
    private const int MaximumAmbilightIntervalMs = 1000;

    private readonly SerialConnection _serialConnection;
    private readonly EffectScheduler _scheduler;
    private readonly LedBrightnessProvider _brightnessProvider;
    private readonly RemoteKeyboardController _remoteKeyboard;
    private readonly ColorEffect _colorEffect;
    private readonly RainbowEffect _rainbowEffect;
    private readonly PartyEffect _partyEffect;
    private readonly AmbilightEffect _ambilightEffect;
    private readonly SwitchableAmbilightFrameProvider _ambilightFrameProvider;
    private readonly ILedOutput _output;

    private LedMode _mode = LedMode.Off;
    private int _ambilightIntervalMs = DefaultAmbilightIntervalMs;
    private bool _started;
    private bool _disposed;

    public LedApplicationController(
        SerialConnection serialConnection,
        EffectScheduler scheduler,
        LedBrightnessProvider brightnessProvider,
        RemoteKeyboardController remoteKeyboard,
        ILedOutput output,
        ColorEffect colorEffect,
        RainbowEffect rainbowEffect,
        PartyEffect partyEffect,
        AmbilightEffect ambilightEffect,
        SwitchableAmbilightFrameProvider ambilightFrameProvider)
    {
        _serialConnection = serialConnection;
        _scheduler = scheduler;
        _brightnessProvider = brightnessProvider;
        _remoteKeyboard = remoteKeyboard;
        _output = output;
        _colorEffect = colorEffect;
        _rainbowEffect = rainbowEffect;
        _partyEffect = partyEffect;
        _ambilightEffect = ambilightEffect;
        _ambilightFrameProvider = ambilightFrameProvider;

        _serialConnection.ConnectionChanged += OnSerialConnectionChanged;
        _serialConnection.RemoteKeyReceived += OnRemoteKeyReceived;
    }

    public event EventHandler<bool>? ConnectionChanged;

    public event EventHandler<LedMode>? ModeChanged;

    public event EventHandler<bool>? SoundReactiveChanged;

    public bool IsConnected => _serialConnection.IsConnected;

    public string SerialPortName => _serialConnection.PortName;

    public IReadOnlyList<string> GetAvailableSerialPorts() => SerialPortCatalog.GetAvailablePorts();

    public void SelectSerialPort(string portName)
    {
        if (_disposed)
        {
            return;
        }

        _serialConnection.ChangePort(portName);
    }

    public LedMode Mode => _mode;

    public AmbilightCaptureMethod SelectedAmbilightCaptureMethod =>
        _ambilightFrameProvider.Method;

    public void SelectAmbilightCaptureMethod(AmbilightCaptureMethod method)
    {
        if (_disposed)
        {
            return;
        }

        _ambilightFrameProvider.SelectMethod(method);
    }

    public byte Brightness
    {
        get => _brightnessProvider.BaseBrightness;
        set => _brightnessProvider.BaseBrightness = value;
    }

    public bool SoundReactive
    {
        get => _brightnessProvider.SoundReactive;
        set
        {
            if (_brightnessProvider.SoundReactive == value)
            {
                return;
            }

            _brightnessProvider.SoundReactive = value;
            SoundReactiveChanged?.Invoke(this, value);
        }
    }

    public int AmbilightIntervalMs
    {
        get => _ambilightIntervalMs;
        set
        {
            var interval = Math.Clamp(
                value,
                MinimumAmbilightIntervalMs,
                MaximumAmbilightIntervalMs);

            if (_ambilightIntervalMs == interval || _disposed)
            {
                return;
            }

            _ambilightIntervalMs = interval;

            if (_mode == LedMode.Ambilight)
            {
                _scheduler.Start(_ambilightEffect, interval);
            }
        }
    }

    public void Start()
    {
        if (_started || _disposed)
        {
            return;
        }

        _started = true;
        _serialConnection.Start();
    }

    public void SelectColor(Color color)
    {
        _colorEffect.SelectedColor = color;
        SetMode(LedMode.Color);
    }

    public void SetMode(LedMode mode)
    {
        if (_disposed)
        {
            return;
        }

        if (mode == LedMode.Off)
        {
            TurnOff();
            return;
        }

        (ILedEffect Effect, int IntervalMs) selection = mode switch
        {
            LedMode.Color => (_colorEffect, ColorEffectIntervalMs),
            LedMode.Rainbow => (_rainbowEffect, AnimationEffectIntervalMs),
            LedMode.Party => (_partyEffect, AnimationEffectIntervalMs),
            LedMode.Ambilight => (_ambilightEffect, _ambilightIntervalMs),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

        _scheduler.Start(selection.Effect, selection.IntervalMs);
        _mode = mode;
        ModeChanged?.Invoke(this, mode);
    }

    public void TurnOff()
    {
        if (_disposed)
        {
            return;
        }

        _scheduler.Stop();
        _mode = LedMode.Off;
        _output.TurnOff();
        ModeChanged?.Invoke(this, LedMode.Off);
    }

    public void ResetAudioDevice() => _brightnessProvider.ResetAudioDevice();

    public void Shutdown()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _serialConnection.ConnectionChanged -= OnSerialConnectionChanged;
        _serialConnection.RemoteKeyReceived -= OnRemoteKeyReceived;

        _scheduler.Stop();
        _output.TurnOff();

        _serialConnection.Dispose();
        _scheduler.Dispose();
        _brightnessProvider.Dispose();
        _ambilightFrameProvider.Dispose();
    }

    public void Dispose() => Shutdown();

    private void OnSerialConnectionChanged(object? sender, bool connected) =>
        ConnectionChanged?.Invoke(this, connected);

    private void OnRemoteKeyReceived(object? sender, RemoteKey key) =>
        _remoteKeyboard.Handle(key);
}
