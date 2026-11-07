using System.Drawing;
using ControlGuiLed.Led;
using ScreenCapture.NET;

namespace ControlGuiLed.Ambilight;

public sealed class DxAmbilightFrameProvider : IAmbilightFrameProvider, IDisposable
{
    private readonly DX11ScreenCaptureService _captureService;
    private readonly IScreenCapture _screenCapture;
    private readonly ICaptureZone _captureZone;
    private readonly IReadOnlyList<Rectangle> _regions;

    public DxAmbilightFrameProvider()
    {
        _captureService = new DX11ScreenCaptureService();

        var graphicsCards = _captureService.GetGraphicsCards().ToArray();
        if (graphicsCards.Length == 0)
        {
            throw new InvalidOperationException("No graphics card is available for screen capture.");
        }

        var displays = _captureService.GetDisplays(graphicsCards[0]).ToArray();
        if (displays.Length == 0)
        {
            throw new InvalidOperationException("No display is available for screen capture.");
        }

        var display = displays[0];

        _screenCapture = ((IScreenCaptureService)_captureService).GetScreenCapture(display);

        // The sampling regions use screen coordinates, so the captured image
        // must stay at full resolution. Any downscaling would make the region
        // coordinates refer to the wrong pixels.
        _captureZone = _screenCapture.RegisterCaptureZone(
            0,
            0,
            display.Width,
            display.Height,
            downscaleLevel: 0);
        _regions = ScreenRegionLayout.Create(new Size(display.Width, display.Height));
    }

    public IReadOnlyList<Rgb> CaptureFrame()
    {
        _screenCapture.CaptureScreen();

        using (_captureZone.Lock())
        {
            return AmbilightFrameSampler.Sample(_captureZone.Image, _regions);
        }
    }

    public void Dispose()
    {
        _screenCapture.UnregisterCaptureZone(_captureZone);

        if (_screenCapture is IDisposable disposableCapture)
        {
            disposableCapture.Dispose();
        }

        _captureService.Dispose();
    }
}
