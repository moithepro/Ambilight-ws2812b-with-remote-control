namespace ControlGuiLed.Audio;

public interface IAudioLevelSource : IDisposable
{
    /// <summary>Returns the current peak level in the range 0..1.</summary>
    double GetPeakLevel();

    /// <summary>Re-opens the default playback device.</summary>
    void Reset();
}
