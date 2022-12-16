using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public record CustomLeaderboardOverviewSelectedPlayerStats
{
	public required int Rank { get; init; }

	public required int Time { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }

	public required CustomLeaderboardOverviewSelectedPlayerNextDagger? NextDagger { get; init; }
}
