# 1 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"


// CONTROL RECIVE/SEND CODES:
# 50 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
// Serial Control send codes
# 75 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
# 76 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino" 2
# 77 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino" 2
// RECIVE CODES (bytes from serial com):






// Timeout for LED Modes Serial com data recieving





// IR Reciever pin is 7

// LED pin is 5


// Data arrays bounds



const int DYNAMIC_MAX_INDEX = 3 * 178;
CRGB leds[178];

enum class LED_MODE
{
  COLOR,
  DYNAMIC,
  PIXEL,
  BRIGHTNESS,
  WAITING,
  OFF
} typedef LED_MODE;

enum class CONTROL_LED_MODE
{
  COLOR,
  FADE
} typedef CONTROL_LED_MODE;

unsigned long colorModeTimeoutTimestamp = 0;
bool colorModeTimeoutRunning = false;
unsigned long pixelModeTimeoutTimestamp = 0;
bool pixelModeTimeoutRunning = false;
unsigned long dynamicModeTimeoutTimestamp = 0;
bool dynamicModeTimeoutRunning = false;
unsigned long brightnessModeTimeoutTimestamp = 0;
bool brightnessModeTimeoutRunning = false;
LED_MODE currentLedMode = LED_MODE::COLOR;
CONTROL_LED_MODE currentControlLedMode = CONTROL_LED_MODE::COLOR;
byte currentColorIndex = 0;

long prevControlCode = 0x000000;
bool prevControlIsUsed = false;
byte currentColorARGB[] = {255, 255, 0, 0};
byte currentPixelIARGB[] = {0, 255, 0, 0, 255};
byte currentPixelIndex = 0;
int currentDynamicIndex = 0;
byte currentBrightness = 255;
byte currentControlBrightness = 100;
long LastControlLedUpdateTimestamp = 0;
float ControlLedModeFadeBrightness = 1;

bool serialMode = true;
byte floatRGBToByte(float f){
  return (byte)(f * 255.0);
}
void colorModeShow()
{
  FastLED.setBrightness(currentColorARGB[0]);
  for (int i = 0; i < 178; i++)
  {

    leds[i].r = currentColorARGB[1];
    leds[i].g = currentColorARGB[2];
    leds[i].b = currentColorARGB[3];
  }
  FastLED.show();
}
void show()
{
  FastLED.setBrightness(currentBrightness);
  FastLED.show();
}
void pixelModeShow()
{
  FastLED.setBrightness(currentPixelIARGB[1]);
  leds[currentPixelIARGB[0]].r = currentPixelIARGB[2];
  leds[currentPixelIARGB[0]].g = currentPixelIARGB[3];
  leds[currentPixelIARGB[0]].b = currentPixelIARGB[4];
  FastLED.show();
}
void turnOff()
{
  FastLED.setBrightness(0);
  for (int i = 0; i < 178; i++)
  {

    leds[i].r = 0;
    leds[i].g = 0;
    leds[i].b = 0;
  }
  FastLED.show();
}
void switchToControlMode()
{
  if (serialMode)
  {
    Serial.write(21);
    FastLED.setBrightness(currentControlBrightness);
    currentControlLedMode = CONTROL_LED_MODE::COLOR;
    serialMode = false;
  }
}
void setup()
{
  Serial.begin(250000);
  IrReceiver.begin(12, true);
  FastLED.addLeds<WS2812, 27, GRB>(leds, 178);
}

