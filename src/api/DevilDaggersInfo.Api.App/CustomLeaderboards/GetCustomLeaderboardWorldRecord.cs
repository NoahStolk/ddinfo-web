using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomLeaderboardWorldRecord
{
	public double Time { get; init; }

	public CustomLeaderboardDagger? Dagger { get; init; }
}
