#include <Arduino.h>
#line 1 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"


// CONTROL RECIVE/SEND CODES:
#define CONTROL_POWER_RECV_CODE 0xBA45FF00
#define CONTROL_VOL_UP_RECV_CODE 0xB946FF00
#define CONTROL_FUNC_STOP_RECV_CODE 0xB847FF00
#define CONTROL_BACKWARDS_RECV_CODE 0xBB44FF00
#define CONTROL_PLAY_PAUSE_RECV_CODE 0xBF40FF00
#define CONTROL_FORWARD_RECV_CODE 0xBC43FF00
#define CONTROL_DOWN_RECV_CODE 0xF807FF00
#define CONTROL_VOL_DOWN_RECV_CODE 0xEA15FF00
#define CONTROL_UP_RECV_CODE 0xF609FF00
#define CONTROL_0_RECV_CODE 0xE916FF00
#define CONTROL_EQ_RECV_CODE 0xE619FF00
#define CONTROL_ST_REPT_RECV_CODE 0xF20DFF00
#define CONTROL_1_RECV_CODE 0xF30CFF00
#define CONTROL_2_RECV_CODE 0xE718FF00
#define CONTROL_3_RECV_CODE 0xA15EFF00
#define CONTROL_4_RECV_CODE 0xF708FF00
#define CONTROL_5_RECV_CODE 0xE31CFF00
#define CONTROL_6_RECV_CODE 0xA55AFF00
#define CONTROL_7_RECV_CODE 0xBD42FF00
#define CONTROL_8_RECV_CODE 0xAD52FF00
#define CONTROL_9_RECV_CODE 0xB54AFF00

#define CONTROL2_BRIGHTNESS_UP_RECV_CODE 0xFF00EF00
#define CONTROL2_BRIGHTNESS_DOWN_RECV_CODE 0xFE01EF00
#define CONTROL2_POWER_OFF_RECV_CODE 0xFD02EF00
#define CONTROL2_POWER_ON_RECV_CODE 0xFC03EF00
#define CONTROL2_R_RECV_CODE 0xFB04EF00
#define CONTROL2_G_RECV_CODE 0xFA05EF00
#define CONTROL2_B_RECV_CODE 0xF906EF00
#define CONTROL2_W_RECV_CODE 0xF807EF00
#define CONTROL2_R1_RECV_CODE 0xF708EF00
#define CONTROL2_G1_RECV_CODE 0xF609EF00
#define CONTROL2_B1_RECV_CODE 0xF50AEF00
#define CONTROL2_FLASH_RECV_CODE 0xF40BEF00
#define CONTROL2_R2_RECV_CODE 0xF30CEF00
#define CONTROL2_G2_RECV_CODE 0xF20DEF00
#define CONTROL2_B2_RECV_CODE 0xF10EEF00
#define CONTROL2_STROBE_RECV_CODE 0xF00FEF00
#define CONTROL2_R3_RECV_CODE 0xEF10EF00
#define CONTROL2_G3_RECV_CODE 0xEE11EF00
#define CONTROL2_B3_RECV_CODE 0xED12EF00
#define CONTROL2_FADE_RECV_CODE 0xEC13EF00
#define CONTROL2_R4_RECV_CODE 0xEB14EF00
#define CONTROL2_G4_RECV_CODE 0xEA15EF00
#define CONTROL2_B4_RECV_CODE 0xE916EF00
#define CONTROL2_SMOOTH_RECV_CODE 0xE817EF00
// Serial Control send codes
#define CONTROL_POWER_SEND_CODE 0
#define CONTROL_VOL_UP_SEND_CODE 1
#define CONTROL_FUNC_STOP_SEND_CODE 2
#define CONTROL_BACKWARDS_SEND_CODE 3
#define CONTROL_PLAY_PAUSE_SEND_CODE 4
#define CONTROL_FORWARD_SEND_CODE 5
#define CONTROL_DOWN_SEND_CODE 6
#define CONTROL_VOL_DOWN_SEND_CODE 7
#define CONTROL_UP_SEND_CODE 8
#define CONTROL_0_SEND_CODE 9
#define CONTROL_EQ_SEND_CODE 10
#define CONTROL_ST_REPT_SEND_CODE 11
#define CONTROL_1_SEND_CODE 12
#define CONTROL_2_SEND_CODE 13
#define CONTROL_3_SEND_CODE 14
#define CONTROL_4_SEND_CODE 15
#define CONTROL_5_SEND_CODE 16
#define CONTROL_6_SEND_CODE 17
#define CONTROL_7_SEND_CODE 18
#define CONTROL_8_SEND_CODE 19
#define CONTROL_9_SEND_CODE 20

