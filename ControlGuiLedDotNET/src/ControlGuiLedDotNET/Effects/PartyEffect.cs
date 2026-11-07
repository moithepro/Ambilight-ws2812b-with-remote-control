using ControlGuiLed.Led;

namespace ControlGuiLed.Effects;

public sealed class PartyEffect(ILedOutput output, IBrightnessProvider brightness) : ILedEffect
{
    private int _hue;

    public void Reset() => _hue = 0;

    public void Render()
    {
        var currentBrightness = brightness.GetCurrentBrightness();

        if (brightness.SoundReactive)
        {
            var colorHue = (_hue + currentBrightness) % 360;
            output.ShowColor(ColorMath.FromHsv(colorHue, 1, 1), currentBrightness);
            _hue = (_hue + 3) % 360;
        }
        else
        {
            output.ShowColor(ColorMath.FromHsv(_hue, 1, 1), currentBrightness);
            _hue = (_hue + 37) % 360;
        }
    }
}
