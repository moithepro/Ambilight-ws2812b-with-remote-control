using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ControlGuiLed.Led;

namespace ControlGuiLed.Ambilight;

/// <summary>
/// Captures the primary screen through GDI. This is the original non-DirectX
/// capture path and works where DXGI desktop duplication is unavailable.
/// </summary>
public sealed class GdiAmbilightFrameProvider : IAmbilightFrameProvider, IDisposable
{
    private readonly Rectangle _screenBounds;
    private readonly Bitmap _screenBitmap;
    private readonly IReadOnlyList<Rectangle> _regions;
    private bool _disposed;

    public GdiAmbilightFrameProvider()
    {
        _screenBounds = Screen.PrimaryScreen?.Bounds
            ?? throw new InvalidOperationException("No primary screen is available for screen capture.");

        _screenBitmap = new Bitmap(
            _screenBounds.Width,
            _screenBounds.Height,
            PixelFormat.Format32bppArgb);
        _regions = ScreenRegionLayout.Create(_screenBounds.Size);
    }

    public IReadOnlyList<Rgb> CaptureFrame()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        using (var graphics = Graphics.FromImage(_screenBitmap))
        {
            graphics.CopyFromScreen(
                _screenBounds.Location,
                Point.Empty,
                _screenBitmap.Size,
                CopyPixelOperation.SourceCopy);
        }

        return AmbilightFrameSampler.Sample(_screenBitmap, _regions);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _screenBitmap.Dispose();
    }
}
