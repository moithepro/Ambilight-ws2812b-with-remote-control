
# Ambilight WS2812B with Remote Control

## Overview
This repository documents a project integrating WS2812B LEDs with remote control functionality for an enhanced PC setup experience.

## Components Used
- **Arduino Uno (or any compatible microcontroller)**
- **178 cm of WS2812B LED strips, with 100 LEDs/m (178 LEDs in total)**
- **5V 40A power supply (to power the LEDs)**
- **KY-022 infrared receiver**
- **330Ω resistor**
- **Wiring and connectors as necessary**

## Project Objectives
The project is designed to achieve the following three main objectives:
1. **LED control via a .NET application on the PC**
2. **PC control using a remote control**
3. **LED control using a remote control**

## Circuit Diagram
The circuit diagram is provided below for reference:

![Arduino Circuit Diagram](https://user-images.githubusercontent.com/52801196/142697124-293a43a1-f6bc-4373-9697-7a915990cf32.png)
![Image](https://user-images.githubusercontent.com/52801196/142697209-36e55328-3a04-44e8-b19c-15cb57b13161.png)

### Key Connections:
- **LED Strip**: Data pin connected to digital pin 5 via a 330Ω resistor.
- **Infrared Receiver**: Signal pin connected to digital pin 7.

## Controlling LEDs via a .NET Program (Java port also available with limited features)
The LED control is managed through a .NET Framework WinForm application, which provides various methods of control beyond just Ambilight. Communication between the PC and Arduino is handled via serial communication, with a baud rate of 250,000 (500,000 for ESP32). The control modes include:

- **Individual Colors**: Choose from a color palette.
- **Rainbow Mode**: Continuously change colors by adjusting the hue.
- **Party Mode**: A dynamic, continuously changing color mode.
- **Ambilight**: Screen pixel data is used to update the LEDs to match the colors on the display. This is the most impressive mode.
- **Spectrogram Mode**: Adjusts the LED brightness based on the audio levels from the system’s audio device. (A reset option for the sound device is available.)
  
Additional features include:
- **Turbo Mode**: Reduces LED update times for faster responsiveness, though this may cause flickering due to hardware limitations.
- **Brightness Control**: Adjustable via a slider in the application.

## Controlling the PC via Remote Control
The .NET application can also read data from the Arduino’s serial port, which allows the remote control to trigger various PC keystrokes.

## Controlling the LEDs via Remote Control
By pressing the "ON" button on the LED remote, control of the LEDs is transferred exclusively to the remote. Functions include brightness adjustment and color selection.

## Supporting the Project
The best way to support this project is by reporting bugs and suggesting new features.

## Photos
Here are some images showcasing the setup and the remote controls:

![Project Setup](https://user-images.githubusercontent.com/52801196/142470433-9fb1de0c-5dad-4057-ae4c-f377d217943b.jpg)
![Remote Controls](https://user-images.githubusercontent.com/52801196/142470793-93cb5baa-1b1a-4918-9836-f59e4a962d59.jpg)
![Setup Detail](https://user-images.githubusercontent.com/52801196/142471346-73e1c083-9f02-43fc-befa-3472a0947fdf.jpg)
