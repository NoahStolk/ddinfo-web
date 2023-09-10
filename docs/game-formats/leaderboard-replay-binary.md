# Leaderboard replay binary

| Data type                         | Size in bytes                | Meaning                      | Default value        |
|-----------------------------------|------------------------------|------------------------------|----------------------|
| string                            | 7                            | Identifier                   | DF_RPL2              |
| int16                             | 2                            | Username length              | N/A (depends on run) |
| string                            | Username length              | Username                     | N/A (depends on run) |
| int16                             | 2                            | ? length                     | ?                    |
| ?                                 | ? length                     | ?                            | ?                    |
| [Replay events](replay-events.md) | Compressed event data length | Compressed event data        | N/A (depends on run) |
