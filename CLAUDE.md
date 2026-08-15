# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Website, web server and web APIs for [devildaggers.info](https://devildaggers.info/) — leaderboards, custom spawnsets/mods, custom leaderboards, and wiki data for the game Devil Daggers. The server also serves APIs consumed by external projects (the game itself, ddinfo-tools, ddstats-rust, DDLIVE, Clubber, and deprecated Windows tools DDSE/DDAE).

Core parsing libraries (spawnsets, mods, replays, wiki data) live in a separate repo and are consumed via the `DevilDaggersInfo.Core` NuGet package.

## Commands

All commands run from the repository root. The solution uses the `.slnx` format.

```bash
# Build
dotnet build src/DevilDaggersInfo.Web.slnx -c Release

# Test
dotnet test src/DevilDaggersInfo.Web.slnx -c Release --no-build

# Single test / filtered
dotnet test src/test/DevilDaggersInfo.Web.Server.Domain.Test --filter "FullyQualifiedName~WorldRecordRepositoryTests"

# Run the site (server hosts the Blazor WASM client)
dotnet run --project src/DevilDaggersInfo.Web.Server   # https://localhost:5001
```

**Tailwind:** building the client runs the Tailwind standalone CLI to generate `wwwroot/tailwind.min.css`. The pinned CLI version is downloaded on first build into `src/tools/` (gitignored, ~43MB, cached across builds) for the host platform — Windows, Linux and macOS on x64/arm64 are all handled. Both the binary and the generated CSS are gitignored. Pass `-p:TailwindBuild=false` to skip the step entirely (useful offline, or when only touching server code).

**Runtime prerequisites:** every project targets `net10.0`. `global.json` (repository root) pins the SDK to the 10.0.1xx feature band with `rollForward: latestFeature`, so a .NET 10 SDK is required and a .NET 11 SDK will not be picked up.

**Config:** `appsettings.json` (production, with `__PLACEHOLDER__` values injected at deploy time) and `appsettings.Development.json` (non-secret local values) are both tracked; real local secrets go in user secrets (`dotnet user-secrets`). The server binds and validates required option sections at startup (`Authentication`, `CustomLeaderboards`, `Discord`, `MySql`) via `AddValidatedOptions`, so it will not start without them. Database is MySQL (Oracle's `MySql.EntityFrameworkCore` provider, over Connector/NET — note the connection string dialect differs from MySqlConnector's); uploaded/generated files live under a `Data` directory next to the server (see `FileSystemService`).

Database migration scripts: see `docs/setup/generating-database-migration-scripts.md` (requires temporarily adding EF package references to `Web.Server.Domain`).

CI (`.github/workflows/`) builds + tests on PR — including the Tailwind step, so CSS/markup drift is caught — and on push to `main` also packs and pushes `ApiSpec.Admin`, `ApiSpec.Main` and `ApiSpec.Tools` to nuget.org — changes to those three projects are public API surface with their own `<Version>`. Deployment is a separate manual `workflow_dispatch` workflow that publishes on a Windows runner and syncs to IIS with MSDeploy — see `docs/setup/deploying.md`; note that `Data/` and `keys/` on the host are deliberately skipped by the sync.

## Architecture

### Project layout (`src/`)

