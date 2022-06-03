namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public class GetCustomLeaderboardForOverview : IAdminOverviewGetDto
{
	public int Id { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public GetCustomLeaderboardDaggers Daggers { get; init; } = new();

	public bool IsFeatured { get; init; }

	public DateTime? DateCreated { get; init; }

	public CustomLeaderboardCategory Category { get; init; }
}
