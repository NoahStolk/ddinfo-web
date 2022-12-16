using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

[Obsolete("DDCL 1.8.3 will be removed.")]
public record GetCustomLeaderboardSelectedPlayerNextDagger
{
	public double Time { get; init; }

	public CustomLeaderboardDagger Dagger { get; init; }
}
