namespace ControlGuiLed.Effects;

public interface ILedEffect
{
    /// <summary>Resets any animation state before the effect starts.</summary>
    void Reset();

    /// <summary>Renders one frame.</summary>
    void Render();
}
