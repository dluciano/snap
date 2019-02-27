# Snap
The Snap Card Game

Foders:
- Clients: it contains three projects. First, the console app, then a REST API project that has some endpoints to get the room, players of the system and lastly the game itself WIP (work in progress)
- The core: it contains the logic of the game.
- Gamesharp: a library to manage game rooms and player turns. WIP
- Dawlin: other library to common utilities. (A state machihne, some helpers)
- Test: Unit/Integration testing of the core and the Gamesharp library (it will be divided later)

The projects contain an abstract and impl projects, it is just a division to group abstract and not abstract objects. Other difference is that the Impl projects have a *Module.cs file that initilize a Autofac module.
