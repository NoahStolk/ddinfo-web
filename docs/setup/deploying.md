# Deploying

Deployment runs from GitHub Actions (`.github/workflows/deploy.yml`), not from a developer machine: the host is IIS behind the Web Management Service, and `msdeploy.exe` only exists on Windows. The workflow is `workflow_dispatch` only — production never ships on a plain push.

Run it from **Actions → deploy.yml → Run workflow**, or:

```bash
gh workflow run deploy.yml                       # deploy main
gh workflow run deploy.yml -f dry_run=true       # preview only: msdeploy -whatif, the site is not touched
gh workflow run deploy.yml --ref my-branch       # deploy another branch's code
```

`workflow_dispatch` only becomes available once the workflow file is on the default branch — GitHub does not offer the trigger for a workflow that exists solely on a topic branch. After that, `--ref` picks which branch is built and deployed (that branch's copy of the workflow is the one that runs).

Start with `dry_run=true` after any change to the sync arguments: it authenticates against the host and reports every file it would add, update or delete without modifying the site, which is how you confirm the `Data` and `keys` skip rules hold.

## What the workflow does

1. `test` (ubuntu): builds and tests the solution. The deploy job does not start unless this passes.
2. `deploy` (windows):
   - `dotnet publish` of `DevilDaggersInfo.Web.Server` into `./publish` (framework-dependent — `web.config` runs the app in-process through `AspNetCoreModuleV2` with `processPath="dotnet"`, so the host needs a matching ASP.NET Core Hosting Bundle installed).
   - Replaces the `__PLACEHOLDER__` values in `publish/appsettings.json` with GitHub secrets. Any placeholder left over fails the job, because a placeholder reaching the host means a broken startup (an invalid `Sentry:Dsn` throws before the host is built).
   - Syncs `./publish` to the IIS site with `msdeploy -verb:sync`.

`web.config` is part of the publish output: the SDK's web.config transform copies it out of the project and fills in the ASP.NET Core Module settings, so the deployed copy is the tracked one (only re-indented, with a BOM added). It therefore overwrites the host's copy on every deploy — edit `src/DevilDaggersInfo.Web.Server/web.config` in the repository, never the file on the server.

## Deployment target

| Setting             | Value                                                                      |
|---------------------|----------------------------------------------------------------------------|
| Management endpoint | `https://<MSDEPLOY_HOST>:8172/msdeploy.axd?site=devildaggers.info` (WMSVC) |
| IIS site / app path | `devildaggers.info`                                                        |
| User                | `<MSDEPLOY_USER>`, basic auth                                              |

The host name and user are GitHub secrets so that they are masked in run logs. They used to live in `Properties/PublishProfiles/IISProfile.pubxml`, which was deleted when deployment moved into CI — everything it configured now lives in the workflow, in this document, or in `DevilDaggersInfo.Web.Server.csproj`.

## Server state that must survive a deploy

`-verb:sync` deletes anything on the destination that is not in the source, and neither of these directories is in the repository — a checkout has no copy of them, so without skip rules the first deploy would delete production data:

| Directory  | Contents                                                                        | Written by                                                    |
|------------|---------------------------------------------------------------------------------|---------------------------------------------------------------|
| `Data/`    | mods, mod screenshots, custom entry replays, leaderboard history and statistics | `FileSystemService` (relative to the content root)            |
| `keys/`    | data protection key ring                                                        | `AddDataProtection().PersistKeysToFileSystem` in `Program.cs` |
| `logs/`    | stdout logs, when enabled in `web.config`                                       | ASP.NET Core Module                                           |

The workflow therefore passes `-skip:objectName=dirPath,absolutePath=.*\\Data` (and the same for `keys` and `logs`). Skip rules apply to both source and destination: those directories are never uploaded and never deleted. `DevilDaggersInfo.Web.Server.csproj` additionally marks the `Data` subdirectories `CopyToPublishDirectory="Never"`, so a local `dotnet publish` does not drag a developer's copy of the data along.

## GitHub secrets

| Secret                                      | Value                                                                               |
|---------------------------------------------|-------------------------------------------------------------------------------------|
| `MSDEPLOY_HOST`                             | host name only, e.g. `webNN.example.com` — the workflow adds `https://` and `:8172` |
| `MSDEPLOY_USER`                             | Web Deploy user                                                                     |
| `MSDEPLOY_PASS`                             | Web Deploy password                                                                 |
| `AUTHENTICATION_JWT_KEY`                    | `Authentication:JwtKey`                                                             |
| `CUSTOM_LEADERBOARDS_INITIALIZATION_VECTOR` | `CustomLeaderboards:InitializationVector`                                           |
| `CUSTOM_LEADERBOARDS_PASSWORD`              | `CustomLeaderboards:Password`                                                       |
| `CUSTOM_LEADERBOARDS_SALT`                  | `CustomLeaderboards:Salt`                                                           |
| `DISCORD_BOT_TOKEN`                         | `Discord:BotToken`                                                                  |
| `MYSQL_CONNECTION_STRING`                   | `MySql:ConnectionString`, including options such as `Convert Zero Datetime=True`    |
| `SENTRY_DSN`                                | `Sentry:Dsn`                                                                        |

Copy the four validated option sections verbatim from the configuration that is currently live. In particular:

- a different `Authentication:JwtKey` invalidates every issued admin token;
- different `CustomLeaderboards` values break decryption of custom leaderboard submissions coming from the game and tools.

Secrets are substituted literally, so a value containing `"` or `\` would produce invalid JSON; the workflow fails the job instead of shipping it.

**The host's `appsettings.json` is now owned by CI** and is overwritten on every deploy. Change production configuration by updating the GitHub secret and redeploying, not by editing the file on the host.

## Local configuration

`appsettings.json` (placeholders) and `appsettings.Development.json` (non-secret local values) are both tracked. Real local secrets go into user secrets, which ASP.NET Core loads automatically in the Development environment and which override the tracked files:

```bash
dotnet user-secrets --project src/DevilDaggersInfo.Web.Server set "Discord:BotToken" "<token>"
```

The bot token is the one value the server cannot start without: `DiscordBotService` calls `ConnectAsync()` during startup. Set `CustomLeaderboards:*` and `Authentication:JwtKey` the same way if you need parity with production locally. `appsettings.Development.json` is excluded from publish output, so it never reaches the host.
