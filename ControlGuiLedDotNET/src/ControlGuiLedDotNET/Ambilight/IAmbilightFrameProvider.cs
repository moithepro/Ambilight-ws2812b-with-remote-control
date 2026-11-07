using ControlGuiLed.Led;

namespace ControlGuiLed.Ambilight;

public interface IAmbilightFrameProvider
{
    IReadOnlyList<Rgb> CaptureFrame();
}
