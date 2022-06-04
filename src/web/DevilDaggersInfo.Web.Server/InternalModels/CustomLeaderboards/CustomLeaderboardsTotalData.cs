using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;

public class CustomLeaderboardsTotalData
{
	public Dictionary<CustomLeaderboardCategory, int> LeaderboardsPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> ScoresPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> SubmitsPerCategory { get; init; } = new();

	public Dictionary<CustomLeaderboardCategory, int> PlayersPerCategory { get; init; } = new();

	public int TotalPlayers { get; init; }
}
