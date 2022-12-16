using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardSelectedPlayerStats
{
	public required int Rank { get; init; }

	public required double Time { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }

	public required GetCustomLeaderboardSelectedPlayerNextDagger? NextDagger { get; init; }
}
