# Architecture

## Project types and dependencies

| **Subfolder** | **Project type**            | **Can depend on**                |
|---------------|-----------------------------|----------------------------------|
| `cmd`         | Console apps (experimental) | `common`, `core`                 |
| `common`      | Common functionality        | Nothing                          |
| `core`        | Core set of features        | `common`, `core`                 |
| `editor`      | Editor apps                 | `common`, `core`, `razor`        |
| `razor`       | Razor UI libraries          | `common`, `core`, `razor`        |
| `tests`       | Unit tests                  | Anything                         |
| `tool`        | Tools for internal usage    | Anything                         |
| `web`         | Website                     | `common`, `core`, `razor`, `web` |

## Project hierarchy

Tests, tools, and source generators are omitted for clarity.

```mermaid
flowchart TD;

    asseteditor_wpf[DevilDaggersInfo.AssetEditor.Wpf]
    cmd_createreplay[DevilDaggersInfo.Cmd.CreateReplay]
    cmd_extractmod[DevilDaggersInfo.Cmd.ExtractMod]
    common[DevilDaggersInfo.Common]
    core_asset[DevilDaggersInfo.Core.Asset]
    core_encryption[DevilDaggersInfo.Core.Encryption]
    core_mod[DevilDaggersInfo.Core.Mod]
    core_replay[DevilDaggersInfo.Core.Replay]
    core_spawnset[DevilDaggersInfo.Core.Spawnset]
    core_wiki[DevilDaggersInfo.Core.Wiki]
    razor_core_asseteditor[DevilDaggersInfo.Razor.Core.AssetEditor]
    razor_core_canvaschart[DevilDaggersInfo.Razor.Core.CanvasChart]
    razor_core_unmarshalled[DevilDaggersInfo.Razor.Core.Unmarshalled]
    web_client[DevilDaggersInfo.Web.Client]
    web_server[DevilDaggersInfo.Web.Server]
    web_shared[DevilDaggersInfo.Web.Shared]
	
	class asseteditor_wpf ui;
	class cmd_createreplay,cmd_extractmod cmd;
	class common common;
	class core_asset,core_encryption,core_mod,core_replay,core_spawnset,core_wiki core;
	class razor_core_asseteditor,razor_core_canvaschart,razor_core_unmarshalled razor_core;
	class web_client web_client;
	class web_server web_server;
	class web_shared web_shared;

    classDef ui fill:#a00,stroke:#333,stroke-width:4px;
    classDef cmd fill:#0a0,stroke:#333,stroke-width:4px;
    classDef common fill:#000,stroke:#333,stroke-width:4px;
    classDef core fill:#006,stroke:#333,stroke-width:4px;
    classDef razor_core fill:#066,stroke:#333,stroke-width:4px;
    classDef web_client fill:#a66,stroke:#333,stroke-width:4px;
    classDef web_server fill:#6a6,stroke:#333,stroke-width:4px;
    classDef web_shared fill:#00a,stroke:#333,stroke-width:4px;

	asseteditor_wpf --> razor_core_asseteditor

	cmd_createreplay --> core_replay
	
	cmd_extractmod --> core_mod
	
	core_mod --> core_asset
	core_mod --> common
	
	core_replay --> common
	
	core_spawnset --> core_wiki
	
	core_wiki --> common
	
	razor_core_asseteditor --> core_mod
	
	razor_core_canvaschart --> razor_core_unmarshalled
	razor_core_canvaschart --> common

	web_client --> razor_core_canvaschart
	web_client --> web_shared
	
	web_server --> core_encryption
	web_server --> web_client
	
	web_shared --> common
	web_shared --> core_mod
	web_shared --> core_replay
	web_shared --> core_spawnset
```