void loop()
{
  // if Serial is available read data, store in Arrays and update LED
  if (Serial.available() && serialMode)
  {
    switch (currentLedMode)
    {
    case LED_MODE::WAITING:
    {
      byte code = Serial.read();
      switch (code)
      {
      case 22:
      {
        currentLedMode = LED_MODE::COLOR;
        break;
      }
      case 23:
      {
        currentLedMode = LED_MODE::DYNAMIC;
        break;
      }
      case 24:
      {
        currentLedMode = LED_MODE::PIXEL;
        break;
      }
      case 25:
      {
        currentLedMode = LED_MODE::BRIGHTNESS;
        break;
      }
      case 27:
      {
        FastLED.setBrightness(0);
        break;
      }
      }
      break;
    }
    case LED_MODE::COLOR:

    {
      // Initialise timeout if not running
      if (!colorModeTimeoutRunning)
      {
        colorModeTimeoutTimestamp = millis();
        colorModeTimeoutRunning = true;
      }
      // If timeout passed reset variables
      if (colorModeTimeoutRunning && colorModeTimeoutTimestamp + 100 < millis())
      {
        currentColorIndex = 0;
        colorModeShow();
        colorModeTimeoutRunning = false;
        currentLedMode = LED_MODE::WAITING;
        goto colorCaseEnd;
      }
      // Read & Update
      while (Serial.available() > 0)
      {
        byte data = Serial.read();
        currentColorARGB[currentColorIndex++] = data;
        if (currentColorIndex > 3)
        {
          currentColorIndex = 0;
          colorModeShow();
          colorModeTimeoutRunning = false;
          currentLedMode = LED_MODE::WAITING;
          goto colorCaseEnd;
        }
      }
    colorCaseEnd:
      break;
    }
    case LED_MODE::DYNAMIC:

    {
      // Initialise timeout if not running
      if (!dynamicModeTimeoutRunning)
      {
        dynamicModeTimeoutTimestamp = millis();
        dynamicModeTimeoutRunning = true;
      }
      // If timeout passed reset variables
      if (dynamicModeTimeoutRunning && dynamicModeTimeoutTimestamp + 200 < millis())
      {
        currentDynamicIndex = 0;
        show();
        dynamicModeTimeoutRunning = false;
        currentLedMode = LED_MODE::WAITING;
        goto dynamicCaseEnd;
      }
      // Read & Update
      while (Serial.available() > 0)
      {
        byte data = Serial.read();
        if (currentDynamicIndex > 0)
        {
          int ledIndex = (currentDynamicIndex - 1) / 3;
          int colorIndex = (currentDynamicIndex - 1) % 3;
          /*switch (colorIndex) {

            case 0:

              {

                leds[ledIndex].r = data;

                break;

              }

            case 1:

              {

                leds[ledIndex].g = data;

                break;

              }

            case 2:

              {

                leds[ledIndex].b = data;

                break;

              }

            }*/
# 317 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
          leds[ledIndex][colorIndex] = data;
        }
        else
        {
          currentBrightness = data;
        }
        currentDynamicIndex++;
        if (currentDynamicIndex > DYNAMIC_MAX_INDEX)
        {
          currentDynamicIndex = 0;
          show();
          dynamicModeTimeoutRunning = false;
          currentLedMode = LED_MODE::WAITING;
          goto dynamicCaseEnd;
        }
      }
    dynamicCaseEnd:
      break;
    }
    case LED_MODE::PIXEL:

    {
      // Initialise timeout if not running
      if (!pixelModeTimeoutRunning)
      {
        pixelModeTimeoutTimestamp = millis();
        pixelModeTimeoutRunning = true;
      }
      // If timeout passed reset variables
      if (pixelModeTimeoutRunning && pixelModeTimeoutTimestamp + 50 < millis())
      {
        currentPixelIndex = 0;
        pixelModeShow();
        pixelModeTimeoutRunning = false;
        currentLedMode = LED_MODE::WAITING;
        goto pixelCaseEnd;
      }
      // Read & Update
      while (Serial.available() > 0)
      {
        byte data = Serial.read();
        currentPixelIARGB[currentPixelIndex++] = data;
        if (currentPixelIndex > 4)
        {
          currentPixelIndex = 0;
          pixelModeShow();
          pixelModeTimeoutRunning = false;
          currentLedMode = LED_MODE::WAITING;
          goto pixelCaseEnd;
        }
      }
    pixelCaseEnd:
      break;
    }
    case LED_MODE::BRIGHTNESS:
    {
      // Initialise timeout if not running
      if (!brightnessModeTimeoutRunning)
      {
        brightnessModeTimeoutTimestamp = millis();
        brightnessModeTimeoutRunning = true;
      }
      // If timeout passed reset variables
      if (brightnessModeTimeoutRunning && brightnessModeTimeoutTimestamp + 50 < millis())
      {
        show();
        brightnessModeTimeoutRunning = false;
        currentLedMode = LED_MODE::WAITING;
        break;
      }
      // Read & Update
      if (Serial.available() > 0)
      {
        byte data = Serial.read();
        currentBrightness = data;
        show();
        brightnessModeTimeoutRunning = false;
        currentLedMode = LED_MODE::WAITING;
        break;
      }
    }
    case LED_MODE::OFF:
    {
      break;
    }
    }
  }
  // Check for IR input
  if (IrReceiver.decode())
  {
    long data = IrReceiver.decodedIRData.decodedRawData;
    IrReceiver.decodedIRData.decodedRawData = 0;
    switch (data)
    {
    case 0x000000:

      break;
    case 0xBA45FF00:
      Serial.write(0);
      prevControlIsUsed = false;
      break;
    case 0xB946FF00:
      Serial.write(1);
      prevControlIsUsed = false;
      break;
    case 0xB847FF00:
      Serial.write(2);
      prevControlIsUsed = false;
      break;
    case 0xBB44FF00:
      Serial.write(3);
      prevControlIsUsed = false;
      break;
    case 0xBF40FF00:
      Serial.write(4);
      prevControlIsUsed = false;
      break;
    case 0xBC43FF00:
      Serial.write(5);
      prevControlIsUsed = false;
      break;
    case 0xF807FF00:
      Serial.write(6);
      prevControlIsUsed = false;
      break;
    case 0xEA15FF00:
      Serial.write(7);
      prevControlIsUsed = false;
      break;
    case 0xF609FF00:
      Serial.write(8);
      prevControlIsUsed = false;
      break;
    case 0xE916FF00:
      Serial.write(9);
      prevControlIsUsed = false;
      break;
    case 0xE619FF00:
      Serial.write(10);
      prevControlIsUsed = false;
      break;
    case 0xF20DFF00:
      Serial.write(11);
      prevControlIsUsed = false;
      break;
    case 0xF30CFF00:
      Serial.write(12);
      prevControlIsUsed = false;
      break;
    case 0xE718FF00:
      Serial.write(13);
      prevControlIsUsed = false;
      break;
    case 0xA15EFF00:
      Serial.write(14);
      prevControlIsUsed = false;
      break;
    case 0xF708FF00:
      Serial.write(15);
      prevControlIsUsed = false;
      break;
    case 0xE31CFF00:
      Serial.write(16);
      prevControlIsUsed = false;
      break;
    case 0xA55AFF00:
      Serial.write(17);
      prevControlIsUsed = false;
      break;
    case 0xBD42FF00:
      Serial.write(18);
      prevControlIsUsed = false;
      break;
    case 0xAD52FF00:
      Serial.write(19);
      prevControlIsUsed = false;
      break;
    case 0xB54AFF00:
      Serial.write(20);
      prevControlIsUsed = false;
      break;

    // control2: Led Control ON Check
    case 0xFC03EF00:
      if (serialMode)
      {
        switchToControlMode();
      }
      else
      {
        serialMode = true;
      }
      break;
    }
    // if serial mode is false that means that LED is controlled by Controller
    if (!serialMode)
    {
      switch (data)
      {
      case 0x000000:
        switch (prevControlCode)
        {
        case 0xFF00EF00:
          if (currentControlLedMode == CONTROL_LED_MODE::COLOR)
          {
            if (currentControlBrightness <= 255 - 10)
              currentControlBrightness += 10;
            FastLED.setBrightness(currentControlBrightness);
            FastLED.show();
          }
          break;
        case 0xFE01EF00:
          if (currentControlLedMode == CONTROL_LED_MODE::COLOR)
          {
            if (currentControlBrightness >= 0 + 10)
              currentControlBrightness -= 10;
            FastLED.setBrightness(currentControlBrightness);
          }
          FastLED.show();
          break;
        }
        break;
      case 0xFD02EF00:
        FastLED.setBrightness(0);
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(0, 0, 0);
        }
        FastLED.show();
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xFF00EF00:
        if (currentControlLedMode == CONTROL_LED_MODE::COLOR)
        {
          if (currentControlBrightness <= 255 - 10)
            currentControlBrightness += 10;
          FastLED.setBrightness(currentControlBrightness);
          FastLED.show();
          prevControlIsUsed = true;
          prevControlCode = data;
        }
        break;
      case 0xFE01EF00:
        if (currentControlLedMode == CONTROL_LED_MODE::COLOR)
        {
          if (currentControlBrightness >= 0 + 10)
            currentControlBrightness -= 10;
          FastLED.setBrightness(currentControlBrightness);
          FastLED.show();
          prevControlIsUsed = true;
          prevControlCode = data;
        }
        break;
      case 0xFB04EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(255, 0, 0);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xF708EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(247, 70, 5);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xF30CEF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(247, 110, 5);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xEF10EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(247, 154, 5);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xEB14EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(247, 211, 5);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xFA05EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(0, 255, 0);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xF609EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(55, 222, 64);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xF20DEF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(55, 222, 139);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xEE11EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(55, 222, 219);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xEA15EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(0, 166, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xF906EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(0, 0, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xF50AEF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(0, 110, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xF10EEF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(76, 0, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xED12EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(130, 0, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xE916EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(255, 0, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xF807EF00:
        for (int i = 0; i < 178; i++)
        {
          leds[i] = CRGB(255, 255, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case 0xEC13EF00:
        currentControlLedMode = CONTROL_LED_MODE::FADE;
        break;
      }
      switch (currentControlLedMode)
      {
      case CONTROL_LED_MODE::FADE:
        //Fade led...
        break;

      default:
        break;
      }

    }
    IrReceiver.resume();
  }
}
