namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record GetCustomLeaderboardDaggers
{
	public double Bronze { get; init; }

	public double Silver { get; init; }

	public double Golden { get; init; }

	public double Devil { get; init; }

	public double Leviathan { get; init; }
}
