# HardPong

![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![MonoGame](https://img.shields.io/badge/MonoGame-3.8.2-E73C00)
![Windows](https://img.shields.io/badge/Windows-0078D6?logo=windows&logoColor=white)
![Linux](https://img.shields.io/badge/Linux-FCC624?logo=linux&logoColor=black)
![macOS](https://img.shields.io/badge/macOS-000000?logo=apple&logoColor=white)
![Android](https://img.shields.io/badge/Android-3DDC84?logo=android&logoColor=white)
![iOS](https://img.shields.io/badge/iOS-000000?logo=ios&logoColor=white)

A classic Pong game developed on **C#** with **MonoGame**, built around small, testable pieces:
input becomes explicit actions, a match session owns the rules, and the screens handle presentation and navigation requests.

![image](https://github.com/user-attachments/assets/71dae815-c7bb-4219-8a19-0ceaa67c40f3)

## How to play

First player to reach **10 points** wins the match.

| Action | Player 1 | Player 2 |
|-----------------------------|-----------|------------|
| Move paddle | `W` / `S` | `↑` / `↓` |
| Start / pause / resume | `ENTER` | `ENTER` |
| Open pause menu | `ESC` | `ESC` |

In the pause menu: `CONTINUE`, `MAIN MENU` or `EXIT GAME`, navigating with `↑` / `↓` and confirming with `ENTER`.

## How to run

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
dotnet run --project HardPong
```

## Tests

The match logic (state machine, score, physics, input mapping) is covered by unit tests
that run without opening a window or loading any assets.

```bash
dotnet test
```

## Project structure

```text
HardPong/
├── Screens/     # MainMenuScreen, GameplayScreen (build, draw, navigate)
├── Gameplay/    # MatchSession, MatchScore, GameStateController, entities and controllers
├── Input/       # keyboard reading translated into GameAction
├── Physics/     # pure collision detection and the collision responses
├── Rendering/   # SpriteRenderer
├── UI/
│   ├── Menus/   # menu presentation, selection, results and selection arrow
│   ├── Effects/ # color, scale and position effects
│   └── Sprites/ # sprites and movement strategies
└── Audio/       # match audio owner
```

The flow is one-directional: input is read into `GameAction`s, `MatchSession` executes them
and simulates frames, and the screens render the entities' state. Physics never plays sounds
and never draws.

`MatchSession.SimulateFrame` returns `CollisionEvents` for paddle hits, wall hits and points.
`GameplayScreen` passes these facts to `PongAudio` for playback and stops the score sound
when a round or match is reset. Session tests require no audio objects or audio mocks.

`GameScreen` separates resource loading from activation: `Initialize` loads content once,
`Enter` prepares each visit, and `Leave` stops playback and rearms input without releasing resources.
Entering gameplay starts a fresh match. `PongGame` disposes both screens at shutdown,
including the inactive screen, before MonoGame releases shared content and the graphics device.
