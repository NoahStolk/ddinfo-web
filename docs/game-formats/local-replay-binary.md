# Local replay binary

| Data type                         | Size in bytes                | Meaning                                                  | Default value                 |
|-----------------------------------|------------------------------|----------------------------------------------------------|-------------------------------|
| string                            | 6                            | Identifier                                               | ddrpl.                        |
| int32                             | 4                            | Version                                                  | 1                             |
| int64                             | 8                            | UTC timestamp in seconds since game release (2016-02-18) | N/A (depends on current time) |
| float32                           | 4                            | Time at end of run                                       | N/A (depends on run)          |
| float32                           | 4                            | Start time                                               | N/A (depends on spawnset)     |
| int32                             | 4                            | Daggers fired                                            | N/A (depends on run)          |
| int32                             | 4                            | Death type                                               | N/A (depends on run)          |
| int32                             | 4                            | Gems                                                     | N/A (depends on run)          |
| int32                             | 4                            | Daggers hit                                              | N/A (depends on run)          |
| int32                             | 4                            | Kills                                                    | N/A (depends on run)          |
| int32                             | 4                            | Player ID                                                | N/A (depends on run)          |
| int32                             | 4                            | Username length                                          | N/A (depends on run)          |
| string                            | Username length              | Username                                                 | N/A (depends on run)          |
| ?                                 | 10                           | ?                                                        | ?                             |
| MD5 hash bytes                    | 16                           | Spawnset hash                                            | N/A (depends on spawnset)     |
| int32                             | 4                            | Spawnset length                                          | N/A (depends on spawnset)     |
| [Spawnset](spawnset-binary.md)    | Spawnset length              | Spawnset                                                 | N/A (depends on spawnset)     |
| int32                             | 4                            | Compressed event data length                             | N/A (depends on run)          |
| [Replay events](replay-events.md) | Compressed event data length | Compressed event data                                    | N/A (depends on run)          |
