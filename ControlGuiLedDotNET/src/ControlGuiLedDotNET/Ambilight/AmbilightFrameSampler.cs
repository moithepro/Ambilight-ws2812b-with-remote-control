using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ControlGuiLed.Led;
using HPPH;

namespace ControlGuiLed.Ambilight;

internal static class AmbilightFrameSampler
{
    private const int SampleStep = 8;

    public static Rgb[] Sample(IImage image, IReadOnlyList<Rectangle> regions) =>
        Sample(
            regions,
            image.Width,
            image.Height,
            (x, y) =>
            {
                var pixel = image[x, y];
                return new Rgb(pixel.R, pixel.G, pixel.B);
            });

    public static Rgb[] Sample(Bitmap bitmap, IReadOnlyList<Rectangle> regions)
    {
        var bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var data = bitmap.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        try
        {
            return Sample(
                regions,
                data.Width,
                data.Height,
                (x, y) =>
                {
                    var pixel = Marshal.ReadInt32(data.Scan0, (y * data.Stride) + (x * 4));
                    return new Rgb(
                        (byte)((pixel >> 16) & 0xFF),
                        (byte)((pixel >> 8) & 0xFF),
                        (byte)(pixel & 0xFF));
                });
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
    }

    private static Rgb[] Sample(
        IReadOnlyList<Rectangle> regions,
        int imageWidth,
        int imageHeight,
        Func<int, int, Rgb> readPixel)
    {
        var colors = new Rgb[regions.Count];

        for (var regionIndex = 0; regionIndex < regions.Count; regionIndex++)
        {
            colors[regionIndex] = SampleRegion(
                regions[regionIndex],
                imageWidth,
                imageHeight,
                readPixel);
        }

        return colors;
    }

    private static Rgb SampleRegion(
        Rectangle region,
        int imageWidth,
        int imageHeight,
        Func<int, int, Rgb> readPixel)
    {
        var left = Math.Max(region.Left, 0);
        var top = Math.Max(region.Top, 0);
        var right = Math.Min(region.Right, imageWidth);
        var bottom = Math.Min(region.Bottom, imageHeight);

        long red = 0;
        long green = 0;
        long blue = 0;
        var sampleCount = 0;

        for (var y = top; y < bottom; y += SampleStep)
        {
            for (var x = left; x < right; x += SampleStep)
            {
                var pixel = readPixel(x, y);
                red += pixel.Red;
                green += pixel.Green;
                blue += pixel.Blue;
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
