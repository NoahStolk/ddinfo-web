namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

public class GetCustomLeaderboardForOverview : IAdminOverviewGetDto
{
	public int Id { get; init; }

	public string SpawnsetName { get; init; } = null!;

	public double TimeBronze { get; init; }

	public double TimeSilver { get; init; }

	public double TimeGolden { get; init; }

	public double TimeDevil { get; init; }

	public double TimeLeviathan { get; init; }

	public bool IsArchived { get; init; }

	public DateTime? DateCreated { get; init; }

	public CustomLeaderboardCategory Category { get; init; }
}
