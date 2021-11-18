# Ambilight-ws2812b-with-remote-control  

## ws2812b Ambilight + remote control for LEDs and PC
  This repository is dedicated to document this project I made for my PC setup.
### Items used:
  **Arduino Uno.\
  178 cm of 100 LEDs/m 5v ws2812b LED Strips (178 LEDs Total).\
  40A 5v Power Supply (to power the LEDs).\
  KY-022 Infrared Reciever.\
  330 Ω Resistor\
  And a lot of wires...**


### The project has **3** main targets:
  **Controlling the LEDs with a .NET program on the PC.**\
  **Controlling the PC with a remote control**\
  **Controlling the LEDs with a remote control.**

### Circuit Diagram:
  Below is the circuit diagram:



  ![Arduino-diagram](https://user-images.githubusercontent.com/52801196/142459418-9de6cd77-dacb-47a4-a21b-45125d318c4c.png)
  The LED Strip data pin is connected to digital pin 5 with a 330 ohm Resistor.\
  The Infrared Reciever Signal pin is connected to digital pin 7.
## Controlling the LEDs with a .NET program on the PC
  Using a .NET Framework WinForm Application to Control the Leds in many ways (not only Ambilight).\
  The LEDs are Controlled **by Serial Communication at 115200 Baud Rate with the Arduino** via the PC in 5 main ways.\
  **-->** The LED Control Operations are executed on the main GUI Thread. (Which might block it but it was the best working option.)\
  **Individual Colors** - Lets you Choose from a Color Palatte.\
  **Rainbow** - Changing Colors by continuously incrementing the hue property of the color.\
  **Party** - Continuously Changing Colors to simulate Party Lights.\
  **Ambilight** - Taking screen pixel data to change the LEDs to colors displayed on the screen. (Best Mode).\
  **Spectogram** - Taking audio device audio level to update the LEDs Brightness According to the current Level (Reset sound device option at rightmost top corner). \
   \
  **Turbo** - Lowering LEDs Update times so it will update faster. (May cause flickering due to hardware limitations)\
  **There is also a Brightness slider Control**

## Controlling the PC with a remote control
