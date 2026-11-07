#include "LedController.h"

void LedController::begin() {
  FastLED.addLeds<WS2812, led_config::kLedPin, GRB>(leds_, led_config::kLedCount);
  FastLED.setBrightness(255);
}

void LedController::setBrightness(uint8_t brightness) {
  FastLED.setBrightness(brightness);
}

void LedController::show() {
  FastLED.show();
}

void LedController::setAll(Rgb color) {
  fill_solid(leds_, led_config::kLedCount, CRGB(color.red, color.green, color.blue));
}

void LedController::setLed(uint16_t index, Rgb color) {
  if (index >= led_config::kLedCount) {
    return;
  }
  leds_[index] = CRGB(color.red, color.green, color.blue);
}

void LedController::setChannel(uint16_t index, uint8_t channel, uint8_t value) {
  if (index >= led_config::kLedCount) {
    return;
  }

  switch (channel) {
    case 0:
      leds_[index].r = value;
      break;
    case 1:
      leds_[index].g = value;
      break;
    case 2:
      leds_[index].b = value;
      break;
    default:
      break;
  }
}

void LedController::turnOff() {
  FastLED.setBrightness(0);
  fill_solid(leds_, led_config::kLedCount, CRGB::Black);
  FastLED.show();
}
