namespace ControlGuiLed.Led;

public interface IBrightnessProvider
{
    byte BaseBrightness { get; set; }

    bool SoundReactive { get; set; }

    byte GetCurrentBrightness();
}
