# KIXEYE Unity/C# Coding Challenge

## Project Instruction

* Use a git client to clone the repo.
* Project use Unity 5.4.1 (other minor patches of Unity 5.4 should work without any problems).

## Gameplay configuration Instructions

Within the Main.unity scene, you should see a GameManager object. Through the Inspector window, you can edit those values of the game:
* Obstacle spawn frequency.
* Obstacle location's vertical and horizontal offset, which affect directly to obstacles' distance to each other.
* The possibility of spawning a particular type of obstacle. The higher value means that this type of obstacle _may_ be spawned more frequently than another type.
* Time scale progression. Endless runner games usually have higher tempo after a while of playing.

Also in this scene, you can configure player's run speed and jump height.

## Assumptions

* I assume there can be any number of obstacles, as long as there is random factor to ensure varied challenge. I provided a script to modify shapes and height of the obstacles at runtime, randomly.
* I assume that I do not have to implement a button for restart the game, as the requirements file does not specify it.
* I assume that I do not have to implement Leaderboard Service in case I do not have prior experience related to HTTP REST API
