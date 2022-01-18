# RacingAR
This is a project that I have implemented as my graduation project from Gebze Technical University. It is intended to be a multiplayer AR game but time constraints pushed me into converting into a singleplayer game to be able to have a content to show at the end of the semester. An example of the gameplay can bee seen in [this](https://youtu.be/9_NHl5WVpcc) trailer video.

# Known Issues
There are a few issues that I am still not able to resolve, please bear with them while testing or please get in contact with me if you do have an insight about how it can be solved/improved
- Playing Again
  - Getting into GameScene a second time (after returning back to Main Menu when game ends) cause the AR Session to.. get messed? I don't know what exactly is the issue here. I'm not able to clean it up properly. So it's needed to close the game and start again if you want to play again

- Road Generation 
  - It is sometimes messy. It can rotate in a way that you don't want to or can't drive the car with since it's twisted
  
- Car Movement
  - The car is also slipping from the surface sometimes. I'm not sure how should I be preventing this. Car is implemented with wheel colliders, so something related to this?
  - Also there can be some bumps or curves that car is simply not able to move. It's probably due to it being a sports car and the car body collider touching with road surface, causing wheels to spin for nothing. Maybe I should've chosen an off road car..
  
  
# Requirements
I've used Unity version 2020.3.22f1 and Lightship version 1.0.1. If you want to play/edit the game yourself in your Unity editor there isn't any external libraries needed. Everything I have used external is inside the ThirdParty folder. Having Android build support on Unity (and setting them in Preferences/External Tools) should be fine for you to build your own APK if needed to. Otherwise you can get the latest APK build inside the Build folder in this repo.
