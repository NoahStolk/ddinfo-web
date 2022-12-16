using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardWorldRecord
{
	public required double Time { get; init; }

	public required CustomLeaderboardDagger? Dagger { get; init; }
}
