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

| Data name                  | Offset | Data type                | Size in bytes |
|----------------------------|--------|--------------------------|---------------|
| Marker (`__ddstats__\0`)   |      0 | String                   | 12            |
| Format version             |     12 | 32-bit integer           | 4             |
| Player ID                  |     16 | 32-bit integer           | 4             |
| Player name                |     20 | String                   | 32            |
| Timer time                 |     52 | 32-bit floating point    | 4             |
| Gems collected             |     56 | 32-bit integer           | 4             |
| Enemies killed             |     60 | 32-bit integer           | 4             |
| Daggers fired              |     64 | 32-bit integer           | 4             |
| Daggers hit                |     68 | 32-bit integer           | 4             |
| Enemies alive              |     72 | 32-bit integer           | 4             |
| Level gems                 |     76 | 32-bit integer           | 4             |
| Homing daggers             |     80 | 32-bit integer           | 4             |
| Gems despawned             |     84 | 32-bit integer           | 4             |
| Gems eaten                 |     88 | 32-bit integer           | 4             |
| Gems total                 |     92 | 32-bit integer           | 4             |
| Homing daggers eaten       |     96 | 32-bit integer           | 4             |
| Skull Is alive             |    100 | 16-bit integer           | 2             |
| Skull IIs alive            |    102 | 16-bit integer           | 2             |
| Skull IIIs alive           |    104 | 16-bit integer           | 2             |
| Spiderlings alive          |    106 | 16-bit integer           | 2             |
| Skull IVs alive            |    108 | 16-bit integer           | 2             |
| Squid Is alive             |    110 | 16-bit integer           | 2             |
| Squid IIs alive            |    112 | 16-bit integer           | 2             |
| Squid IIIs alive           |    114 | 16-bit integer           | 2             |
| Centipedes alive           |    116 | 16-bit integer           | 2             |
| Gigapedes alive            |    118 | 16-bit integer           | 2             |
| Spider Is alive            |    120 | 16-bit integer           | 2             |
| Spider IIs alive           |    122 | 16-bit integer           | 2             |
| Leviathans alive           |    124 | 16-bit integer           | 2             |
| Orbs alive                 |    126 | 16-bit integer           | 2             |
| Thorns alive               |    128 | 16-bit integer           | 2             |
| Ghostpedes alive           |    130 | 16-bit integer           | 2             |
| Spider eggs alive          |    132 | 16-bit integer           | 2             |
| Skull Is killed            |    134 | 16-bit integer           | 2             |
| Skull IIs killed           |    136 | 16-bit integer           | 2             |
| Skull IIIs killed          |    138 | 16-bit integer           | 2             |
| Spiderlings killed         |    140 | 16-bit integer           | 2             |
| Skull IVs killed           |    142 | 16-bit integer           | 2             |
| Squid Is killed            |    144 | 16-bit integer           | 2             |
| Squid IIs killed           |    146 | 16-bit integer           | 2             |
| Squid IIIs killed          |    148 | 16-bit integer           | 2             |
| Centipedes killed          |    150 | 16-bit integer           | 2             |
| Gigapedes killed           |    152 | 16-bit integer           | 2             |
| Spider Is killed           |    154 | 16-bit integer           | 2             |
| Spider IIs killed          |    156 | 16-bit integer           | 2             |
| Leviathans killed          |    158 | 16-bit integer           | 2             |
| Orbs killed                |    160 | 16-bit integer           | 2             |
| Thorns killed              |    162 | 16-bit integer           | 2             |
| Ghostpedes killed          |    164 | 16-bit integer           | 2             |
| Spider eggs killed         |    166 | 16-bit integer           | 2             |
| Is player alive            |    168 | 8-bit integer (boolean)  | 1             |
| Is replay                  |    169 | 8-bit integer (boolean)  | 1             |
| Death type                 |    170 | 8-bit integer            | 1             |
| Is in game                 |    171 | 8-bit integer (boolean)  | 1             |
| Replay player ID           |    172 | 32-bit integer           | 4             |
| Replay player name         |    176 | String                   | 32            |
| Survival hash              |    208 | MD5 hash byte array      | 16            |
| Level up time 2            |    224 | 32-bit floating point    | 4             |
| Level up time 3            |    228 | 32-bit floating point    | 4             |
| Level up time 4            |    232 | 32-bit floating point    | 4             |
| Leviathan down time        |    236 | 32-bit floating point    | 4             |
| Orb down time              |    240 | 32-bit floating point    | 4             |
| Game status                |    244 | 32-bit integer           | 4             |
| Max homing                 |    248 | 32-bit integer           | 4             |
| Max homing time            |    252 | 32-bit floating point    | 4             |
| Max enemies alive          |    256 | 32-bit integer           | 4             |
| Max enemies alive time     |    260 | 32-bit floating point    | 4             |
| Max time                   |    264 | 32-bit floating point    | 4             |
| N/A (padding)              |    268 | N/A                      | 4             |
| Graph stats base address   |    272 | 64-bit integer (pointer) | 8             |
| Graph stats count          |    280 | 32-bit integer           | 4             |
| Graph stats loaded         |    284 | 8-bit integer (boolean)  | 1             |
| N/A (padding)              |    285 | N/A                      | 3             |
| Hand level start           |    288 | 32-bit integer           | 4             |
| Homing start               |    292 | 32-bit integer           | 4             |
| Timer start                |    296 | 32-bit floating point    | 4             |
| Prohibited mods            |    300 | 8-bit integer (boolean)  | 1             |
| N/A (padding)              |    301 | N/A                      | 3             |
| Replay base address        |    304 | 64-bit integer (pointer) | 8             |
| Replay buffer length       |    312 | 32-bit integer           | 4             |
| Play replay from memory    |    316 | 8-bit integer (boolean)  | 1             |
| Game mode                  |    317 | 8-bit integer            | 1             |
| TimeAttack / Race finished |    318 | 8-bit integer (boolean)  | 1             |

