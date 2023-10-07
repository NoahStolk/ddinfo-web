namespace DevilDaggersInfo.Web.ApiSpec.Admin.CustomLeaderboards;

public record GetCustomLeaderboardForOverview : IAdminOverviewGetDto
{
	public required int Id { get; init; }

	public required string SpawnsetName { get; init; }

	public required GetCustomLeaderboardDaggers Daggers { get; init; }

	public required bool IsFeatured { get; init; }

	public required DateTime? DateCreated { get; init; }

	public required CustomLeaderboardRankSorting RankSorting { get; init; }
}
