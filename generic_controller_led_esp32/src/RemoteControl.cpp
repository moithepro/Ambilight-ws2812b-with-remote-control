#include "RemoteControl.h"

#include <algorithm>
#include <array>

namespace {

struct PcKeyMapping {
  IrKey irKey;
  PcKey pcKey;
};

struct ColorPreset {
  IrKey irKey;
  Rgb color;
};

constexpr std::array<PcKeyMapping, 21> kPcKeyMappings = {{
    {IrKey::Power, PcKey::Power},
    {IrKey::VolumeUp, PcKey::VolumeUp},
    {IrKey::FuncStop, PcKey::FuncStop},
    {IrKey::Backwards, PcKey::Backwards},
    {IrKey::PlayPause, PcKey::PlayPause},
    {IrKey::Forward, PcKey::Forward},
    {IrKey::Down, PcKey::Down},
    {IrKey::VolumeDown, PcKey::VolumeDown},
    {IrKey::Up, PcKey::Up},
    {IrKey::Digit0, PcKey::Digit0},
    {IrKey::Equalizer, PcKey::Equalizer},
    {IrKey::StopRepeat, PcKey::StopRepeat},
    {IrKey::Digit1, PcKey::Digit1},
    {IrKey::Digit2, PcKey::Digit2},
    {IrKey::Digit3, PcKey::Digit3},
    {IrKey::Digit4, PcKey::Digit4},
    {IrKey::Digit5, PcKey::Digit5},
    {IrKey::Digit6, PcKey::Digit6},
    {IrKey::Digit7, PcKey::Digit7},
    {IrKey::Digit8, PcKey::Digit8},
    {IrKey::Digit9, PcKey::Digit9},
}};

constexpr std::array<ColorPreset, 16> kColorPresets = {{
    {IrKey::Red, Rgb{255, 0, 0}},
    {IrKey::Green, Rgb{0, 255, 0}},
    {IrKey::Blue, Rgb{0, 0, 255}},
    {IrKey::White, Rgb{255, 255, 255}},
    {IrKey::Red1, Rgb{247, 70, 5}},
    {IrKey::Green1, Rgb{55, 222, 64}},
    {IrKey::Blue1, Rgb{0, 110, 255}},
    {IrKey::Red2, Rgb{247, 110, 5}},
    {IrKey::Green2, Rgb{55, 222, 139}},
    {IrKey::Blue2, Rgb{76, 0, 255}},
    {IrKey::Red3, Rgb{247, 154, 5}},
    {IrKey::Green3, Rgb{55, 222, 219}},
    {IrKey::Blue3, Rgb{130, 0, 255}},
    {IrKey::Red4, Rgb{247, 211, 5}},
    {IrKey::Green4, Rgb{0, 166, 255}},
    {IrKey::Blue4, Rgb{255, 0, 255}},
}};

}  // namespace

RemoteControl::RemoteControl(Stream& serial, LedController& leds,
                             LedSerialProtocol& serialProtocol)
    : serial_(serial), leds_(leds), serialProtocol_(serialProtocol) {}

bool RemoteControl::isSerialControlEnabled() const {
  return serialControlEnabled_;
}

void RemoteControl::handle(IrKey key) {
  if (key == IrKey::Repeat) {
    if (!serialControlEnabled_ && repeatEnabled_) {
      repeatLastBrightnessChange();
    }
    return;
  }

  if (forwardToPc(key)) {
    repeatEnabled_ = false;
    return;
  }

  if (key == IrKey::LedPowerOn) {
    if (serialControlEnabled_) {
      enterRemoteMode();
    } else {
      exitRemoteMode();
    }
    return;
  }

  if (!serialControlEnabled_) {
    handleRemoteModeKey(key);
  }
}

bool RemoteControl::forwardToPc(IrKey key) {
  for (const auto& mapping : kPcKeyMappings) {
    if (mapping.irKey == key) {
      serial_.write(static_cast<uint8_t>(mapping.pcKey));
      return true;
    }
  }
  return false;
}

void RemoteControl::enterRemoteMode() {
  serial_.write(static_cast<uint8_t>(PcKey::CancelRemoteControl));
  leds_.setBrightness(remoteBrightness_);
  remoteColorMode_ = RemoteColorMode::Color;
  repeatEnabled_ = false;
  serialControlEnabled_ = false;
  serialProtocol_.cancel();
}

void RemoteControl::exitRemoteMode() {
  serialControlEnabled_ = true;
  repeatEnabled_ = false;
}

void RemoteControl::handleRemoteModeKey(IrKey key) {
  switch (key) {
    case IrKey::LedPowerOff:
      leds_.turnOff();
      remoteColorMode_ = RemoteColorMode::Color;
      repeatEnabled_ = false;
      break;

    case IrKey::LedBrightnessUp:
      adjustRemoteBrightness(led_config::kRemoteBrightnessStep, key);
      break;

    case IrKey::LedBrightnessDown:
      adjustRemoteBrightness(-led_config::kRemoteBrightnessStep, key);
      break;

    case IrKey::Fade:
      // Retained for compatibility with the original remote. There is no
      // fade animation implemented in the firmware.
      remoteColorMode_ = RemoteColorMode::Fade;
      repeatEnabled_ = false;
      break;

    default:
      applyColorPreset(key);
      break;
  }
}

bool RemoteControl::applyColorPreset(IrKey key) {
  for (const auto& preset : kColorPresets) {
    if (preset.irKey == key) {
      setRemoteColor(preset.color);
      return true;
    }
  }
  return false;
}

void RemoteControl::setRemoteColor(Rgb color) {
  // Restore the remote brightness: a previous power-off sets FastLED's
  // brightness to zero, and a color preset should bring the strip back.
  leds_.setBrightness(remoteBrightness_);
  leds_.setAll(color);
  leds_.show();
  remoteColorMode_ = RemoteColorMode::Color;
  repeatEnabled_ = false;
}

void RemoteControl::adjustRemoteBrightness(int delta, IrKey sourceKey) {
  if (remoteColorMode_ != RemoteColorMode::Color) {
    return;
  }

  const int adjusted =
      std::clamp(static_cast<int>(remoteBrightness_) + delta, 0, 255);
  remoteBrightness_ = static_cast<uint8_t>(adjusted);
  leds_.setBrightness(remoteBrightness_);
  leds_.show();

  repeatEnabled_ = true;
  lastBrightnessKey_ = sourceKey;
}

void RemoteControl::repeatLastBrightnessChange() {
  if (lastBrightnessKey_ == IrKey::LedBrightnessUp) {
    adjustRemoteBrightness(led_config::kRemoteBrightnessStep, lastBrightnessKey_);
  } else if (lastBrightnessKey_ == IrKey::LedBrightnessDown) {
    adjustRemoteBrightness(-led_config::kRemoteBrightnessStep, lastBrightnessKey_);
  }
}
