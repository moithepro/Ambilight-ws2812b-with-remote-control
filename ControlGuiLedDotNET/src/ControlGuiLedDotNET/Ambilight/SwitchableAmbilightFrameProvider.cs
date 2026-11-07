using ControlGuiLed.Led;

namespace ControlGuiLed.Ambilight;

/// <summary>
/// Exposes one stable frame provider whose capture implementation can be
/// changed at runtime. The selected implementation is created lazily, so a
/// failure in one path does not prevent the application from starting.
/// </summary>
public sealed class SwitchableAmbilightFrameProvider : IAmbilightFrameProvider, IDisposable
{
    private readonly Lock _sync = new();

    private IAmbilightFrameProvider? _activeProvider;
    private AmbilightCaptureMethod _method;
    private bool _disposed;

    public SwitchableAmbilightFrameProvider(AmbilightCaptureMethod initialMethod)
    {
        _method = initialMethod;
    }

    public AmbilightCaptureMethod Method
    {
        get
        {
            lock (_sync)
            {
                return _method;
            }
        }
    }

    public void SelectMethod(AmbilightCaptureMethod method)
    {
        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_method == method)
            {
                return;
            }

            _method = method;
            DisposeActiveProvider();
        }
    }

    public IReadOnlyList<Rgb> CaptureFrame()
    {
        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            _activeProvider ??= CreateProvider();
            return _activeProvider.CaptureFrame();
        }
    }

    public void Dispose()
    {
        lock (_sync)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            DisposeActiveProvider();
        }
    }

    private IAmbilightFrameProvider CreateProvider() => _method switch
    {
        AmbilightCaptureMethod.DirectX => new DxAmbilightFrameProvider(),
        AmbilightCaptureMethod.Gdi => new GdiAmbilightFrameProvider(),
        _ => throw new ArgumentOutOfRangeException(
            nameof(_method),
            _method,
            "Unknown Ambilight capture method."),
    };

    private void DisposeActiveProvider()
    {
        if (_activeProvider is IDisposable disposableProvider)
        {
            disposableProvider.Dispose();
        }

        _activeProvider = null;
    }
}
