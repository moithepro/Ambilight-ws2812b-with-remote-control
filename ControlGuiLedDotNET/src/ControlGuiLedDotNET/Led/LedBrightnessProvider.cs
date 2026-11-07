using ControlGuiLed.Audio;

namespace ControlGuiLed.Led;

public sealed class LedBrightnessProvider : IBrightnessProvider, IDisposable
{
    private readonly IAudioLevelSource _audioLevelSource;

    public LedBrightnessProvider(IAudioLevelSource audioLevelSource, byte baseBrightness)
    {
        _audioLevelSource = audioLevelSource;
        BaseBrightness = baseBrightness;
    }

    public byte BaseBrightness { get; set; }

    public bool SoundReactive { get; set; }

    public byte GetCurrentBrightness()
    {
        if (!SoundReactive)
        {
            return BaseBrightness;
        }

        if (BaseBrightness == 0)
        {
            return 0;
        }

        var peakLevel = _audioLevelSource.GetPeakLevel();
        if (peakLevel <= 0)
        {
            return 0;
        }

        var multiplier = peakLevel / (BaseBrightness / 255.0);
        return (byte)Math.Clamp((int)(255 * multiplier), 0, 255);
    }

    public void ResetAudioDevice() => _audioLevelSource.Reset();

    public void Dispose() => _audioLevelSource.Dispose();
}
