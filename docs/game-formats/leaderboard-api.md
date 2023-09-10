# Leaderboard API

There are multiple HTTP endpoints used for Devil Daggers to connect to the leaderboard.

## POST http://dd.hasmodai.com/backend15/get_scores.php

Used to get scores from the leaderboard (max 100 at a time).

### Parameters

| Name   | Type  | Description                           | Examples                                                                      |
|:-------|:------|:--------------------------------------|:------------------------------------------------------------------------------|
| offset | int32 | The offset of the scores to retrieve. | `offset=0` returns top 100 scores, `offset=1000` returns scores 1001 to 1100. |

### Response

The response is in binary and has the following format:

#### Header

| Data name             | Offset | Data type | Size in bytes |
|:----------------------|:-------|:----------|:--------------|
| ?                     | 0      | ?         | 11            |
| Global deaths         | 11     | uint64    | 8             | 
| Global kills          | 19     | uint64    | 8             |
| Global daggers fired  | 27     | uint64    | 8             |
| Global time played    | 35     | uint64    | 8             |
| Global gems           | 43     | uint64    | 8             |
| Global daggers hit    | 51     | uint64    | 8             |
| Total scores returned | 59     | uint16    | 2             |
| ?                     | 61     | ?         | 14            |
| Total players         | 75     | int32     | 4             |
| ?                     | 79     | ?         | 4             |

#### Scores

This data is repeated for each score returned.

| Data name                    | Data type     | Size in bytes   |
|:-----------------------------|:--------------|:----------------|
| Username length              | int16         | 2               |
| Username                     | string (utf8) | username length |
| Rank                         | int32         | 4               |
| Player id                    | int32         | 4               |
| Time in 10th of milliseconds | int32         | 4               |
| Kills                        | int32         | 4               |
| Gems                         | int32         | 4               |
| [Death type](death-types.md) | int32         | 4               |
| Total deaths                 | uint64        | 8               |
| Total kills                  | uint64        | 8               |
| Total daggers fired          | uint64        | 8               |
| Total time played            | uint64        | 8               |
| Total gems                   | uint64        | 8               |
| Total daggers hit            | uint64        | 8               |
| ?                            | ?             | 4               |

## POST http://dd.hasmodai.com/backend16/get_user_search_public.php

Used to search for a user by username.

### Parameters

| Name   | Type                                                   | Description                |
|:-------|:-------------------------------------------------------|:---------------------------|
| search | string (must be between 3 and 16 characters in length) | The username to search for |

### Response

The response is in binary and has the following format:

#### Header

| Data name             | Offset | Data type | Size in bytes |
|:----------------------|:-------|:----------|:--------------|
| ?                     | 0      | ?         | 11            |
| Total scores returned | 11     | uint16    | 2             |
| ?                     | 13     | ?         | 6             |

#### Scores

This data is repeated for each score returned.

| Data name                    | Data type     | Size in bytes   |
|:-----------------------------|:--------------|:----------------|
| Username length              | int16         | 2               |
| Username                     | string (utf8) | username length |
| Rank                         | int32         | 4               |
| Player id                    | int32         | 4               |
| ?                            | ?             | 4               |
| Time in 10th of milliseconds | int32         | 4               |
| Kills                        | int32         | 4               |
| Daggers fired                | int32         | 4               |
| Daggers hit                  | int32         | 4               |
| Gems                         | int32         | 4               |
| [Death type](death-types.md) | int32         | 4               |
| Total deaths                 | uint64        | 8               |
| Total kills                  | uint64        | 8               |
| Total daggers fired          | uint64        | 8               |
| Total time played            | uint64        | 8               |
| Total gems                   | uint64        | 8               |
| Total daggers hit            | uint64        | 8               |
| ?                            | ?             | 4               |

## POST http://l.sorath.com/dd/get_multiple_users_by_id_public.php

Used to get multiple users by their player id.

### Parameters

| Name | Type   | Description                                                                 | Examples                                                                |
|:-----|:-------|:----------------------------------------------------------------------------|:------------------------------------------------------------------------|
| uid  | string | The player ids to search for. Multiple ids can be separated by a comma (,). | `ids=1` returns the user with player id 1, `ids=1,2,3` returns 3 users. |

### Response

The response is in binary and has the following format:

#### Header

| Data name | Offset | Data type | Size in bytes |
|:----------|:-------|:----------|:--------------|
| ?         | 0      | ?         | 19            |

#### Scores

This data is repeated for each score returned.

| Data name                    | Data type     | Size in bytes   |
|:-----------------------------|:--------------|:----------------|
| Username length              | int16         | 2               |
| Username                     | string (utf8) | username length |
| Rank                         | int32         | 4               |
| Player id                    | int32         | 4               |
| ?                            | ?             | 4               |
| Time in 10th of milliseconds | int32         | 4               |
| Kills                        | int32         | 4               |
| Daggers fired                | int32         | 4               |
| Daggers hit                  | int32         | 4               |
| Gems                         | int32         | 4               |
| [Death type](death-types.md) | int32         | 4               |
| Total deaths                 | uint64        | 8               |
| Total kills                  | uint64        | 8               |
| Total daggers fired          | uint64        | 8               |
| Total time played            | uint64        | 8               |
| Total gems                   | uint64        | 8               |
| Total daggers hit            | uint64        | 8               |

## POST http://dd.hasmodai.com/backend16/get_user_by_id_public.php

Used to get a user by their player id.

### Parameters

| Name | Type   | Description                  | Examples                                   |
|:-----|:-------|:-----------------------------|:-------------------------------------------|
| uid  | string | The player id to search for. | `uid=1` returns the user with player id 1. |

### Response

The response is in binary and has the following format:

| Data name                    | Data type     | Size in bytes   |
|:-----------------------------|:--------------|:----------------|
| ?                            | ?             | 19              |
| Username length              | int16         | 2               |
| Username                     | string (utf8) | username length |
| Rank                         | int32         | 4               |
| Player id                    | int32         | 4               |
| ?                            | ?             | 4               |
| Time in 10th of milliseconds | int32         | 4               |
| Kills                        | int32         | 4               |
| Daggers fired                | int32         | 4               |
| Daggers hit                  | int32         | 4               |
| Gems                         | int32         | 4               |
| [Death type](death-types.md) | int32         | 4               |
| Total deaths                 | uint64        | 8               |
| Total kills                  | uint64        | 8               |
| Total daggers fired          | uint64        | 8               |
| Total time played            | uint64        | 8               |
| Total gems                   | uint64        | 8               |
| Total daggers hit            | uint64        | 8               |

## POST http://dd.hasmodai.com/backend16/get_replay.php

Used to get a replay by its player id.

### Parameters

| Name   | Type | Description                          | Examples                                         |
|:-------|:-----|:-------------------------------------|:-------------------------------------------------|
| replay | int  | The player id to get the replay for. | `uid=1` returns the replay for player with id 1. |

### Response

[Leaderboard replay binary](leaderboard-replay-binary.md)
