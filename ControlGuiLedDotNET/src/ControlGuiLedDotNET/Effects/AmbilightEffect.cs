using ControlGuiLed.Ambilight;
using ControlGuiLed.Led;

namespace ControlGuiLed.Effects;

public sealed class AmbilightEffect(
    IAmbilightFrameProvider frameProvider,
    ILedOutput output,
    IBrightnessProvider brightness) : ILedEffect
{
    public void Reset()
    {
        // The effect is stateless; every frame is derived from the screen.
    }

    public void Render()
    {
        var frame = frameProvider.CaptureFrame();
        output.ShowDynamicFrame(frame, brightness.GetCurrentBrightness());
    }
}
