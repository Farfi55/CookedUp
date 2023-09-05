# CookedUp

Unity Game inspired by Overcooked and PlateUp! with Bots AI using ASP

## About

This game builds upon this fantastic [free youtube course](https://www.youtube.com/watch?v=AmGSEH7QcDg) by [Code Monkey](https://www.youtube.com/@CodeMonkeyUnity)

It is my **thesis project** for my bachelor's degree, which focuses on creating a **Bot AI** using **[Answer Set Programming](https://www.wikiwand.com/en/Answer_set_programming)** (ASP) to play this game.

More specifically, I'm using the [ThinkEngine](https://github.com/DeMaCS-UNICAL/ThinkEngine) Framework to facilitate the communication between Unity and the ASP solvers it supports ([DLV](https://dlv.demacs.unical.it/home) and [clingo](https://github.com/potassco/clingo))

## Gameplay Preview

[![CookedUp Gameplay with 2 Bots](https://img.youtube.com/vi/D0zVua0PdVo/0.jpg)](https://www.youtube.com/watch?v=D0zVua0PdVo)

## How to play

### Download

You can download the latest game relase from the [releases page](https://github.com/Farfi55/CookedUp/releases) and run it.

Or you can build the game yourself (see [How to build](#how-to-build)) to get the latest version.

### Goal

The goal of the game is to cook and serve as many dishes requests as possible in the given time.

### Gameplay

You play as a chef in a restaurant kitchen, throughout the game you will have to cook recipes for your customers.

To prepare a dish, you will have to pick up the required ingredients, cut or cook them if necessary, and place them on a plate.

Be careful not to burn the ingredients, or you will have to throw them away and start over.

Once you have all the ingredients on the plate, you can deliver it to the customer by placing it on the delivery counter (the counter with the moving arrow).

Requests have a time limit, so you will have to be quick to deliver them before they expire.

*See what score can you get before the time runs out!*

### Controls

On the keyboard:

- **WASD** or **Arrow Keys** to move
- **Space** or **E** or **P** to interact with objects
- **F** or **L** to cut ingredients
- **ESC** to pause the game

On a xbox controller (most other controllers should work as well):

- **Left Stick** to move
- **A button** to interact with objects
- **X button** to cut ingredients
- **Select button** to pause the game

## How to build

### Requirements

- Unity 2022.3 or newer *(tested with 2022.3.8 on Windows 10 and Linux)*

### Steps

1. Clone this repository ([github docs](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository))
2. Open the project in Unity through Unity Hub
3. Open the scene `Assets/Scenes/GameScene.unity`
4. Build the project (File -> Build Settings -> Build) and select a folder to build to
5. Play the game by running the executable in the build folder

optionally, you can also run the game directly from Unity by pressing the play button in the top center of the screen

## Bot AI logic

The Bot AI is implemented as a finite state machine, with the following states:

- Pick up Plate
- Place Plate
- Get Ingredients
- Ingredient Burning
- Ingredient Left on CuttingCounter
- Drop Ingredient
- Pick up completed Plate
- Deliver
- Recipe failed

![Bot AI logic](.github/img/CookedUp_Bot_Logic-combined.drawio.png)

## Screenshots

| ![screenshot lobby](.github/img/Screenshot_lobby.png) | ![screenshot gameplay 1](.github/img/Screenshot_gameplay_1.png) |
| ------------------------------------------------------ | ---------------------------------------------------------------- |
| ![screenshot gameplay 2](.github/img/Screenshot_gameplay_2.png) | ![screenshot gameplay 3](.github/img/Screenshot_gameplay_3.png) |
| ![screenshot game over](.github/img/Screenshot_gameplay_end.png) |
