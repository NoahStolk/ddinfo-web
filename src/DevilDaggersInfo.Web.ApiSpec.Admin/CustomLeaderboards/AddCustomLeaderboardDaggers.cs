namespace DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;

public record AddCustomLeaderboardDaggers
{
	public required double Bronze { get; init; }

	public required double Silver { get; init; }

	public required double Golden { get; init; }

	public required double Devil { get; init; }

	public required double Leviathan { get; init; }
}
