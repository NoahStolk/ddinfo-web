# Spawnset binary

## Files

There are 3 files in Devil Daggers which use this format. These are "survival", "dagger", and "menu", although only "survival" makes use of all the features.

- `devildaggers/dd/dagger` contains the dagger lobby.
- `devildaggers/dd/menu` contains the menu.
- `devildaggers/dd/survival` contains the game.
	
## Format

The internal structure of spawnset binaries consists of 5 parts:

| Name                 | Size in bytes                   |
|----------------------|---------------------------------|
| Header buffer        | 36                              |
| Arena buffer         | 10404                           |
| Spawns header buffer | 36 or 40 depending on version   |
| Spawns buffer        | 28 x the amount of spawns       |
| Settings buffer      | 0, 5, or 9 depending on version |

### Overview of known values

- Header buffer
	- Versions
	- Shrinking controls
	- Brightness
	- Game mode
- Arena buffer
	- For every tile:
		- Tile height
- Spawns header buffer
	- Race dagger position
	- Dagger unlock times
	- Spawn count
- Spawns buffer
	- For every spawn:
		- Spawn enemy type
		- Spawn delay
- Settings buffer
	- Initial hand
	- Additional gems
	- Timer start

### Header buffer

Fixed-length buffer of 36 bytes. Contains shrinking control and brightness values as well as presumably a version number and some unknown miscellaneous values that usually cause the game to crash or behave oddly when modified.

The header buffer for the default spawnset looks like this:

| Binary (hex) | Data type | Meaning                         | Value |
|--------------|-----------|---------------------------------|-------|
| 04000000     | int32     | Spawn version                   | 4     |
| 09000000     | int32     | World version                   | 9     |
| 0000A041     | float32   | Shrink end radius               | 20    |
| 01004842     | float32   | Shrink start radius             | 50    |
| CDCCCC3C     | float32   | Shrink rate                     | 0.025 |
| 00007042     | float32   | Brightness                      | 60    |
| 00000000     | int32     | Game mode                       | 0     |
| 33000000     | int32     | Arena dimension*                | 51    |
| 01000000     | ?         | ?                               | ?     |

* Only 51 as arena dimension works correctly. Other values cause the tiles to render incorrectly and replays will glitch out and sometimes crash. The game will also instantly crash when loading a spawnset with a dimension of 52 or higher.

World version is 8 for V1, and 9 for all later versions.

Spawn version is always 4 except when using settings buffer which was added in V3.1. Spawn version 5 adds support for initial hand upgrade and additional gems. Spawn version 6 adds support for timer start.

### Arena buffer

Fixed-length one-dimensional array of 2601 (51 x 51 = 2601 tiles) float32s (2601 x 32 / 8 = 10404 bytes) representing the height of each tile in the arena.

### Spawns header buffer

Fixed-length buffer of 40 bytes (or 36 when world version is 8). Contains the amount of spawns and some other values.

The spawns header buffer for the default spawnset looks like this:

| Binary (hex) | Data type | Meaning                            | Value |
|--------------|-----------|------------------------------------|-------|
| 00000000     | float32   | Race game mode dagger position X   | 0     |
| 00000000     | float32   | Race game mode dagger position Z   | 0     |
| 00000000     | ?         | ?                                  | ?     |
| 01000000     | ?         | ?                                  | ?     |
| F4010000     | int32     | Devil dagger unlock time (unused)  | 500   |
| FA000000     | int32     | Golden dagger unlock time (unused) | 250   |
| 78000000     | int32     | Silver dagger unlock time (unused) | 120   |
| 3C000000     | int32     | Bronze dagger unlock time (unused) | 60    |
| 00000000     | ?         | ?                                  | ?     |
| 76000000     | int32     | Spawn count                        | 118   |

### Spawns buffer

This is the only part of the file with a variable length. It represents the list of spawns. Each spawn buffer consists of 28 bytes that include the enemy type as a int32 and the delay value as a float32. The other bytes in each of the spawn buffers seem to be the same for all of them and appear to have no meaning.

