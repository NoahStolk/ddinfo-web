# Local replay binary

| Data type                         |                         Size | Meaning                                                  | Default value                 |
|-----------------------------------|------------------------------|----------------------------------------------------------|-------------------------------|
| String                            |                            6 | Identifier                                               | `ddrpl.`                      |
| 32-bit integer                    |                            4 | Version                                                  | 1                             |
| 64-bit integer                    |                            8 | UTC timestamp in seconds since game release (2016-02-18) | N/A (depends on current time) |
| 32-bit float                      |                            4 | Time at end of run                                       | N/A (depends on run)          |
| 32-bit float                      |                            4 | Start time                                               | N/A (depends on spawnset)     |
| 32-bit integer                    |                            4 | Daggers fired                                            | N/A (depends on run)          |
| 32-bit integer                    |                            4 | Death type                                               | N/A (depends on run)          |
| 32-bit integer                    |                            4 | Gems                                                     | N/A (depends on run)          |
| 32-bit integer                    |                            4 | Daggers hit                                              | N/A (depends on run)          |
| 32-bit integer                    |                            4 | Kills                                                    | N/A (depends on run)          |
| 32-bit integer                    |                            4 | Player ID                                                | N/A (depends on run)          |
| 32-bit integer                    |                            4 | Username length                                          | N/A (depends on run)          |
| String                            |              Username length | Username                                                 | N/A (depends on run)          |
| ?                                 |                           10 | ?                                                        | ?                             |
| MD5 hash byte array               |                           16 | Spawnset hash                                            | N/A (depends on spawnset)     |
| 32-bit integer                    |                            4 | Spawnset length                                          | N/A (depends on spawnset)     |
| [Spawnset](spawnset-binary.md)    |              Spawnset length | Spawnset                                                 | N/A (depends on spawnset)     |
| 32-bit integer                    |                            4 | Compressed event data length                             | N/A (depends on run)          |
| [Replay events](replay-events.md) | Compressed event data length | Compressed event data                                    | N/A (depends on run)          |
