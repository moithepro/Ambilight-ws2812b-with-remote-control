using ControlGuiLed.Ambilight;
using ControlGuiLed.Audio;
using ControlGuiLed.Effects;
using ControlGuiLed.Input;
using ControlGuiLed.Led;
using ControlGuiLed.Serial;
using ControlGuiLed.Timing;

namespace ControlGuiLed.App;

internal static class AppComposition
{
    public static LedApplicationController CreateController()
    {
        var serialConnection = new SerialConnection(
            LedDeviceSettings.PortName,
            LedDeviceSettings.BaudRate);

        var output = new LedStripOutput(serialConnection);
        var brightnessProvider = new LedBrightnessProvider(
            new WasapiAudioLevelSource(),
            LedDeviceSettings.DefaultBrightness);
        var ambilightFrameProvider = new SwitchableAmbilightFrameProvider(
            AmbilightCaptureMethod.Gdi);

        return new LedApplicationController(
            serialConnection,
            new EffectScheduler(),
            brightnessProvider,
            new RemoteKeyboardController(new SendInputKeyboard()),
            output,
            new ColorEffect(output, brightnessProvider),
            new RainbowEffect(output, brightnessProvider),
            new PartyEffect(output, brightnessProvider),
            new AmbilightEffect(ambilightFrameProvider, output, brightnessProvider),
            ambilightFrameProvider);
    }
}
