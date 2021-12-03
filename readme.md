A* Pathfinder in Unity
=

## Camera Controls

WASD - Move forward/back and strafe
Q/E - Move up/down

## Mouse Controls

Right click in a tile to place the NPC, click on another tile to set the destination

## Keyboard Controls

Enter - Start pathfinding
Backspace - Pause pathfinding (new destination can not be set/will be ignored)
Keypad plus - Slow down update speed
Keypad minus - Speed up update speed

## World Generator Settings

Recommend size for `Valley` Generation Type: X = 40, Z = 80

Grid Size X - Grid Width
Grid Size Z - Grid Depth
Traversal Multiplier - Increase to try to avoid changing height when pathfinding
Generation Type - Flat, Noisy, and Valley terrain will be generated at runtime
Selection behaviour - Normal is normal, Debug is to verify that neighbour nodes are being properly considered
Update Change Rate - Modifier for Keypad plus/minus keys
Update Speed - higher number = increase time before pathfinder update

## Other

Running in debug will log out update time to the screen, bear in mind this will be affected by artifically limiting the update speed.

