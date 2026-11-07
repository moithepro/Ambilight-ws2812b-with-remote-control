using System.Drawing;
using ControlGuiLed.Serial;

namespace ControlGuiLed.Led;

public sealed class LedStripOutput(SerialConnection serialConnection) : ILedOutput
{
    public void ShowColor(Color color, byte brightness) =>
        serialConnection.Write(LedProtocol.EncodeColor(color, brightness));

    public void ShowDynamicFrame(IReadOnlyList<Rgb> colors, byte brightness)
    {
        if (colors.Count != LedDeviceSettings.LedCount)
        {
            throw new ArgumentException(
                $"A dynamic frame must contain exactly {LedDeviceSettings.LedCount} colors.",
                nameof(colors));
        }

        serialConnection.Write(LedProtocol.EncodeDynamicFrame(colors, brightness));
    }

    public void TurnOff() => ShowColor(Color.Black, 0);
}
