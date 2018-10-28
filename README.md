# Bot RoC

This project aims to automate tasks for the android game Rise of Civilization (RoC).  
Warning : Lilith games has an anti-bot policy. I'm not responsible if you get caught and banned from the game.  
Although this bot should not be detected, as it uses only standard inputs from adb (unless the new anti-bot system uses captcha).  

It's basically a program that can connect to the Memu android emulator find and tap on images.  
It has been abandonned. Feel free to contribute or fork.  
What it can actually do :
* Collect resources
* Explore
* Collect gifts from tribal villages

## Getting Started

To get this project up and running, just build it with Visual Studio.

### Prerequisites

This project is only compatible with Memu using the 1920*1200 resolution so far.
The game should already been installed on Memu.
All the buildings that should be interacted with should be on the default screen (tapping on the home button on the bottom left twice).

## Built With NuGet

* [log4net](http://logging.apache.org/log4net/) - Logging actions
* [SharpAdbClient](https://github.com/quamotion/madb) - Framework used to interact with the Android emulator
* [Tesseract](https://github.com/charlesw/tesseract) - OCR used (not used actually)

## Authors

* **AZn5ReD** - *Initial work* - [BotRoC](https://github.com/AZn5ReD/BotRoC)

## Still TODO

* Automatic build
* Automatic troop creation
* Automatic collect of rewards from messages
* Many more...
