using System.Drawing;
using ControlGuiLed.Led;
using HPPH;

namespace ControlGuiLed.Ambilight;

internal static class AmbilightFrameSampler
{
    private const int SampleStep = 8;

    public static Rgb[] Sample(IImage image, IReadOnlyList<Rectangle> regions)
    {
        var colors = new Rgb[regions.Count];

        for (var regionIndex = 0; regionIndex < regions.Count; regionIndex++)
        {
            colors[regionIndex] = SampleRegion(image, regions[regionIndex]);
        }

        return colors;
    }

    private static Rgb SampleRegion(IImage image, Rectangle region)
    {
        var left = Math.Max(region.Left, 0);
        var top = Math.Max(region.Top, 0);
        var right = Math.Min(region.Right, image.Width);
        var bottom = Math.Min(region.Bottom, image.Height);

        long red = 0;
        long green = 0;
        long blue = 0;
        var sampleCount = 0;

        for (var y = top; y < bottom; y += SampleStep)
        {
            for (var x = left; x < right; x += SampleStep)
            {
                var pixel = image[x, y];
                red += pixel.R;
                green += pixel.G;
                blue += pixel.B;
                sampleCount++;
            }
        }

        if (sampleCount == 0)
        {
            return default;
        }

        return new Rgb(
            (byte)(red / sampleCount),
            (byte)(green / sampleCount),
            (byte)(blue / sampleCount));
    }
}
