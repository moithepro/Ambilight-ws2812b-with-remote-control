using System.Drawing;

namespace ControlGuiLed.Led;

public static class ColorMath
{
    public static Color FromHsv(double hue, double saturation, double value)
    {
        hue = ((hue % 360) + 360) % 360;

        var chroma = value * saturation;
        var secondary = chroma * (1 - Math.Abs(((hue / 60) % 2) - 1));
        var match = value - chroma;

        var (red, green, blue) = hue switch
        {
            < 60 => (chroma, secondary, 0d),
            < 120 => (secondary, chroma, 0d),
            < 180 => (0d, chroma, secondary),
            < 240 => (0d, secondary, chroma),
            < 300 => (secondary, 0d, chroma),
            _ => (chroma, 0d, secondary),
        };

        return Color.FromArgb(
            255,
            ToByte(red + match),
            ToByte(green + match),
            ToByte(blue + match));
    }

    private static int ToByte(double value) =>
        Math.Clamp((int)Math.Round(value * 255), 0, 255);
}
