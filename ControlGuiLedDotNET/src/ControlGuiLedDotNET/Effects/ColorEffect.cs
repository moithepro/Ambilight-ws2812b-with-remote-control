using System.Drawing;
using ControlGuiLed.Led;

namespace ControlGuiLed.Effects;

public sealed class ColorEffect(ILedOutput output, IBrightnessProvider brightness) : ILedEffect
{
    private Color? _lastSentColor;
    private byte _lastSentBrightness;

    public Color SelectedColor { get; set; } = Color.Black;

    public void Reset() => _lastSentColor = null;

    public void Render()
    {
        var currentBrightness = brightness.GetCurrentBrightness();

        if (_lastSentColor == SelectedColor && _lastSentBrightness == currentBrightness)
        {
            return;
        }

        output.ShowColor(SelectedColor, currentBrightness);
        _lastSentColor = SelectedColor;
        _lastSentBrightness = currentBrightness;
    }
}
