# ddinfo-web

Website, web server, web API hosted at [devildaggers.info](https://devildaggers.info/)

The purpose of this website is to make it easier for players to understand the game, and to provide a platform for the community to create and share custom spawnsets (levels), mods, participate in custom leaderboards, and a lot more.

![Home page](images/home-page.png)

## Website features

- Viewing and searching the official leaderboards
- Viewing leaderboard data for any player
- Browsing leaderboard history, statistics, world record progression, player settings, and more
- Displaying custom spawnsets (levels) made by the community
  - Spawns list and accurate end loop timings
  - Accurate arena previewer with shrink slider
- Displaying mods made by the community
  - Mods can be downloaded directly from the website
  - Asset types and information about the mod is displayed
- Viewing custom leaderboards for spawnsets
  - Data for custom scores can be viewed, as well as replay statistics in the form of graphs
  - Custom replays can be downloaded directly from the website
- Guides on how to use and create custom spawnsets and mods
- Accurate wiki pages
  - Enemy types
  - List of spawns
  - Hand upgrades
  - Dagger types
  - List of game assets (the actual assets are not included)
- Editing your player settings (if you have an account)
- Browsing the APIs
- An admin portal for moderators to upload spawnsets and mods, manage custom leaderboards and player data, and more

## Web server features

The web server is responsible for serving the website and providing various APIs used by the website and the tools, but also by other projects, such as DDLIVE, ddstats-rust, Clubber, and Devil Daggers itself.

- Integration with the official leaderboards
- Keeping track of leaderboard history, statistics, and world record progression for the official leaderboards
- Storing player settings data that can be maintained by the player themselves if they have an account
- Hosting custom spawnsets
- Hosting custom leaderboards
  - Supports every game mode in the game (Survival, Time Attack, Race)
  - Allows sorting leaderboards by not just time, but various other statistics as well, such as:
    - Gems collected/despawned/eaten
    - Enemies killed/alive
    - Homing stored/eaten
  - Criteria system that allows for more interesting ways to play spawnsets, such as leaderboards where the player is not allowed to kill certain enemies
  - Hosting replays for every score
  - Hosting graph data for every score
- Hosting mods
- Providing APIs used by the website, as well as APIs for:
  - [ddinfo-tools](https://github.com/NoahStolk/ddinfo-tools)
  - The deprecated Windows-only tools
    - [DDSE 2](https://github.com/NoahStolk/DevilDaggersSurvivalEditor)
    - [DDAE 1](https://github.com/NoahStolk/DevilDaggersAssetEditor)
  - [ddstats-rust](https://github.com/lsaa/ddstats-rust)
  - [DDLIVE](https://github.com/rotisseriechicken/DDLIVE)
  - [Clubber](https://github.com/Spoertm/Clubber)
  - Devil Daggers itself

## Other repositories

- [ddinfo-core](https://github.com/NoahStolk/ddinfo-core/) - core libraries for parsing spawnsets, mods, replays, and more
- [ddinfo-tools](https://github.com/NoahStolk/ddinfo-tools) - cross-platform tools for practicing the game, creating and editing spawnsets and mods, connecting to custom leaderboards, and more

### Deprecated tools

The original apps are not included in this repository. Visit these repositories:

- [Survival Editor](https://github.com/NoahStolk/DevilDaggersSurvivalEditor) - old Windows-only spawnset editor
- [Custom Leaderboards](https://github.com/NoahStolk/DevilDaggersCustomLeaderboards) - old Windows-only custom leaderboards client (no longer works)
- [Asset Editor](https://github.com/NoahStolk/DevilDaggersAssetEditor) - old Windows-only asset editor
