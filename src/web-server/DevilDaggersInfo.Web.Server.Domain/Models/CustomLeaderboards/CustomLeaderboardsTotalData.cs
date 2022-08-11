using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public class CustomLeaderboardsTotalData
{
	public Dictionary<CustomLeaderboardCategory, int> LeaderboardsPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> ScoresPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> SubmitsPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> PlayersPerCategory { get; init; } = new();

	public int TotalPlayers { get; init; }
}
