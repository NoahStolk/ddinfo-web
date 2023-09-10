# Replay events

Either a [local replay binary](local-replay-binary.md) or a [replay hosted on the Devil Daggers leaderboard servers](leaderboard-replay-binary.md) contains replay events at the end of the data.

Data is compressed using ZLIB. In order to get raw event data, make sure to:

1. Skip the first 2 bytes (ZLIB header).
2. Inflate (decompress ZLIB) the rest of the data.

### <a id="entity-ids"></a>Entity IDs ###

Events work with entity IDs, which are integer values. An entity in Devil Daggers is an enemy or a dagger. Every entity has its own ID which can be referenced to from other events. The entity IDs are not stored, but can be defined by reading the [spawn events](#spawn-events) in order. The first entity spawning in the replay will have ID 1, the second entity will have ID 2, etc. Entity ID 0 is reserved for miscellaneous purposes, such as the player (used in the hit event when the player dies) or the void (when a dagger disappears).

## Raw replay events

Every event starts with an event type, which is a single byte. After that, additional data follows which differs in length per event type.

### Event types

| Event type                                            | Value | Size (excluding event type byte itself)     |
|-------------------------------------------------------|-------|---------------------------------------------|
| [Spawn event](#spawn-events)                          | 0x00  | Variable, see [entity types](#entity-types) |
| [Entity position event](#entity-position-event)       | 0x01  | 10                                          |
| [Entity orientation event](#entity-orientation-event) | 0x02  | 22                                          |
| [Entity target event](#entity-target-event)           | 0x04  | 10                                          |
| [Hit event](#hit-event)                               | 0x05  | 12                                          |
| Gem event                                             | 0x06  | 0                                           |
| [Transmute event](#transmute-event)                   | 0x07  | 28                                          |
| [Inputs event](#inputs-event)                         | 0x09  | 12 (16 for the first inputs event)          |
| End event                                             | 0x0B  | 0                                           |

### <a id="entity-types"></a>Entity types ###

| Entity type                                       | Value | Size (excluding entity type byte itself) |
|---------------------------------------------------|-------|------------------------------------------|
| [Dagger](#dagger-spawn-event)                     | 0x01  | 30                                       |
| [Squid I](#squid-spawn-event)                     | 0x03  | 32                                       |
| [Squid II](#squid-spawn-event)                    | 0x04  | 32                                       |
| [Squid III](#squid-spawn-event)                   | 0x05  | 32                                       |
| [Boid (skulls and spiderling)](#boid-spawn-event) | 0x06  | 45                                       |
| [Centipede](#pede-spawn-event)                    | 0x07  | 64                                       |
| [Gigapede](#pede-spawn-event)                     | 0x0C  | 64                                       |
| [Ghostpede](#pede-spawn-event)                    | 0x0F  | 64                                       |
| [Spider I](#spider-spawn-event)                   | 0x08  | 16                                       |
| [Spider II](#spider-spawn-event)                  | 0x09  | 16                                       |
| [Spider Egg (I and II)](#spider-egg-spawn-event)  | 0x0A  | 28                                       |
| [Leviathan](#leviathan-spawn-event)               | 0x0B  | 4                                        |
| [Thorn](#thorn-spawn-event)                       | 0x0D  | 20                                       |

### <a id="spawn-events"></a>Spawn events ###

#### <a id="dagger-spawn-event"></a>Dagger spawn event ####

| Data type           | Size | Meaning                      |
|---------------------|------|------------------------------|
| int32               | 4    | N/A (hardcoded at 0)         |
| vec3&lt;int16&gt;   | 6    | Position                     |
| mat3x3&lt;int16&gt; | 18   | Orientation                  |
| int8 (boolean)      | 1    | Shot (as opposed to rapid)   |
| uint8               | 1    | [Dagger type](#dagger-types) |

##### <a id="dagger-types"></a>Dagger types #####

| Dagger type           | Value |
|-----------------------|-------|
| Level 1               | 0x01  |
| Level 2               | 0x02  |
| Level 3               | 0x03  |
| Level 3 homing        | 0x04  |
| Level 4               | 0x05  |
| Level 4 homing        | 0x06  |
| Level 4 homing splash | 0x07  |

#### <a id="squid-spawn-event"></a>Squid spawn event ####

| Data type           | Size | Meaning            |
|---------------------|------|--------------------|
| int32               | 4    | ?                  |
| vec3&lt;float32&gt; | 12   | Position           |
| vec3&lt;float32&gt; | 12   | Direction          |
| float32             | 4    | Rotation (radians) |

#### <a id="boid-spawn-event"></a>Boid spawn event ####

| Data type           | Size | Meaning                  |
|---------------------|------|--------------------------|
| int32               | 4    | Spawner entity ID        |
| uint8               | 1    | [Boid type](#boid-types) |
| vec3&lt;int16&gt;   | 6    | Position                 |
| mat3x3&lt;int16&gt; | 18   | Orientation              |
| vec3&lt;float32&gt; | 12   | Velocity                 |
| float32             | 4    | Speed                    |

##### <a id="boid-types"></a>Boid types ###

| Boid type  | Value |
|------------|-------|
| Skull I    | 0x01  |
| Skull II   | 0x02  |
| Skull III  | 0x03  |
| Spiderling | 0x04  |
| Skull IV   | 0x05  |

#### <a id="pede-spawn-event"></a>Pede spawn event ####

| Data type             | Size | Meaning     |
|-----------------------|------|-------------|
| int32                 | 4    | ?           |
| vec3&lt;float32&gt;   | 12   | Position    |
| vec3&lt;float32&gt;?  | 12   | ?           |
| mat3x3&lt;float32&gt; | 36   | Orientation |

#### <a id="spider-spawn-event"></a>Spider spawn event ####

| Data type           | Size | Meaning  |
|---------------------|------|----------|
| int32               | 4    | ?        |
| vec3&lt;float32&gt; | 12   | Position |

#### <a id="spider-egg-spawn-event"></a>Spider Egg spawn event ####

| Data type            | Size | Meaning              |
|----------------------|------|----------------------|
| int32                | 4    | Spawner entity ID    |
| vec3&lt;float32&gt;  | 12   | Position?            |
| vec3&lt;float32&gt;? | 12   | Target position?     |

#### <a id="leviathan-spawn-event"></a>Leviathan spawn event ####

| Data type | Size | Meaning  |
|-----------|------|----------|
| int32     | 4    | ?        |

#### <a id="thorn-spawn-event"></a>Thorn spawn event ####

| Data type           | Size | Meaning             |
|---------------------|------|---------------------|
| int32               | 4    | ?                   |
| vec3&lt;float32&gt; | 12   | Position?           |
| float32             | 4    | Rotation (radians)? |

### <a id="entity-position-event"></a>Entity position event ###

| Data type         | Size | Meaning   |
|-------------------|------|-----------|
| int32             | 4    | Entity ID |
| vec3&lt;int16&gt; | 6    | Position  |

### <a id="entity-orientation-event"></a>Entity orientation event ###

| Data type           | Size | Meaning     |
|---------------------|------|-------------|
| int32               | 4    | Entity ID   |
| mat3x3&lt;int16&gt; | 18   | Orientation |

### <a id="entity-target-event"></a>Entity target event ###

| Data type         | Size | Meaning         |
|-------------------|------|-----------------|
| int32             | 4    | Entity ID       |
| vec3&lt;int16&gt; | 6    | Target position |

### <a id="hit-event"></a>Hit event ###

| Data type | Size | Meaning                |
|-----------|------|------------------------|
| int32     | 4    | Entity ID A            |
| int32     | 4    | Entity ID B (optional) |
| int32     | 4    | User data              |

Examples:
- If entity ID A is 0, it means the player died. Entity ID B will contain the [death type](#death-types). User data will be 0.
- When a dagger is deleted from the scene; entity ID A is the entity ID of the dagger and entity ID B is 0.
- When a dagger is eaten by Ghostpede; entity ID A is the entity ID of the Ghostpede and entity ID B is the entity ID of the dagger.
- When a Level 4 homing splash 'dagger' hits a Squid I; entity ID A is the entity ID of the Squid I and entity ID B is the entity ID of the homing splash 'dagger'.

#### User data

These values represent a part of an enemy. This is used to determine whether an enemy was actually damaged or not.

Weak points:

| Enemy type | Weak point values |
|------------|-------------------|
| Squid I    | 0                 |
| Squid II   | 0, 1              |
| Squid III  | 0, 1, 2           |
| Leviathan  | 0, 1, 2, 3, 4, 5  |
| Spider I   | 0                 |
| Spider II  | 0                 |

Any other value means the enemy was hit but not damaged.

In case of pedes, each segment has its own ID which is always a hit. When damaging a dead pede segment, the value is negated.

All other enemies are always hit, regardless of this value.

#### <a id="death-types"></a>Death types ####

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

### <a id="transmute-event"></a>Transmute event ###

| Data type         | Size | Meaning   |
|-------------------|------|-----------|
| int32             | 4    | Entity ID |
| vec3&lt;int16&gt; | 6    | ?         |
| vec3&lt;int16&gt; | 6    | ?         |
| vec3&lt;int16&gt; | 6    | ?         |
| vec3&lt;int16&gt; | 6    | ?         |

### <a id="inputs-event"></a>Inputs event ###

Input events mark the end of the game tick. They are always the last event in the group of events representing one game tick.

| Data type       | Size | Meaning                                                              |
|-----------------|------|----------------------------------------------------------------------|
| uint8 (boolean) | 1    | Holding left                                                         |
| uint8 (boolean) | 1    | Holding right                                                        |
| uint8 (boolean) | 1    | Holding forward                                                      |
| uint8 (boolean) | 1    | Holding backward                                                     |
| uint8           | 1    | [Jump](#jump-types)                                                  |
| uint8           | 1    | [Shoot](#shoot-types)                                                |
| uint8           | 1    | [Shoot homing](#shoot-types)                                         |
| int16           | 2    | Relative mouse offset X                                              |
| int16           | 2    | Relative mouse offset Y                                              |
| float32         | 4    | Look speed (only present in the first inputs event)                  |
| uint8           | 1    | Byte (always 0x0A) to mark the end of the event (or whole game tick) |

#### <a id="jump-types"></a>Jump types ####

| Jump type     | Value |
|---------------|-------|
| None          | 0x00  |
| Holding jump  | 0x01  |
| Pressing jump | 0x02  |

#### <a id="shoot-types"></a>Shoot types ####

| Shoot type            | Value |
|-----------------------|-------|
| None                  | 0x00  |
| Holding shoot input   | 0x01  |
| Releasing shoot input | 0x02  |
