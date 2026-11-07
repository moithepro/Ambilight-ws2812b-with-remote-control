#include "IrRemote.h"

#include <IRremote.hpp>

#include "Config.h"

void IrRemote::begin() {
  IrReceiver.begin(led_config::kIrReceiverPin, ENABLE_LED_FEEDBACK);
}

std::optional<IrKey> IrRemote::poll() {
  if (!IrReceiver.decode()) {
    return std::nullopt;
  }

  const uint32_t rawCode = IrReceiver.decodedIRData.decodedRawData;
  IrReceiver.resume();
  return static_cast<IrKey>(rawCode);
}
