using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;

namespace ControlGuiLed.Audio;

/// <summary>
/// Reads the peak level of the default playback device.
/// The COM device is created lazily on the calling thread, which in this
/// application is the LED effect thread, and reset on demand.
/// </summary>
public sealed class WasapiAudioLevelSource : IAudioLevelSource
{
    private readonly Lock _sync = new();
    private MMDevice? _device;
    private bool _deviceUnavailable;
    private bool _disposed;

    public double GetPeakLevel()
    {
        lock (_sync)
        {
            if (_disposed || _deviceUnavailable)
            {
                return 0;
            }

            if (_device is null && !TryOpenDefaultDevice())
            {
                return 0;
            }

            try
            {
                return _device?.AudioMeterInformation.MasterPeakValue ?? 0;
            }
            catch (COMException)
            {
                CloseDevice();
                _deviceUnavailable = true;
                return 0;
            }
        }
    }

    public void Reset()
    {
        lock (_sync)
        {
            CloseDevice();
            _deviceUnavailable = false;
        }
    }

    public void Dispose()
    {
        lock (_sync)
        {
            _disposed = true;
            CloseDevice();
        }
    }

    private bool TryOpenDefaultDevice()
    {
        try
        {
            using var enumerator = new MMDeviceEnumerator();
            _device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            return true;
        }
        catch (COMException)
        {
            _deviceUnavailable = true;
            return false;
        }
    }

    private void CloseDevice()
    {
        _device?.Dispose();
        _device = null;
    }
}
