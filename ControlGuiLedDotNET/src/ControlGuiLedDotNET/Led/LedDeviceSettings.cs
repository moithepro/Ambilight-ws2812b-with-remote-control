namespace ControlGuiLed.Led;

public static class LedDeviceSettings
{
    // The strip runs along the right and left edges (32 regions each) and the
    // top and bottom edges (57 regions each), for 178 LEDs in total.
    public const int HorizontalEdgeRegionCount = 57;
    public const int VerticalEdgeRegionCount = 32;
    public const int LedCount = (2 * HorizontalEdgeRegionCount) + (2 * VerticalEdgeRegionCount);

    public const int BaudRate = 500_000;
    public const string PortName = "COM4";
    public const byte DefaultBrightness = 200;
}
