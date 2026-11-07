#pragma once

#include <cstdint>

// Commands sent by the PC application to the controller.
enum class Command : uint8_t {
  Color = 22,
  Dynamic = 23,
  Pixel = 24,
  Brightness = 25,
  Waiting = 26,
  Off = 27,
};

// Single-byte key codes sent by the controller to the PC application.
enum class PcKey : uint8_t {
  Power = 0,
  VolumeUp = 1,
  FuncStop = 2,
  Backwards = 3,
  PlayPause = 4,
  Forward = 5,
  Down = 6,
  VolumeDown = 7,
  Up = 8,
  Digit0 = 9,
  Equalizer = 10,
  StopRepeat = 11,
  Digit1 = 12,
  Digit2 = 13,
  Digit3 = 14,
  Digit4 = 15,
  Digit5 = 16,
  Digit6 = 17,
  Digit7 = 18,
  Digit8 = 19,
  Digit9 = 20,
  CancelRemoteControl = 21,
};

// Raw NEC codes reported by the IR receiver.
// A raw code of zero is the repeat frame of the previous key.
enum class IrKey : uint32_t {
  Repeat = 0x00000000,

  Power = 0xBA45FF00,
  VolumeUp = 0xB946FF00,
  FuncStop = 0xB847FF00,
  Backwards = 0xBB44FF00,
  PlayPause = 0xBF40FF00,
  Forward = 0xBC43FF00,
  Down = 0xF807FF00,
  VolumeDown = 0xEA15FF00,
  Up = 0xF609FF00,
  Digit0 = 0xE916FF00,
  Equalizer = 0xE619FF00,
  StopRepeat = 0xF20DFF00,
  Digit1 = 0xF30CFF00,
  Digit2 = 0xE718FF00,
  Digit3 = 0xA15EFF00,
  Digit4 = 0xF708FF00,
  Digit5 = 0xE31CFF00,
  Digit6 = 0xA55AFF00,
  Digit7 = 0xBD42FF00,
  Digit8 = 0xAD52FF00,
  Digit9 = 0xB54AFF00,

  LedBrightnessUp = 0xFF00EF00,
  LedBrightnessDown = 0xFE01EF00,
  LedPowerOff = 0xFD02EF00,
  LedPowerOn = 0xFC03EF00,
  Red = 0xFB04EF00,
  Green = 0xFA05EF00,
  Blue = 0xF906EF00,
  White = 0xF807EF00,
  Red1 = 0xF708EF00,
  Green1 = 0xF609EF00,
  Blue1 = 0xF50AEF00,
  Flash = 0xF40BEF00,
  Red2 = 0xF30CEF00,
  Green2 = 0xF20DEF00,
  Blue2 = 0xF10EEF00,
  Strobe = 0xF00FEF00,
  Red3 = 0xEF10EF00,
  Green3 = 0xEE11EF00,
  Blue3 = 0xED12EF00,
  Fade = 0xEC13EF00,
  Red4 = 0xEB14EF00,
  Green4 = 0xEA15EF00,
  Blue4 = 0xE916EF00,
  Smooth = 0xE817EF00,
};
