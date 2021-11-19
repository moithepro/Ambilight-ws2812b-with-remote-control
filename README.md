# Ambilight-ws2812b-with-remote-control  

## ws2812b Ambilight + remote control for LEDs and PC
  This repository is dedicated to document this project I made for my PC setup.
### Items used:
  **• Arduino Uno.\
  • 178 cm of 100 LEDs/m 5v ws2812b LED Strips (178 LEDs Total).\
  • 40A 5v Power Supply (to power the LEDs).\
  • KY-022 Infrared Reciever.\
  • 330 Ω Resistor\
  • And a lot of wires...**



### The project has **3** main targets:
  **Controlling the LEDs with a .NET program on the PC.**\
  **Controlling the PC with a remote control**\
  **Controlling the LEDs with a remote control.**

### Circuit Diagram:
  Below is the circuit diagram:


  ![Arduino-diagram](https://user-images.githubusercontent.com/52801196/142697124-293a43a1-f6bc-4373-9697-7a915990cf32.png)
  ![image](https://user-images.githubusercontent.com/52801196/142697209-36e55328-3a04-44e8-b19c-15cb57b13161.png)

  The LED Strip data pin is connected to digital pin 5 with a 330 ohm Resistor.\
  The Infrared Reciever Signal pin is connected to digital pin 7.
## Controlling the LEDs with a .NET program on the PC
  Using a .NET Framework WinForm Application to Control the Leds in many ways (not only Ambilight).\
  The LEDs are Controlled **by Serial Communication at 115200 Baud Rate with the Arduino** via the PC in 5 main ways.\
   **|**\
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
  The same .NET Application also checks for the Arduino Serial Port data and sends various keystrokes to the PC.

## Controlling the LEDs with a remote control.
pressing The "ON" button on the LED remote controls enables only the remote control to control the led.\
Controls include brightness control and color setting.

## Photos
![IMG_20211016_213444](https://user-images.githubusercontent.com/52801196/142470433-9fb1de0c-5dad-4057-ae4c-f377d217943b.jpg)
The remote controls:\
![1637258300903](https://user-images.githubusercontent.com/52801196/142470793-93cb5baa-1b1a-4918-9836-f59e4a962d59.jpg)
![IMG_20211019_174302](https://user-images.githubusercontent.com/52801196/142471346-73e1c083-9f02-43fc-befa-3472a0947fdf.jpg)
