using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

[Obsolete("DDCL 1.8.3 will be removed.")]
public record GetCustomLeaderboardWorldRecord
{
	public double Time { get; init; }

	public CustomLeaderboardDagger? Dagger { get; init; }
}