## Graph stats

| Data name                  | Offset | Data type                | Size in bytes |
|----------------------------|--------|--------------------------|---------------|
| Gems collected             |      0 | 32-bit integer           | 4             |
| Enemies killed             |      4 | 32-bit integer           | 4             |
| Daggers fired              |      8 | 32-bit integer           | 4             |
| Daggers hit                |     12 | 32-bit integer           | 4             |
| Enemies alive              |     16 | 32-bit integer           | 4             |
| Level gems                 |     20 | 32-bit integer           | 4             |
| Homing daggers             |     24 | 32-bit integer           | 4             |
| Gems despawned             |     28 | 32-bit integer           | 4             |
| Gems eaten                 |     32 | 32-bit integer           | 4             |
| Gems total                 |     36 | 32-bit integer           | 4             |
| Homing daggers eaten       |     40 | 32-bit integer           | 4             |
| Skull Is alive             |     44 | 16-bit integer           | 2             |
| Skull IIs alive            |     46 | 16-bit integer           | 2             |
| Skull IIIs alive           |     48 | 16-bit integer           | 2             |
| Spiderlings alive          |     50 | 16-bit integer           | 2             |
| Skull IVs alive            |     52 | 16-bit integer           | 2             |
| Squid Is alive             |     54 | 16-bit integer           | 2             |
| Squid IIs alive            |     56 | 16-bit integer           | 2             |
| Squid IIIs alive           |     58 | 16-bit integer           | 2             |
| Centipedes alive           |     60 | 16-bit integer           | 2             |
| Gigapedes alive            |     62 | 16-bit integer           | 2             |
| Spider Is alive            |     64 | 16-bit integer           | 2             |
| Spider IIs alive           |     66 | 16-bit integer           | 2             |
| Leviathans alive           |     68 | 16-bit integer           | 2             |
| Orbs alive                 |     70 | 16-bit integer           | 2             |
| Thorns alive               |     72 | 16-bit integer           | 2             |
| Ghostpedes alive           |     74 | 16-bit integer           | 2             |
| Spider eggs alive          |     76 | 16-bit integer           | 2             |
| Skull Is killed            |     78 | 16-bit integer           | 2             |
| Skull IIs killed           |     80 | 16-bit integer           | 2             |
| Skull IIIs killed          |     82 | 16-bit integer           | 2             |
| Spiderlings killed         |     84 | 16-bit integer           | 2             |
| Skull IVs killed           |     86 | 16-bit integer           | 2             |
| Squid Is killed            |     88 | 16-bit integer           | 2             |
| Squid IIs killed           |     90 | 16-bit integer           | 2             |
| Squid IIIs killed          |     92 | 16-bit integer           | 2             |
| Centipedes killed          |     94 | 16-bit integer           | 2             |
| Gigapedes killed           |     96 | 16-bit integer           | 2             |
| Spider Is killed           |     98 | 16-bit integer           | 2             |
| Spider IIs killed          |    100 | 16-bit integer           | 2             |
| Leviathans killed          |    102 | 16-bit integer           | 2             |
| Orbs killed                |    104 | 16-bit integer           | 2             |
| Thorns killed              |    106 | 16-bit integer           | 2             |
| Ghostpedes killed          |    108 | 16-bit integer           | 2             |
| Spider eggs killed         |    110 | 16-bit integer           | 2             |

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

## Death types

| Death type  | Value |
|-------------|-------|
| FALLEN      | 0     |
| SWARMED     | 1     |
| IMPALED     | 2     |
| GORED       | 3     |
| INFESTED    | 4     |
| OPENED      | 5     |
| PURGED      | 6     |
| DESECRATED  | 7     |
| SACRIFICED  | 8     |
| EVISCERATED | 9     |
| ANNIHILATED | 10    |
| INTOXICATED | 11    |
| ENVENOMATED | 12    |
| INCARNATED  | 13    |
| DISCARNATED | 14    |
| ENTANGLED   | 15    |
| HAUNTED     | 16    |
