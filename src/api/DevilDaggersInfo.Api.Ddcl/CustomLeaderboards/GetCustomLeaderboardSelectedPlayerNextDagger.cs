using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomLeaderboardSelectedPlayerNextDagger
{
	public double Time { get; init; }

	public CustomLeaderboardDagger Dagger { get; init; }
}