| Group | Purpose |
| --- | --- |
| `Web.Server` | ASP.NET Core host: controllers, hosted services, Discord bot, NSwag, rewrite rules, and the concrete implementations of the domain's inverted interfaces. |
| `Web.Server.Domain` | Base domain: EF entities + `ApplicationDbContext`, shared repositories/services, domain models and commands. Must **not** reference any API spec project. |
| `Web.Server.Domain.Main` / `.Admin` | Subdomains, each may reference exactly its own API spec (`ApiSpec.Main` / `ApiSpec.Admin`). Both depend on the base domain. |
| `Web.ApiSpec.*` | DTO/contract-only projects, one per API consumer (`Main`, `Admin`, `Tools`, `Dd`, `Ddae`, `Ddse`, `DdLive`, `DdstatsRust`, `Clubber`). |
| `Web.Client` | Blazor WebAssembly site, served by `Web.Server`. |
| `Web.Client.Core.Canvas` / `.CanvasArena` / `.CanvasChart` | Canvas rendering via JS interop (`JSHost.ImportAsync` in the client's `Program.cs`) for arena previews and replay/history charts. |
| `Web.Core.Claims` | Shared role names and claims helpers, used by both server and client. |
| `DevUtil.*` | One-off console tools (leaderboard fetching, history CSV dumps, stat distribution). Not part of the deployed app. |

### Domain rules (from `docs/architecture/web-server.md`)

- **Repositories are read-only; services read and write.** Both talk to the database and file system directly. A service must **not** depend on a repository.
- Place a repository/service in `Domain.Main` or `Domain.Admin` when only that one API needs it — it may then use that API's DTOs directly. Place it in the base `Domain` when multiple APIs need it, and have it expose **domain models/commands** instead of DTOs (this is why the base domain cannot reference API specs).
- Note that some names are duplicated across layers (e.g. `CustomEntryRepository` exists in both `Domain` and `Domain.Admin`); `Program.cs` registers them with fully qualified names — keep that pattern when adding services.

### API surface

- Controllers are grouped into `Controllers/<ApiName>/` folders (`Main`, `Admin`, `Tools`, `Dd`, `Ddae`, `Ddse`, `DdLive`, `DdstatsRust`, `Clubber`). **Swagger documents are built from the controller namespace** — `ApiOperationProcessor` matches `...Controllers.{ApiName}`, so a controller in the wrong namespace silently disappears from its Swagger doc. Each new API also needs an `AddSwaggerDocument` call in `Program.cs`.
- Routes follow `api/<kebab-case>` for Main and `api/admin/<kebab-case>` for Admin. Admin endpoints are gated with `[Authorize(Roles = Roles.X)]` from `Web.Core.Claims`; auth is JWT bearer, and the client stores the token in local storage (`ApiHttpClient`).
- Conversion between layers uses extension methods in `Web.Server/Converters/{ApiToDomain,DomainToApi}/<ApiName>/` (`ToDomain()`, `ToMainApi()`, …) plus per-subdomain converters. Keep DTO ⇄ domain mapping in converters rather than inline in controllers.
- Paged endpoints use the shared `Page<T>` DTO and `Constants.PageSizeMin/Max/Default` with `[Range]` validation.

Adding a Main API endpoint typically touches: DTO in `ApiSpec.Main` → repository/service in `Domain`/`Domain.Main` → converter → controller in `Controllers/Main` → method on `MainApiHttpClient` → the Razor page.

### Background work

`HostedServices/` contains `AbstractBackgroundService` subclasses (leaderboard history recording, player name fetching, Discord user ID fetching, Discord log flushing) plus a one-shot `StartupCacheHostedService`. Several are only registered outside the Development environment — check `Program.cs` before assuming a service runs locally. Runtime state is exposed through singleton caches (`ILeaderboardHistoryCache`, `LeaderboardStatisticsCache`, `ModArchiveCache`) and surfaced in the admin portal.

Interfaces in `Domain/Services/Inversion/` (`IFileSystemService`, `IDdLeaderboardService`, `ILogContainerService`, the custom-leaderboard loggers) exist so the domain stays free of hosting/IO concerns; implementations live in `Web.Server/Services` and `Web.Server/Clients`.

### Client

Blazor WASM. Pages under `Pages/<Area>/`, reusable components under `Components/`, all styling via Tailwind utility classes with a custom palette and named grid templates in `tailwind.config.js` (`explicit-tailwind-classes.tailwind` keeps dynamically composed classes from being purged). API access goes exclusively through `MainApiHttpClient` / `AdminApiHttpClient`, which extend `ApiHttpClient`. URL redirects for old site versions are kept server-side in `Program.cs` and `RewriteRules/` — add one there when a route changes.

## Conventions

- `.editorconfig`: **tabs** everywhere (spaces only in `.csproj`/`.pubxml`/`.slnx`/`.yml`). Existing code uses `_camelCase` private fields, explicit types over `var`, and file-scoped namespaces.
- `Directory.Build.props`: `net10.0`, `LangVersion 12.0`, nullable enabled with `WarningsAsErrors=nullable`, `AnalysisMode=All`, implicit usings, invariant globalization. Analyzer warnings (StyleCop, Sonar, Roslynator, Nullable.Extended) are numerous and non-blocking — don't chase pre-existing ones, but don't add new ones either.
- `Directory.Packages.props`: central package management. Add new packages there as `<PackageVersion>` and reference them without a version in the csproj. Dependabot keeps versions current.
- Tests use MSTest + NSubstitute + EF Core InMemory (`TestDbContext`, `TestData`, `MockEntities`); test-only analyzer relaxations live in `src/test/Tests.globalconfig`.

## Reference docs

`docs/game-formats/` documents the binary formats and game data this repo parses (spawnset/mod/replay binaries, replay events, death types, game memory, the official leaderboard API). Consult it before touching parsing or replay/stat logic.
