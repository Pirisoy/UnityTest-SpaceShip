# Playe Soft Test

This project is getting developed on Unity 2020.3.21f1.

We have 2 scenes. MainMenu and GamePlay.\
MainMenu is simple, it's just for entering GamePlay.\
It has a MainMenuController to handle this event.\
\
\
In the GamePlay scene, we have some Managers, like GameManager, EnemySpawner, SoundManager.\
GameManager handles most of the events, like changing UI, handling hp and scores. We could separate these tasks in the future.\
EnemySpawner handles the spawning of enemies. For now, it's just a random enemy at random times. EnemySpawner has a pool for enemies to reduce total instantiates.\
SoundManager handles the sounds of the game.\
\
\
We have a Ship class. It represents a ship! every ship can have 1 or more weapons.\
The PlayerShip class is a child of the ship class. It is just a normal ship but can move with player input that comes from GameManager.\
Every weapon has its pool of bullets. That's because we may have different bullet prefabs for every different weapon.\
We have an Asteroid class. It spawns in a random place and moves toward a random place. It may be one of three types(big, medium, small) and may have some children that spawn on its death.\
\
\
We have a folder in Assets named Data. It contains some scriptable objects for changing the data of the game.\
We have 1 object for each asteroid that contains its data, like speed, score, type, child type.\
We have an object for ship data that contains data like movement speed, rotation speed.\
We have an object for weapon data that contains data like bullet force and fire rate. (In the future, each weapon may have its damage and each asteroid has a different hp).\
We have an object for GameData, it contains some data like max life and min/max time between each enemy spawn.\
