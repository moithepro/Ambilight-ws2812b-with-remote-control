#pragma once

#include <optional>

#include "Protocol.h"

// Thin wrapper around the IRremote receiver.
class IrRemote {
 public:
  void begin();
  std::optional<IrKey> poll();
};
