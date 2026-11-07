#pragma once

#include <cstddef>
#include <cstdint>

namespace led_config {

// Hardware
inline constexpr uint8_t kLedPin = 27;
inline constexpr uint16_t kLedCount = 178;
inline constexpr uint8_t kIrReceiverPin = 12;

// Serial link to the PC application
inline constexpr uint32_t kSerialBaudRate = 500000;

// Remote-control defaults
inline constexpr uint8_t kRemoteBrightness = 100;
inline constexpr uint8_t kRemoteBrightnessStep = 10;

// A frame that is not completed within its timeout is committed as-is.
inline constexpr uint32_t kColorFrameTimeoutMs = 100;
inline constexpr uint32_t kDynamicFrameTimeoutMs = 200;
inline constexpr uint32_t kPixelFrameTimeoutMs = 50;
inline constexpr uint32_t kBrightnessFrameTimeoutMs = 50;

// Payload sizes (bytes following the command byte)
inline constexpr size_t kColorPayloadSize = 4;   // brightness, red, green, blue
inline constexpr size_t kPixelPayloadSize = 5;   // led index, brightness, red, green, blue
inline constexpr size_t kDynamicPayloadSize = 1 + (3 * kLedCount);  // brightness, then RGB per LED

}  // namespace led_config
