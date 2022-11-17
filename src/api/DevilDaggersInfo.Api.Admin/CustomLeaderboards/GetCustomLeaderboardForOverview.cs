using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record GetCustomLeaderboardForOverview : IAdminOverviewGetDto
{
	public int Id { get; init; }

	public required string SpawnsetName { get; init; }

	public required GetCustomLeaderboardDaggers Daggers { get; init; }

	public bool IsFeatured { get; init; }

	public DateTime? DateCreated { get; init; }

	public CustomLeaderboardCategory Category { get; init; }
}
