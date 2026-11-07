using ControlGuiLed.Led;

namespace ControlGuiLed.Effects;

public sealed class RainbowEffect(ILedOutput output, IBrightnessProvider brightness) : ILedEffect
{
    private int _hue;

    public void Reset() => _hue = 0;

    public void Render()
    {
        output.ShowColor(ColorMath.FromHsv(_hue, 1, 1), brightness.GetCurrentBrightness());
        _hue = (_hue + 1) % 360;
    }
}
