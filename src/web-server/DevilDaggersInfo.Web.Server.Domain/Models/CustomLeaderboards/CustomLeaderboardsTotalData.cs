using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardsTotalData
{
	public required Dictionary<CustomLeaderboardCategory, int> LeaderboardsPerCategory { get; init; }

	public required Dictionary<CustomLeaderboardCategory, int> ScoresPerCategory { get; init; }

	public required Dictionary<CustomLeaderboardCategory, int> SubmitsPerCategory { get; init; }

	public required Dictionary<CustomLeaderboardCategory, int> PlayersPerCategory { get; init; }

	public required int TotalPlayers { get; init; }
}
