# Core libraries

There are currently four types of core libraries.

## `app-core`

| **Project**              | **Reasoning**                                                        |
|--------------------------|----------------------------------------------------------------------|
| App.Core.ApiClient       | Provides API client base functionality for apps                      |
| App.Core.AssetInterop    | Provides interoperability between Core.Mod and Warp.NET              |
| App.Core.GameMemory      | Only apps are able to access game memory                             |
| App.Core.NativeInterface | Only apps are able to access native functionality through interfaces |

## `core`

| **Project**             | **Reasoning**                                         |
|-------------------------|-------------------------------------------------------|
| Core.Asset              | Any project may access asset information              |
| Core.CriteriaExpression | The web projects and the app require this library     |
| Core.Encryption         | Both the app and the web server require this library  |
| Core.Mod                | Any project may want to parse mod binaries            |
| Core.Replay             | Any project may want to parse replay binaries         |
| Core.Spawnset           | Any project may want to parse spawnset binaries       |
| Core.Versioning         | Both the apps and the web server require this library |
| Core.Wiki               | Any project may access wiki information               |

## `razor-core`

| **Project**            | **Reasoning**                                      |
|------------------------|----------------------------------------------------|
| Razor.Core.Canvas      | Base functionality for CanvasArena and CanvasChart |
| Razor.Core.CanvasArena | Any UI project may want to render canvas arenas    |
| Razor.Core.CanvasChart | Any UI project may want to render canvas charts    |

## `web-core`

| **Project**                 | **Reasoning**                                                      |
|-----------------------------|--------------------------------------------------------------------|
| Web.Core.Claims             | Both the web server domain and the web client require this library |
