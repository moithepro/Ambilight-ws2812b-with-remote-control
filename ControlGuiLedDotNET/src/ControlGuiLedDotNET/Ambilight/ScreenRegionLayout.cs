using System.Drawing;
using ControlGuiLed.Led;

namespace ControlGuiLed.Ambilight;

/// <summary>
/// Divides the screen edges into the sampling regions used by the LED strip.
/// The strip runs up the right edge, across the top, down the left edge and
/// along the bottom edge, so the regions are returned in that order.
/// </summary>
public static class ScreenRegionLayout
{
    private const double RegionOffset = 0.8;

    public static Rectangle[] Create(Size screenSize)
    {
        var horizontalWidth = screenSize.Width / 80;
        var horizontalHeight = screenSize.Height / 29;
        var horizontalMargin = screenSize.Width / 38;
        var horizontalSpacing =
            (screenSize.Width
                - (2 * horizontalMargin)
                - (LedDeviceSettings.HorizontalEdgeRegionCount * horizontalWidth))
            / LedDeviceSettings.HorizontalEdgeRegionCount;

        var verticalWidth = screenSize.Width / 40;
        var verticalHeight = screenSize.Height / 58;
        var verticalMargin = screenSize.Height / 34;
        var verticalSpacing =
            (screenSize.Height
                - (2 * verticalMargin)
                - (LedDeviceSettings.VerticalEdgeRegionCount * verticalHeight))
            / LedDeviceSettings.VerticalEdgeRegionCount;

        var top = new Rectangle[LedDeviceSettings.HorizontalEdgeRegionCount];
        var bottom = new Rectangle[LedDeviceSettings.HorizontalEdgeRegionCount];

        for (var index = 0; index < LedDeviceSettings.HorizontalEdgeRegionCount; index++)
        {
            var x = (int)Math.Round(
                horizontalMargin + (index * (horizontalWidth + horizontalSpacing + RegionOffset)));

            top[index] = new Rectangle(x, 0, horizontalWidth, horizontalHeight);
            bottom[index] = new Rectangle(
                x,
                screenSize.Height - horizontalHeight - 1,
                horizontalWidth,
                horizontalHeight);
        }

        var left = new Rectangle[LedDeviceSettings.VerticalEdgeRegionCount];
        var right = new Rectangle[LedDeviceSettings.VerticalEdgeRegionCount];

        for (var index = 0; index < LedDeviceSettings.VerticalEdgeRegionCount; index++)
        {
            var y = (int)Math.Round(
                verticalMargin + (index * (verticalHeight + verticalSpacing + RegionOffset)));

            left[index] = new Rectangle(0, y, verticalWidth, verticalHeight);
            right[index] = new Rectangle(
                screenSize.Width - verticalWidth - 1,
                y,
                verticalWidth,
                verticalHeight);
        }

        Array.Reverse(right);
        Array.Reverse(top);

        var regions = new Rectangle[
            right.Length + top.Length + left.Length + bottom.Length];

        right.CopyTo(regions, 0);
        top.CopyTo(regions, right.Length);
        left.CopyTo(regions, right.Length + top.Length);
        bottom.CopyTo(regions, right.Length + top.Length + left.Length);

        return regions;
    }
}
