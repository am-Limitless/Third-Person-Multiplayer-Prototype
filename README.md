# Third-Person-Multiplayer-Prototype

Overview

This is a third-person multiplayer game designed to showcase basic player movement and interaction mechanics. Players can walk, run, jump, and interact with a sphere to change its color, which is synchronized across all connected clients. The game is developed using Unity and utilizes the Mirror Networking framework for multiplayer functionality. The current build is made for PC platforms and can be downloaded for play.

Features

Player Movement: Players can walk, run, and jump to explore the game world.

Interactive Sphere: Players can interact with a sphere to change its color. The color change is synchronized across all clients in real-time.

Multiplayer Support: Powered by Mirror Networking, the game supports multiple players interacting in the same session.

Cross-Client Synchronization: Any changes to the sphere's color are reflected across all connected players.

System Requirements

Operating System: Windows 10 or higher (64-bit recommended)

Processor: Intel Core i3 or equivalent

Memory: 4 GB RAM

Graphics: Integrated or dedicated graphics card with DirectX 11 support

Storage: 500 MB available space

Installation Instructions

Download the Game:

Download the game files from the provided link.

Extract the Files:

Extract the downloaded ZIP file to a location on your PC.

Run the Game:

Double-click on MultiplayerGame.exe to launch the game.

How to Play

Start the Game:

Launch the game by running MultiplayerGame.exe.

Host a Game:

Select the "Host" option to create a multiplayer session.

Join a Game:

Enter the host's IP address and select "Join" to connect to an existing session.

Control Your Character:

Use the following controls to move and interact:

W, A, S, D: Move the character

Spacebar: Jump

Shift (Hold): Run

E: Interact with the sphere

Interact with the Sphere:

Approach the sphere and press E to change its color.

Networking Setup

Ensure all players are on the same local network or use port forwarding for online play.

The host must share their IP address with other players to allow them to join the session.

Known Issues

Latency: Minor delays in color synchronization may occur on slower networks.

Collision Glitches: Occasionally, players may experience minor clipping when jumping near walls.

Future Enhancements

Add support for mobile platforms, including Android.

Implement additional interaction mechanics and objects.

Expand the game world with more detailed environments.

Credits

Development: Gokul Surendran

Networking Framework: Mirror Networking

Engine: Unity
