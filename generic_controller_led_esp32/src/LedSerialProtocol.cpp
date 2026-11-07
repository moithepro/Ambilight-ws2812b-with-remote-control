#include "LedSerialProtocol.h"

#include "Protocol.h"

LedSerialProtocol::LedSerialProtocol(Stream& serial, LedController& leds)
    : serial_(serial), leds_(leds) {}

void LedSerialProtocol::poll() {
  while (serial_.available() > 0) {
    const uint8_t value = static_cast<uint8_t>(serial_.read());

    if (state_ == State::Idle) {
      startFrame(value);
    } else {
      appendPayloadByte(value);
    }
  }

  if (state_ != State::Idle && hasTimedOut()) {
    finishFrame();
  }
}

void LedSerialProtocol::cancel() {
  state_ = State::Idle;
  payloadIndex_ = 0;
}

void LedSerialProtocol::discardPendingInput() {
  while (serial_.available() > 0) {
    serial_.read();
  }
}

void LedSerialProtocol::startFrame(uint8_t commandByte) {
  switch (static_cast<Command>(commandByte)) {
    case Command::Color:
      resetColorFrame();
      beginFrame(State::Color);
      break;
    case Command::Dynamic:
      beginFrame(State::Dynamic);
      break;
    case Command::Pixel:
      resetPixelFrame();
      beginFrame(State::Pixel);
      break;
    case Command::Brightness:
      beginFrame(State::Brightness);
      break;
    case Command::Off:
      leds_.turnOff();
      break;
    default:
      break;
  }
}

void LedSerialProtocol::beginFrame(State state) {
  state_ = state;
  payloadIndex_ = 0;
  frameStartedAtMs_ = millis();
}

void LedSerialProtocol::appendPayloadByte(uint8_t value) {
  switch (state_) {
    case State::Color:
      colorFrame_[payloadIndex_++] = value;
      if (payloadIndex_ >= led_config::kColorPayloadSize) {
        finishFrame();
      }
      break;

    case State::Pixel:
      pixelFrame_[payloadIndex_++] = value;
      if (payloadIndex_ >= led_config::kPixelPayloadSize) {
        finishFrame();
      }
      break;

    case State::Brightness:
      brightnessFrame_[0] = value;
      finishFrame();
      break;

    case State::Dynamic: {
      if (payloadIndex_ == 0) {
        leds_.setBrightness(value);
      } else {
        const size_t colorIndex = payloadIndex_ - 1;
        const uint16_t ledIndex = static_cast<uint16_t>(colorIndex / 3);
        const uint8_t channel = static_cast<uint8_t>(colorIndex % 3);
        leds_.setChannel(ledIndex, channel, value);
      }

      ++payloadIndex_;
      if (payloadIndex_ >= led_config::kDynamicPayloadSize) {
        finishFrame();
      }
      break;
    }

    case State::Idle:
      break;
  }
}

void LedSerialProtocol::finishFrame() {
  switch (state_) {
    case State::Color:
      applyColorFrame();
      break;
    case State::Dynamic:
      leds_.show();
      break;
    case State::Pixel:
      applyPixelFrame();
      break;
    case State::Brightness:
      leds_.setBrightness(brightnessFrame_[0]);
      leds_.show();
      break;
    case State::Idle:
      break;
  }

  state_ = State::Idle;
  payloadIndex_ = 0;
}

bool LedSerialProtocol::hasTimedOut() const {
  return (millis() - frameStartedAtMs_) >= timeoutFor(state_);
}

uint32_t LedSerialProtocol::timeoutFor(State state) const {
  switch (state) {
    case State::Color:
      return led_config::kColorFrameTimeoutMs;
    case State::Dynamic:
      return led_config::kDynamicFrameTimeoutMs;
    case State::Pixel:
      return led_config::kPixelFrameTimeoutMs;
    case State::Brightness:
      return led_config::kBrightnessFrameTimeoutMs;
    case State::Idle:
    default:
      return 0;
  }
}

void LedSerialProtocol::applyColorFrame() {
  leds_.setBrightness(colorFrame_[0]);
  leds_.setAll(Rgb{colorFrame_[1], colorFrame_[2], colorFrame_[3]});
  leds_.show();
}

void LedSerialProtocol::applyPixelFrame() {
  leds_.setBrightness(pixelFrame_[1]);
  leds_.setLed(pixelFrame_[0], Rgb{pixelFrame_[2], pixelFrame_[3], pixelFrame_[4]});
  leds_.show();
}

void LedSerialProtocol::resetColorFrame() {
  colorFrame_[0] = 255;
  colorFrame_[1] = 0;
  colorFrame_[2] = 0;
  colorFrame_[3] = 0;
}

void LedSerialProtocol::resetPixelFrame() {
  pixelFrame_[0] = 0;
  pixelFrame_[1] = 255;
  pixelFrame_[2] = 0;
  pixelFrame_[3] = 0;
  pixelFrame_[4] = 0;
}
