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

Tests, tools, source generators, and common libraries are omitted for clarity.

```mermaid
flowchart TD;
	asseteditor_wpf[AssetEditor.Wpf]
	cmd_createreplay[Cmd.CreateReplay]
	cmd_extractmod[Cmd.ExtractMod]
	core_asset[Core.Asset]
	core_encryption[Core.Encryption]
	core_mod[Core.Mod]
	core_replay[Core.Replay]
	core_spawnset[Core.Spawnset]
	core_wiki[Core.Wiki]
	razor_core_asseteditor[Razor.Core.AssetEditor]
	razor_core_canvaschart[Razor.Core.CanvasChart]
	razor_core_unmarshalled[Razor.Core.Unmarshalled]
	web_client[Web.Client]
	web_server[Web.Server]
	web_shared[Web.Shared]
	
	ddse_legacy[DDSE 2]
	ddcl_legacy[DDCL 1]
	ddae_legacy[DDAE 1]
	ddcore_legacy[DevilDaggersCore.Wpf]

	class asseteditor_wpf ui;
	class cmd_createreplay,cmd_extractmod cmd;
	class core_asset,core_encryption,core_mod,core_replay,core_spawnset,core_wiki core;
	class razor_core_asseteditor,razor_core_canvaschart,razor_core_unmarshalled razor_core;
	class web_client web_client;
	class web_server web_server;
	class web_shared web_shared;

	class ddse_legacy,ddcl_legacy,ddae_legacy,ddcore_legacy legacy;

	classDef ui fill:#a00,stroke:#333,stroke-width:4px;
	classDef cmd fill:#0a0,stroke:#333,stroke-width:4px;
	classDef core fill:#006,stroke:#333,stroke-width:4px;
	classDef razor_core fill:#066,stroke:#333,stroke-width:4px;
	classDef web_client fill:#a66,stroke:#333,stroke-width:4px;
	classDef web_server fill:#6a6,stroke:#333,stroke-width:4px;
	classDef web_shared fill:#00a,stroke:#333,stroke-width:4px;

	classDef legacy fill:#666,stroke:#333,stroke-width:4px;

	subgraph Core
		core_encryption
		core_replay

		core_mod --> core_asset
		core_spawnset --> core_wiki
	end

	subgraph Razor Core
		razor_core_asseteditor ----> core_mod
		razor_core_canvaschart --> razor_core_unmarshalled
	end

	subgraph Web
		web_client ----> razor_core_canvaschart
		web_client --> web_shared
        
		web_server --> core_encryption
		web_server --> web_client
        
		web_shared ----> core_mod
		web_shared ----> core_replay
		web_shared ----> core_spawnset
	end
	
	subgraph Tool
		asseteditor_wpf ----> razor_core_asseteditor
	end

	subgraph Cmd
		cmd_createreplay ----> core_replay
		cmd_extractmod ----> core_mod
	end
	
	subgraph Legacy
		ddcl_legacy

		ddse_legacy --> ddcore_legacy
		ddae_legacy --> ddcore_legacy
	end
```

## Data hierarchy

```mermaid
flowchart TD;
	database[(Database)]
	filesystem[(File system)]
	server[Server]
	api[API]
	devildaggersinfo[Website]
	devildaggers[Devil Daggers]
	ddse[DDSE]
	ddcl[DDCL]
	ddae[DDAE]
	ddstatsrust[ddstats-rust]
	ddlive[DDLIVE]
	clubberserver[Clubber server]
	clubberapi[Clubber API]
	devildaggersleaderboards[Devil Daggers leaderboards API]
	
	class database,filesystem,server,api,devildaggersinfo,ddse,ddcl,ddae ddinfo;
	class devildaggers,ddstatsrust,ddlive,clubberserver,clubberapi,devildaggersleaderboards external;
	
	classDef ddinfo fill:#a60,stroke:#333,stroke-width:4px;
	classDef external fill:#60a,stroke:#333,stroke-width:4px;
	
	subgraph External
		devildaggers
		ddstatsrust
		ddlive
		clubberserver
		clubberapi
		devildaggersleaderboards
	end
	
	devildaggers --> api
	ddstatsrust --> api
	ddlive --> api
	clubberserver --> api
	
	server --> devildaggersleaderboards
	server --> clubberapi
	
	subgraph DevilDaggers.info
		server --> database
		server --> filesystem
		
		api --> server
		
		devildaggersinfo --> api
		ddse --> api
		ddcl --> api
		ddae --> api
	end
```