#define CANCEL_LED_SEND_CODE 21

#include <IRremote.h>
#include <FastLED.h>
// RECIVE CODES (bytes from serial com):
#define COLOR_RECV_CODE 22
#define DYNAMIC_RECV_CODE 23
#define PIXEL_RECV_CODE 24
#define BRIGHTNESS_RECV_CODE 25
#define WAITING_RECV_CODE 26
#define OFF_RECV_CODE 27
// Timeout for LED Modes Serial com data recieving
#define COLOR_MODE_TIMEOUT_INTERVAL 100
#define PIXEL_MODE_TIMEOUT_INTERVAL 50
#define DYNAMIC_MODE_TIMEOUT_INTERVAL 200
#define BRIGHTNESS_MODE_TIMEOUT_INTERVAL 50
#define BAUD_RATE 250000
// IR Reciever pin is 7
#define CONTROL_RECV_PIN 12
// LED pin is 5
#define LED_PIN 27
#define NUM_LEDS 178
// Data arrays bounds
#define COLOR_ARR_MAX_INDEX 3
#define PIXEL_ARR_MAX_INDEX 4

const int DYNAMIC_MAX_INDEX = 3 * NUM_LEDS;
CRGB leds[NUM_LEDS];

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
#line 142 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
byte floatRGBToByte(float f);
#line 145 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
void colorModeShow();
#line 157 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
void show();
#line 162 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
void pixelModeShow();
#line 170 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
void turnOff();
#line 182 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
void switchToControlMode();
#line 192 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
void setup();
#line 199 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
void loop();
#line 142 "d:\\Documents\\Ambilight-ws2812b-with-remote-control\\generic_controller_led_esp32\\src\\generic_controller_led_esp32.ino"
byte floatRGBToByte(float f){
  return (byte)(f * 255.0);
}
void colorModeShow()
{
  FastLED.setBrightness(currentColorARGB[0]);
  for (int i = 0; i < NUM_LEDS; i++)
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
  for (int i = 0; i < NUM_LEDS; i++)
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
    Serial.write(CANCEL_LED_SEND_CODE);
    FastLED.setBrightness(currentControlBrightness);
    currentControlLedMode = CONTROL_LED_MODE::COLOR;
    serialMode = false;
  }
}
void setup()
{
  Serial.begin(BAUD_RATE);
  IrReceiver.begin(CONTROL_RECV_PIN, ENABLE_LED_FEEDBACK);
  FastLED.addLeds<WS2812, LED_PIN, GRB>(leds, NUM_LEDS);
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
      case COLOR_RECV_CODE:
      {
        currentLedMode = LED_MODE::COLOR;
        break;
      }
      case DYNAMIC_RECV_CODE:
      {
        currentLedMode = LED_MODE::DYNAMIC;
        break;
      }
      case PIXEL_RECV_CODE:
      {
        currentLedMode = LED_MODE::PIXEL;
        break;
      }
      case BRIGHTNESS_RECV_CODE:
      {
        currentLedMode = LED_MODE::BRIGHTNESS;
        break;
      }
      case OFF_RECV_CODE:
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
      if (colorModeTimeoutRunning && colorModeTimeoutTimestamp + COLOR_MODE_TIMEOUT_INTERVAL < millis())
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
        if (currentColorIndex > COLOR_ARR_MAX_INDEX)
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
      if (dynamicModeTimeoutRunning && dynamicModeTimeoutTimestamp + DYNAMIC_MODE_TIMEOUT_INTERVAL < millis())
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
      if (pixelModeTimeoutRunning && pixelModeTimeoutTimestamp + PIXEL_MODE_TIMEOUT_INTERVAL < millis())
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
        if (currentPixelIndex > PIXEL_ARR_MAX_INDEX)
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
      if (brightnessModeTimeoutRunning && brightnessModeTimeoutTimestamp + BRIGHTNESS_MODE_TIMEOUT_INTERVAL < millis())
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
    case CONTROL_POWER_RECV_CODE:
      Serial.write(CONTROL_POWER_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_VOL_UP_RECV_CODE:
      Serial.write(CONTROL_VOL_UP_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_FUNC_STOP_RECV_CODE:
      Serial.write(CONTROL_FUNC_STOP_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_BACKWARDS_RECV_CODE:
      Serial.write(CONTROL_BACKWARDS_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_PLAY_PAUSE_RECV_CODE:
      Serial.write(CONTROL_PLAY_PAUSE_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_FORWARD_RECV_CODE:
      Serial.write(CONTROL_FORWARD_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_DOWN_RECV_CODE:
      Serial.write(CONTROL_DOWN_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_VOL_DOWN_RECV_CODE:
      Serial.write(CONTROL_VOL_DOWN_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_UP_RECV_CODE:
      Serial.write(CONTROL_UP_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_0_RECV_CODE:
      Serial.write(CONTROL_0_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_EQ_RECV_CODE:
      Serial.write(CONTROL_EQ_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_ST_REPT_RECV_CODE:
      Serial.write(CONTROL_ST_REPT_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_1_RECV_CODE:
      Serial.write(CONTROL_1_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_2_RECV_CODE:
      Serial.write(CONTROL_2_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_3_RECV_CODE:
      Serial.write(CONTROL_3_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_4_RECV_CODE:
      Serial.write(CONTROL_4_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_5_RECV_CODE:
      Serial.write(CONTROL_5_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_6_RECV_CODE:
      Serial.write(CONTROL_6_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_7_RECV_CODE:
      Serial.write(CONTROL_7_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_8_RECV_CODE:
      Serial.write(CONTROL_8_SEND_CODE);
      prevControlIsUsed = false;
      break;
    case CONTROL_9_RECV_CODE:
      Serial.write(CONTROL_9_SEND_CODE);
      prevControlIsUsed = false;
      break;

    // control2: Led Control ON Check
    case CONTROL2_POWER_ON_RECV_CODE:
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
        case CONTROL2_BRIGHTNESS_UP_RECV_CODE:
          if (currentControlLedMode == CONTROL_LED_MODE::COLOR)
          {
            if (currentControlBrightness <= 255 - 10)
              currentControlBrightness += 10;
            FastLED.setBrightness(currentControlBrightness);
            FastLED.show();
          }
          break;
        case CONTROL2_BRIGHTNESS_DOWN_RECV_CODE:
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
      case CONTROL2_POWER_OFF_RECV_CODE:
        FastLED.setBrightness(0);
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(0, 0, 0);
        }
        FastLED.show();
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_BRIGHTNESS_UP_RECV_CODE:
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
      case CONTROL2_BRIGHTNESS_DOWN_RECV_CODE:
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
      case CONTROL2_R_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(255, 0, 0);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_R1_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(247, 70, 5);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_R2_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(247, 110, 5);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_R3_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(247, 154, 5);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_R4_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(247, 211, 5);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_G_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(0, 255, 0);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_G1_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(55, 222, 64);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_G2_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(55, 222, 139);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_G3_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(55, 222, 219);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_G4_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(0, 166, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_B_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(0, 0, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_B1_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(0, 110, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_B2_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(76, 0, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_B3_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(130, 0, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_B4_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(255, 0, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_W_RECV_CODE:
        for (int i = 0; i < NUM_LEDS; i++)
        {
          leds[i] = CRGB(255, 255, 255);
        }
        FastLED.show();
        prevControlIsUsed = false;
        currentControlLedMode = CONTROL_LED_MODE::COLOR;
        break;
      case CONTROL2_FADE_RECV_CODE:
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

