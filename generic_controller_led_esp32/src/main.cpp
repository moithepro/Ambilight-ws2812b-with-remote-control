#include <Arduino.h>

#include "Config.h"
#include "IrRemote.h"
#include "LedController.h"
#include "LedSerialProtocol.h"
#include "RemoteControl.h"

namespace {

LedController leds;
IrRemote irRemote;
LedSerialProtocol serialProtocol(Serial, leds);
RemoteControl remoteControl(Serial, leds, serialProtocol);

}  // namespace

void setup() {
  Serial.begin(led_config::kSerialBaudRate);
  leds.begin();
  irRemote.begin();
}

void loop() {
  if (remoteControl.isSerialControlEnabled()) {
    serialProtocol.poll();
  } else {
    // The PC keeps sending frames while the remote owns the LEDs. Drop them
    // so that stale frames are not replayed when serial control returns.
    serialProtocol.discardPendingInput();
  }

  if (const auto key = irRemote.poll()) {
    remoteControl.handle(*key);
  }
}
