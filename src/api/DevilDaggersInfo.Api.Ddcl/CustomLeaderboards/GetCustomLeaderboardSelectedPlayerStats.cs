using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomLeaderboardSelectedPlayerStats
{
	public int Rank { get; init; }

	public double Time { get; init; }

	public CustomLeaderboardDagger? Dagger { get; init; }

	public GetCustomLeaderboardSelectedPlayerNextDagger? NextDagger { get; init; }
}
