#pragma once

#include <Arduino.h>

#include "Config.h"
#include "LedController.h"

// Decodes the byte-oriented LED protocol sent by the PC application.
//
// Every command starts with a Command byte followed by a fixed-size payload.
// A payload that is not completed within its timeout is committed as-is,
// which keeps the controller responsive if the serial link is interrupted.
class LedSerialProtocol {
 public:
  LedSerialProtocol(Stream& serial, LedController& leds);

  void poll();
  void cancel();
  void discardPendingInput();

 private:
  enum class State {
    Idle,
    Color,
    Dynamic,
    Pixel,
    Brightness,
  };

  void startFrame(uint8_t commandByte);
  void beginFrame(State state);
  void appendPayloadByte(uint8_t value);
  void finishFrame();
  bool hasTimedOut() const;
  uint32_t timeoutFor(State state) const;

  void applyColorFrame();
  void applyPixelFrame();
  void resetColorFrame();
  void resetPixelFrame();

  Stream& serial_;
  LedController& leds_;

  State state_ = State::Idle;
  uint32_t frameStartedAtMs_ = 0;
  size_t payloadIndex_ = 0;

  uint8_t colorFrame_[led_config::kColorPayloadSize] = {255, 0, 0, 0};
  uint8_t pixelFrame_[led_config::kPixelPayloadSize] = {0, 255, 0, 0, 0};
  uint8_t brightnessFrame_[1] = {0};
};
