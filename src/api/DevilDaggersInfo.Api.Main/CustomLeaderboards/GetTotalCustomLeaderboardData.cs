using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Main.CustomLeaderboards;

public record GetTotalCustomLeaderboardData
{
	public Dictionary<CustomLeaderboardCategory, int> LeaderboardsPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> ScoresPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> SubmitsPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> PlayersPerCategory { get; init; } = new();

	public int TotalPlayers { get; init; }
}
