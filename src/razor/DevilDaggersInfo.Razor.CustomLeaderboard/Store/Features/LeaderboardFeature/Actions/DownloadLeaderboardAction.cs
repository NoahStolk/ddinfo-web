using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;

public record DownloadLeaderboardAction(byte[] SpawnsetHash);