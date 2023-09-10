# Game memory

Devil Daggers stores various read-only data in memory so it can easily be read by external programs. The initial offset to the region can be found through Cheat Engine on Windows. The initial offset only changes when Devil Daggers is updated (though if it is a very small update the offset sometimes does not change at all).

## Finding the offset (Windows)

1. Open the latest version of Devil Daggers.
2. Open Cheat Engine.
3. Attach Devil Daggers process (dd.exe).
4. Click "Memory view".
5. Press CTRL+F or right click on the memory and click "Search memory...".
6. Select "Text".
7. Set "From" to `0`.
8. Set "To" to `7FFFFFFFFFFFFFFF`.
9. Enter `__ddstats__` in the bottom text box and click OK.
10. Select the first byte `5F` and right click on it.
11. Select "Add this address to the list".
12. Leave the address as is, leave "Type" set to "Byte", leave the "Pointer" checkbox disabled, and click OK.
13. Close the Memory Viewer window.
14. Right click the new address in the list and click "Pointer scan for this address".
15. Enable "Max different offsets per node" and set it to 2.
16. Set "Maximum offset value" to a large number such as 40000000.
17. Set "Max level" to 2.
18. Click OK and save the file somewhere (you probably won't be needing it again).
19. A couple results will show up after the scan has completed. The correct one should be the first one with Offset 0 set to 0 and Offset 1 being empty. The address needed is the hexadecimal number in the Base Address, after "dd.exe+", for example `00256DC0`.
20. Should the offset be updated in the DevilDaggers.info database, make sure to convert this number to its decimal representation. `00256DC0` would become `2452928`. You can do this using an online tool, or using Windows Calculator by going into the "Programmer" tab, entering the value into the HEX field, and then copying the converted value in the DEC field.

## Main

| Data name                    | Offset | Data type       | Size in bytes |
|------------------------------|--------|-----------------|---------------|
| Marker (`__ddstats__\0`)     |      0 | string          | 12            |
| Format version               |     12 | int32           | 4             |
| Player ID                    |     16 | int32           | 4             |
| Player name                  |     20 | string          | 32            |
| Timer time                   |     52 | float32         | 4             |
| Gems collected               |     56 | int32           | 4             |
| Enemies killed               |     60 | int32           | 4             |
| Daggers fired                |     64 | int32           | 4             |
| Daggers hit                  |     68 | int32           | 4             |
| Enemies alive                |     72 | int32           | 4             |
| Level gems                   |     76 | int32           | 4             |
| Homing daggers               |     80 | int32           | 4             |
| Gems despawned               |     84 | int32           | 4             |
| Gems eaten                   |     88 | int32           | 4             |
| Gems total                   |     92 | int32           | 4             |
| Homing daggers eaten         |     96 | int32           | 4             |
| Skull Is alive               |    100 | uint16          | 2             |
| Skull IIs alive              |    102 | uint16          | 2             |
| Skull IIIs alive             |    104 | uint16          | 2             |
| Spiderlings alive            |    106 | uint16          | 2             |
| Skull IVs alive              |    108 | uint16          | 2             |
| Squid Is alive               |    110 | uint16          | 2             |
| Squid IIs alive              |    112 | uint16          | 2             |
| Squid IIIs alive             |    114 | uint16          | 2             |
| Centipedes alive             |    116 | uint16          | 2             |
| Gigapedes alive              |    118 | uint16          | 2             |
| Spider Is alive              |    120 | uint16          | 2             |
| Spider IIs alive             |    122 | uint16          | 2             |
| Leviathans alive             |    124 | uint16          | 2             |
| Orbs alive                   |    126 | uint16          | 2             |
| Thorns alive                 |    128 | uint16          | 2             |
| Ghostpedes alive             |    130 | uint16          | 2             |
| Spider eggs alive            |    132 | uint16          | 2             |
| Skull Is killed              |    134 | uint16          | 2             |
| Skull IIs killed             |    136 | uint16          | 2             |
| Skull IIIs killed            |    138 | uint16          | 2             |
| Spiderlings killed           |    140 | uint16          | 2             |
| Skull IVs killed             |    142 | uint16          | 2             |
| Squid Is killed              |    144 | uint16          | 2             |
| Squid IIs killed             |    146 | uint16          | 2             |
| Squid IIIs killed            |    148 | uint16          | 2             |
| Centipedes killed            |    150 | uint16          | 2             |
| Gigapedes killed             |    152 | uint16          | 2             |
| Spider Is killed             |    154 | uint16          | 2             |
| Spider IIs killed            |    156 | uint16          | 2             |
| Leviathans killed            |    158 | uint16          | 2             |
| Orbs killed                  |    160 | uint16          | 2             |
| Thorns killed                |    162 | uint16          | 2             |
| Ghostpedes killed            |    164 | uint16          | 2             |
| Spider eggs killed           |    166 | uint16          | 2             |
| Is player alive              |    168 | uint8 (boolean) | 1             |
| Is replay                    |    169 | uint8 (boolean) | 1             |
| [Death type](death-types.md) |    170 | uint8           | 1             |
| Is in game                   |    171 | uint8 (boolean) | 1             |
| Replay player ID             |    172 | int32           | 4             |
| Replay player name           |    176 | String          | 32            |
| Survival hash                |    208 | MD5 hash bytes  | 16            |
| Level up time 2              |    224 | float32         | 4             |
| Level up time 3              |    228 | float32         | 4             |
| Level up time 4              |    232 | float32         | 4             |
| Leviathan down time          |    236 | float32         | 4             |
| Orb down time                |    240 | float32         | 4             |
| Game status                  |    244 | int32           | 4             |
| Max homing                   |    248 | int32           | 4             |
| Max homing time              |    252 | float32         | 4             |
| Max enemies alive            |    256 | int32           | 4             |
| Max enemies alive time       |    260 | float32         | 4             |
| Max time                     |    264 | float32         | 4             |
| N/A (padding)                |    268 | N/A             | 4             |
| Graph stats base address     |    272 | int64 (pointer) | 8             |
| Graph stats count            |    280 | int32           | 4             |
| Graph stats loaded           |    284 | uint8 (boolean) | 1             |
| N/A (padding)                |    285 | N/A             | 3             |
| Hand level start             |    288 | int32           | 4             |
| Homing start                 |    292 | int32           | 4             |
| Timer start                  |    296 | float32         | 4             |
| Prohibited mods              |    300 | uint8 (boolean) | 1             |
| N/A (padding)                |    301 | N/A             | 3             |
| Replay base address          |    304 | int64 (pointer) | 8             |
| Replay buffer length         |    312 | int32           | 4             |
| Play replay from memory      |    316 | uint8 (boolean) | 1             |
| Game mode                    |    317 | uint8           | 1             |
| TimeAttack / Race finished   |    318 | uint8 (boolean) | 1             |

## Graph stats

| Data name                  | Offset | Data type | Size in bytes |
|----------------------------|--------|-----------|---------------|
| Gems collected             |      0 | int32     | 4             |
| Enemies killed             |      4 | int32     | 4             |
| Daggers fired              |      8 | int32     | 4             |
| Daggers hit                |     12 | int32     | 4             |
| Enemies alive              |     16 | int32     | 4             |
| Level gems                 |     20 | int32     | 4             |
| Homing daggers             |     24 | int32     | 4             |
| Gems despawned             |     28 | int32     | 4             |
| Gems eaten                 |     32 | int32     | 4             |
| Gems total                 |     36 | int32     | 4             |
| Homing daggers eaten       |     40 | int32     | 4             |
| Skull Is alive             |     44 | uint16    | 2             |
| Skull IIs alive            |     46 | uint16    | 2             |
| Skull IIIs alive           |     48 | uint16    | 2             |
| Spiderlings alive          |     50 | uint16    | 2             |
| Skull IVs alive            |     52 | uint16    | 2             |
| Squid Is alive             |     54 | uint16    | 2             |
| Squid IIs alive            |     56 | uint16    | 2             |
| Squid IIIs alive           |     58 | uint16    | 2             |
| Centipedes alive           |     60 | uint16    | 2             |
| Gigapedes alive            |     62 | uint16    | 2             |
| Spider Is alive            |     64 | uint16    | 2             |
| Spider IIs alive           |     66 | uint16    | 2             |
| Leviathans alive           |     68 | uint16    | 2             |
| Orbs alive                 |     70 | uint16    | 2             |
| Thorns alive               |     72 | uint16    | 2             |
| Ghostpedes alive           |     74 | uint16    | 2             |
| Spider eggs alive          |     76 | uint16    | 2             |
| Skull Is killed            |     78 | uint16    | 2             |
| Skull IIs killed           |     80 | uint16    | 2             |
| Skull IIIs killed          |     82 | uint16    | 2             |
| Spiderlings killed         |     84 | uint16    | 2             |
| Skull IVs killed           |     86 | uint16    | 2             |
| Squid Is killed            |     88 | uint16    | 2             |
| Squid IIs killed           |     90 | uint16    | 2             |
| Squid IIIs killed          |     92 | uint16    | 2             |
| Centipedes killed          |     94 | uint16    | 2             |
| Gigapedes killed           |     96 | uint16    | 2             |
| Spider Is killed           |     98 | uint16    | 2             |
| Spider IIs killed          |    100 | uint16    | 2             |
| Leviathans killed          |    102 | uint16    | 2             |
| Orbs killed                |    104 | uint16    | 2             |
| Thorns killed              |    106 | uint16    | 2             |
| Ghostpedes killed          |    108 | uint16    | 2             |
| Spider eggs killed         |    110 | uint16    | 2             |

## Game statuses

| Game status                 | Value |
|-----------------------------|-------|
| Title                       | 0     |
| Menu                        | 1     |
| Lobby                       | 2     |
| Playing                     | 3     |
| Dead                        | 4     |
| Own replay from last run    | 5     |
| Own replay from leaderboard | 6     |
| Other player's replay       | 7     |
| Local replay                | 8     |