These are the first 3 spawns in the original game:

| Binary (hex) | Data type | Meaning     | Value |
|--------------|-----------|-------------|-------|
| 00000000     | int32     | Enemy type  | 0     |
| 00004040     | float32   | Spawn delay | 3     |
| 00000000     | ?         | ?           | ?     |
| 03000000     | ?         | ?           | ?     |
| 00000000     | ?         | ?           | ?     |
| 0000F041     | ?         | ?           | ?     |
| 0A000000     | ?         | ?           | ?     |

| Binary (hex) | Data type | Meaning     | Value |
|--------------|-----------|-------------|-------|
| FFFFFFFF     | int32     | Enemy type  | -1    |
| 0000C040     | float32   | Spawn delay | 6     |
| 00000000     | ?         | ?           | ?     |
| 03000000     | ?         | ?           | ?     |
| 00000000     | ?         | ?           | ?     |
| 0000F041     | ?         | ?           | ?     |
| 0A000000     | ?         | ?           | ?     |

| Binary (hex) | Data type | Meaning     | Value |
|--------------|-----------|-------------|-------|
| 00000000     | int32     | Enemy type  | 0     |
| 0000A040     | float32   | Spawn delay | 5     |
| 00000000     | ?         | ?           | ?     |
| 03000000     | ?         | ?           | ?     |
| 00000000     | ?         | ?           | ?     |
| 0000F041     | ?         | ?           | ?     |
| 0A000000     | ?         | ?           | ?     |

### Enemy types

Here's the list of enemy types that the survival file defines:

#### V3 / V3.1 / V3.2 (current)

| Binary (hex) | Data type | Meaning   | Value |
|--------------|-----------|-----------|-------|
| 00000000     | int32     | Squid I   | 0     |
| 01000000     | int32     | Squid II  | 1     |
| 02000000     | int32     | Centipede | 2     |
| 03000000     | int32     | Spider I  | 3     |
| 04000000     | int32     | Leviathan | 4     |
| 05000000     | int32     | Gigapede  | 5     |
| 06000000     | int32     | Squid III | 6     |
| 07000000     | int32     | Thorn     | 7     |
| 08000000     | int32     | Spider II | 8     |
| 09000000     | int32     | Ghostpede | 9     |
| FFFFFFFF     | int32     | Empty     | -1    |

#### V2

| Binary (hex) | Data type | Meaning   | Value |
|--------------|-----------|-----------|-------|
| 00000000     | int32     | Squid I   | 0     |
| 01000000     | int32     | Squid II  | 1     |
| 02000000     | int32     | Centipede | 2     |
| 03000000     | int32     | Spider I  | 3     |
| 04000000     | int32     | Leviathan | 4     |
| 05000000     | int32     | Gigapede  | 5     |
| 06000000     | int32     | Squid III | 6     |
| 07000000     | int32     | Andras    | 7     |
| 08000000     | int32     | Spider II | 8     |
| FFFFFFFF     | int32     | Empty     | -1    |

#### V1

| Binary (hex) | Data type | Meaning   | Value |
|--------------|-----------|-----------|-------|
| 00000000     | int32     | Squid I   | 0     |
| 01000000     | int32     | Squid II  | 1     |
| 02000000     | int32     | Centipede | 2     |
| 03000000     | int32     | Spider I  | 3     |
| 04000000     | int32     | Leviathan | 4     |
| 05000000     | int32     | Gigapede  | 5     |
| FFFFFFFF     | int32     | Empty     | -1    |

### Settings buffer

Fixed-length buffer of 9 bytes. It was added to the game's V3.1 update which released on February 2021, specifically for practice and modding purposes, and is not used in the default spawnset. It only works when the header's spawn version is 5 or 6. The last value, timer start, only works on spawn version 6.

| Binary (hex) | Data type | Meaning              | Value |
|--------------|-----------|----------------------|-------|
| 04           | uint8     | Initial hand upgrade | 4     |
| 05000000     | int32     | Additional gems      | 5     |
| 0000A041     | float32   | Timer start          | 20    |
