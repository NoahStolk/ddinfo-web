# Replay events

Either a [local replay binary](https://github.com/NoahStolk/DevilDaggersInfo/blob/master/docs/game-formats/local-replay-binary.md) or a replay hosted on the Devil Daggers leaderboard servers contains replay events at the end of the data.

Data is compressed using ZLIB. In order to get raw event data, make sure to:

1. Skip the first 2 bytes (ZLIB header).
2. Inflate (decompress ZLIB) the rest of the data.

## Entity IDs

Event types work with entity IDs, which are integer values. An entity in Devil Daggers is an enemy or a dagger. Every entity has its own ID which can be referenced to from other events. The entity IDs are not stored, but can be defined by reading the spawn events in order. The first entity spawning in the replay will have ID 1, the second entity will have ID 2, etc.

## Raw replay events

Every event starts with an event type, which is a single byte. After that, additional data follows which differs in length per event type.

### Event types

| Event type                   | Value  | Length (excluding event type byte itself)   |
|------------------------------|--------|---------------------------------------------|
| [Spawn event](#spawn-events) | `0x00` | Variable, see [entity types](#entity-types) |
| Entity position event        | `0x01` | 10                                          |
| Entity orientation event     | `0x02` | 22                                          |
| Entity target event          | `0x04` | 10                                          |
| Hit event                    | `0x05` | 12                                          |
| Gem event                    | `0x06` | 0                                           |
| Transmute event              | `0x07` | 28                                          |
| Inputs event                 | `0x09` | 12 (16 for the first inputs event           |
| End event                    | `0x0b` | 0                                           |

### <a id="entity-types"></a>Entity types ###

| Entity type                  | Value  | Length (excluding entity type byte itself) |
|------------------------------|--------|--------------------------------------------|
| Dagger                       | `0x01` | 30                                         |
| Squid I                      | `0x03` | 32                                         |
| Squid II                     | `0x04` | 32                                         |
| Squid III                    | `0x05` | 32                                         |
| Boid (skulls and spiderling) | `0x06` | 45                                         |
| Centipede                    | `0x07` | 64                                         |
| Gigapede                     | `0x0c` | 64                                         |
| Ghostpede                    | `0x0f` | 64                                         |
| Spider I                     | `0x08` | 16                                         |
| Spider II                    | `0x09` | 16                                         |
| Spider Egg (I and II)        | `0x0a` | 28                                         |
| Leviathan                    | `0x0b` | 4                                          |
| Thorn                        | `0x0d` | 20                                         |

### <a id="spawn-events"></a>Spawn events ###

#### <a id="dagger-spawn-event"></a>Dagger spawn event ####

| Data type     | Size | Meaning                      |
|---------------|------|------------------------------|
| int32         | 4    | N/A (hardcoded at 0)         |
| vec3<int16>   | 6    | Position                     |
| mat3x3<int16> | 18   | Orientation                  |
| int8 (bool)   | 1    | Shot (as opposed to rapid)   |
| uint8         | 1    | [Dagger type](#dagger-types) |

##### <a id="dagger-types"></a>Dagger types #####

| Dagger type           | Value  |
|-----------------------|--------|
| Level 1               | `0x01` |
| Level 2               | `0x02` |
| Level 3               | `0x03` |
| Level 3 homing        | `0x04` |
| Level 4               | `0x05` |
| Level 4 homing        | `0x06` |
| Level 4 homing splash | `0x07` |

#### <a id="squid-spawn-event"></a>Squid spawn event ####

| Data type     | Size | Meaning            |
|---------------|------|--------------------|
| int32         | 4    | ?                  |
| vec3<float32> | 12   | Position           |
| vec3<float32> | 12   | Direction          |
| float32       | 4    | Rotation (radians) |

#### <a id="boid-spawn-event"></a>Boid spawn event ####

| Data type       | Size | Meaning                  |
|-----------------|------|--------------------------|
| int32           | 4    | Spawner entity ID        |
| uint8           | 1    | [Boid type](#boid-types) |
| vec3<int16>     | 6    | Position                 |
| vec3<int16>?    | 6    | ?                        |
| vec3<int16>?    | 6    | ?                        |
| vec3<int16>?    | 6    | ?                        |
| vec3<float32>?  | 12   | ?                        |
| float32         | 4    | Speed                    |

##### <a id="boid-types"></a>Boid types ###

| Boid type  | Value  |
|------------|--------|
| Skull I    | `0x01` |
| Skull II   | `0x02` |
| Skull III  | `0x03` |
| Spiderling | `0x04` |
| Skull IV   | `0x05` |

#### <a id="pede-spawn-event"></a>Pede spawn event ####

TODO

#### <a id="spider-spawn-event"></a>Spider spawn event ####

TODO

#### <a id="spider-egg-spawn-event"></a>Spider Egg spawn event ####

TODO

#### <a id="leviathan-spawn-event"></a>Leviathan spawn event ####

TODO

#### <a id="thorn-spawn-event"></a>Thorm spawn event ####

TODO

### <a id="entity-position-event"></a>Entity position event ###

TODO

### <a id="entity-position-event"></a>Entity orientation event ###

TODO

### <a id="entity-position-event"></a>Entity target event ###

TODO

### <a id="entity-position-event"></a>Hit event ###

TODO

### <a id="entity-position-event"></a>Gem event ###

Empty

### <a id="entity-position-event"></a>Transmute event ###

TODO

### <a id="entity-position-event"></a>Inputs event ###

TODO

### <a id="entity-position-event"></a>End event ###

Empty
