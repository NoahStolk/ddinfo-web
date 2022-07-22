# Core libraries

There are currently four types of core libraries.

## `app-core`

| **Project**              | **Reasoning**                                                        |
|--------------------------|----------------------------------------------------------------------|
| App.Core.ApiClient       | Provides API client base functionality for apps                      |
| App.Core.GameMemory      | Only apps are able to access game memory                             |
| App.Core.NativeInterface | Only apps are able to access native functionality through interfaces |

## `core`

| **Project**     | **Reasoning**                                                           |
|-----------------|-------------------------------------------------------------------------|
| Core.Asset      | Any project may access asset information                                |
| Core.Encryption | Both the custom leaderboard app and the web server require this library |
| Core.Mod        | Any project may want to parse mod binaries                              |
| Core.Replay     | Any project may want to parse replay binaries                           |
| Core.Spawnset   | Any project may want to parse spawnset binaries                         |
| Core.Versioning | Both the apps and the web server require this library                   |
| Core.Wiki       | Any project may access wiki information                                 |

## `razor-core`

| **Project**             | **Reasoning**                                   |
|-------------------------|-------------------------------------------------|
| Razor.Core.CanvasChart  | Any UI project may want to render canvas charts |
| Razor.Core.Components   | Any UI project has access to these components   |
| Razor.Core.Unmarshalled | Required by Razor.Core.CanvasChart              |

## `web-core`

| **Project**     | **Reasoning**                                                      |
|-----------------|--------------------------------------------------------------------|
| Web.Core.Claims | Both the web server domain and the web client require this library |
