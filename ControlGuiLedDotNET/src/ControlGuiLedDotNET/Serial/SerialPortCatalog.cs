using System.IO.Ports;

namespace ControlGuiLed.Serial;

public static class SerialPortCatalog
{
    public static IReadOnlyList<string> GetAvailablePorts() =>
        SerialPort.GetPortNames()
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
}
