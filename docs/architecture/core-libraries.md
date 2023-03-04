# Core libraries

There are currently two types of core libraries.

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

## `web-core`

| **Project**                 | **Reasoning**                                                      |
|-----------------------------|--------------------------------------------------------------------|
| Web.Core.Claims             | Both the web server domain and the web client require this library |
