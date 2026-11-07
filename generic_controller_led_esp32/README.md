# ESP32 LED controller

PlatformIO firmware for the WS2812B Ambilight controller on an ESP32
(`esp32dev` board).

## Hardware

| Signal | Default pin |
| --- | --- |
| WS2812B data | GPIO 27 |
| IR receiver | GPIO 12 |
| LED count | 178 (WS2812, GRB order) |

The serial link to the PC runs at **500000 baud** (the ESP32 variant of the
original Arduino sketch). The PC application uses `COM3` by default.

## Build and upload

```bash
pio run
pio run --target upload
pio device monitor
```

Pinned dependencies (see `platformio.ini`):

- `espressif32` platform 8.0.0
- FastLED 3.10.9
- IRremote 4.7.1 (NEC decoding only)

## Project layout

```
include/Config.h              Hardware pins, timeouts and payload sizes
include/Protocol.h            IR codes, PC key codes and serial commands
include/LedController.h       FastLED buffer and output
include/IrRemote.h            IR receiver wrapper
include/LedSerialProtocol.h   PC -> controller frame decoder
include/RemoteControl.h       Remote-control mode and PC key forwarding
src/                          Implementations and main loop
```

Each class has a single responsibility:

- `LedController` owns the LED buffer and all FastLED calls.
- `IrRemote` only polls and decodes IR frames.
- `LedSerialProtocol` only parses serial frames and applies them.
- `RemoteControl` decides who owns the LEDs and maps IR keys to actions.

## Serial protocol (PC -> controller)

All frames begin with a command byte.

| Command | Byte | Payload |
| --- | --- | --- |
| Color | 22 | brightness, R, G, B |
| Dynamic | 23 | brightness, then R, G, B for every LED |
| Pixel | 24 | LED index, brightness, R, G, B |
| Brightness | 25 | brightness |
| Off | 27 | none |

Incomplete frames are committed after a short timeout so a disturbed serial
link cannot leave the parser stuck.

## Remote control

Pressing the LED remote's **power on** button transfers control to the remote
and sends key `21` to the PC application. Pressing it again returns control to
the serial link.

While the remote owns the LEDs, brightness up/down (with NEC repeat support)
and the color preset buttons are active. The media and number keys on the
main remote are forwarded to the PC as single-byte key codes and can trigger
keyboard shortcuts.
