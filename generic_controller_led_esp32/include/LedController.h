#pragma once

#include <FastLED.h>

#include "Config.h"
#include "Rgb.h"

// Owns the LED buffer and all FastLED calls.
class LedController {
 public:
  void begin();

  void setBrightness(uint8_t brightness);
  void show();

  void setAll(Rgb color);
  void setLed(uint16_t index, Rgb color);
  void setChannel(uint16_t index, uint8_t channel, uint8_t value);
  void turnOff();

 private:
  CRGB leds_[led_config::kLedCount];
};
