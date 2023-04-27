namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetTotalCustomLeaderboardData
{
	public required Dictionary<CustomLeaderboardCategory, int> LeaderboardsPerCategory { get; init; }

	public required Dictionary<CustomLeaderboardCategory, int> ScoresPerCategory { get; init; }

	public required Dictionary<CustomLeaderboardCategory, int> SubmitsPerCategory { get; init; }

	public required Dictionary<CustomLeaderboardCategory, int> PlayersPerCategory { get; init; }

	public required int TotalPlayers { get; init; }
}
