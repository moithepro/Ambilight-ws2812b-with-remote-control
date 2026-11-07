# ControlGuiLed (`.NET 10`)

Tray application that controls the WS2812B LED strip over a serial port and
turns remote-control button presses into PC keyboard shortcuts.

## Requirements

- Windows 10/11
- [.NET 10 SDK](https://dotnet.microsoft.com/) (tested with 10.0.403)
- A controller running the matching firmware
  (`generic_controller_led_esp32` or the Arduino sketch)
- A serial port for the controller at **500000 baud** (the configured default
  is preselected, and any available port can be chosen in the app)

## Build and run

```bash
dotnet build ControlGuiLedDotNET.sln
dotnet run --project src/ControlGuiLedDotNET
```

The application starts hidden in the notification area. Left-click the tray
icon to show or hide the window; use **Exit** in the tray menu to quit. The app
turns the LEDs off on exit.

The **Serial port** drop-down lists the ports currently available on the PC
and reconnects immediately when the selection changes. The list is refreshed
each time the drop-down is opened, so devices plugged in later appear without
restarting the app.

The **Ambilight capture** drop-down switches between **GDI (screen copy)**
(the default) and **DirectX 11**. The GDI path is the original non-DirectX
implementation and works when DXGI desktop duplication is unavailable, for
example in some virtual machines or remote sessions. It is slower than the
DirectX path. The change takes effect on the next Ambilight frame.

## Configuration

| Setting | Location |
| --- | --- |
| Default serial port and baud rate | `Led/LedDeviceSettings.cs` |
| LED count (178) | `Led/LedDeviceSettings.cs` |
| Default brightness | `Led/LedDeviceSettings.cs` |
| Screen sampling regions | `Ambilight/ScreenRegionLayout.cs` |

## Architecture

```
Program.cs                     Composition root and application start
App/AppComposition.cs          Wires the object graph
App/LedApplicationController   Use cases: modes, brightness, shutdown
App/MainForm.*                 UI and tray behaviour only
Effects/                       Color, Rainbow, Party and Ambilight effects
Ambilight/                     DX11/GDI capture, provider switch, region layout, sampling
Audio/                         WASAPI peak level for sound-reactive brightness
Input/                         Remote key -> SendInput keyboard mapping
Led/                           Protocol encoding, LED output, HSV math
Serial/                        Port catalog, connection, reconnect, key decoding
Timing/                        High-resolution effect scheduler
```

Design notes:

- `ILedOutput` is the only path to the LED hardware; effects never touch the
  serial port.
- `IBrightnessProvider` supplies either the slider brightness or a
  sound-derived value, so effects contain no audio code.
- Serial framing lives in `LedProtocol`; no byte layout is duplicated.
- Effects implement `ILedEffect` and are scheduled by `EffectScheduler`, so
  mode switching and timing are independent of the UI.

## Serial protocol

| Command | Byte | Payload |
| --- | --- | --- |
| Color | 22 | brightness, R, G, B |
| Dynamic | 23 | brightness, then R, G, B per LED |
| Pixel | 24 | LED index, brightness, R, G, B |
| Brightness | 25 | brightness |
| Off | 27 | none |

Keys received from the controller are in `Serial/RemoteKey.cs`. The remote's
power key toggles a navigation mode in which `4` and `6` act as left/right
arrow keys.
