using System.Drawing;

namespace ControlGuiLed.Led;

public interface ILedOutput
{
    void ShowColor(Color color, byte brightness);

    void ShowDynamicFrame(IReadOnlyList<Rgb> colors, byte brightness);

    void TurnOff();
}
