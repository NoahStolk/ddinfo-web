# Leaderboard replay binary

| Data type                         | Size                         | Meaning                      | Default value        |
|-----------------------------------|------------------------------|------------------------------|----------------------|
| String                            | 7                            | Identifier                   | DF_RPL2              |
| 16-bit integer                    | 2                            | Username length              | N/A (depends on run) |
| String                            | Username length              | Username                     | N/A (depends on run) |
| 16-bit integer                    | 2                            | ? length                     | ?                    |
| ?                                 | ? length                     | ?                            | ?                    |
| [Replay events](replay-events.md) | Compressed event data length | Compressed event data        | N/A (depends on run) |
