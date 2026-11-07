using System.Drawing;

namespace ControlGuiLed.Led;

/// <summary>
/// Byte layouts of the serial protocol understood by the controller firmware.
/// </summary>
public static class LedProtocol
{
    /// <summary>Commands sent from the PC to the controller.</summary>
    public static class Commands
    {
        public const byte Color = 22;
        public const byte Dynamic = 23;
    }

    /// <summary>Color frame: command, brightness, red, green, blue.</summary>
    public static byte[] EncodeColor(Color color, byte brightness) =>
        [Commands.Color, brightness, color.R, color.G, color.B];

    /// <summary>Dynamic frame: command, brightness, then red/green/blue per LED.</summary>
    public static byte[] EncodeDynamicFrame(IReadOnlyList<Rgb> colors, byte brightness)
    {
        var frame = new byte[2 + (colors.Count * 3)];
        frame[0] = Commands.Dynamic;
        frame[1] = brightness;

        for (var index = 0; index < colors.Count; index++)
        {
            var offset = 2 + (index * 3);
            frame[offset] = colors[index].Red;
            frame[offset + 1] = colors[index].Green;
            frame[offset + 2] = colors[index].Blue;
        }

        return frame;
    }
}
