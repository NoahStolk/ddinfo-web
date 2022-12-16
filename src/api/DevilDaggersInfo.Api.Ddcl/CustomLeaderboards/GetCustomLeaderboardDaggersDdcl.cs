namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

[Obsolete("DDCL 1.8.3 will be removed.")]
public record GetCustomLeaderboardDaggersDdcl
{
	public double Bronze { get; init; }

	public double Silver { get; init; }

	public double Golden { get; init; }

	public double Devil { get; init; }

	public double Leviathan { get; init; }
}
