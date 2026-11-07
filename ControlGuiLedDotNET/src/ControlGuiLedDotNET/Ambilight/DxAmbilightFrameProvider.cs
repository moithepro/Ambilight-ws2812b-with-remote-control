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
        var captureService = new DX11ScreenCaptureService();
        IScreenCapture? screenCapture = null;

        try
        {
            var graphicsCards = captureService.GetGraphicsCards().ToArray();
            if (graphicsCards.Length == 0)
            {
                throw new InvalidOperationException("No graphics card is available for screen capture.");
            }

            var displays = captureService.GetDisplays(graphicsCards[0]).ToArray();
            if (displays.Length == 0)
            {
                throw new InvalidOperationException("No display is available for screen capture.");
            }

            var display = displays[0];

            screenCapture = ((IScreenCaptureService)captureService).GetScreenCapture(display);

            // The sampling regions use screen coordinates, so the captured
            // image must stay at full resolution. Any downscaling would make
            // the region coordinates refer to the wrong pixels.
            var captureZone = screenCapture.RegisterCaptureZone(
                0,
                0,
                display.Width,
                display.Height,
                downscaleLevel: 0);
            _regions = ScreenRegionLayout.Create(new Size(display.Width, display.Height));

            _captureService = captureService;
            _screenCapture = screenCapture;
            _captureZone = captureZone;
        }
        catch
        {
            if (screenCapture is IDisposable disposableCapture)
            {
                disposableCapture.Dispose();
            }

            captureService.Dispose();
            throw;
        }
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
