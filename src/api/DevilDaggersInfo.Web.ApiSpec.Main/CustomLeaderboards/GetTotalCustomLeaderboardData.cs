using DevilDaggersInfo.Api.Main.Spawnsets;

namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetTotalCustomLeaderboardData
{
	public required Dictionary<GameMode, int> LeaderboardsPerGameMode { get; init; }

	public required Dictionary<GameMode, int> ScoresPerGameMode { get; init; }

	public required Dictionary<GameMode, int> SubmitsPerGameMode { get; init; }

	public required Dictionary<GameMode, int> PlayersPerGameMode { get; init; }

	public required int TotalPlayers { get; init; }
}
