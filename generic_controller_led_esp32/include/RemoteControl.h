#pragma once

#include <Arduino.h>

#include "Config.h"
#include "LedController.h"
#include "LedSerialProtocol.h"
#include "Protocol.h"

// Handles IR keys and arbitrates between PC control and remote control.
class RemoteControl {
 public:
  RemoteControl(Stream& serial, LedController& leds, LedSerialProtocol& serialProtocol);

  void handle(IrKey key);
  bool isSerialControlEnabled() const;

 private:
  enum class RemoteColorMode {
    Color,
    Fade,
  };

  bool forwardToPc(IrKey key);
  void enterRemoteMode();
  void exitRemoteMode();
  void handleRemoteModeKey(IrKey key);
  bool applyColorPreset(IrKey key);
  void setRemoteColor(Rgb color);
  void adjustRemoteBrightness(int delta, IrKey sourceKey);
  void repeatLastBrightnessChange();

  Stream& serial_;
  LedController& leds_;
  LedSerialProtocol& serialProtocol_;

  bool serialControlEnabled_ = true;
  RemoteColorMode remoteColorMode_ = RemoteColorMode::Color;
  uint8_t remoteBrightness_ = led_config::kRemoteBrightness;
  bool repeatEnabled_ = false;
  IrKey lastBrightnessKey_ = IrKey::Repeat;
};
