namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

public class GetCustomLeaderboardForOverview : IAdminOverviewGetDto
{
	public int Id { get; init; }

	public string SpawnsetName { get; init; } = null!;

	[Format(FormatUtils.TimeFormat)]
	public double TimeBronze { get; init; }

	[Format(FormatUtils.TimeFormat)]
	public double TimeSilver { get; init; }

	[Format(FormatUtils.TimeFormat)]
	public double TimeGolden { get; init; }

	[Format(FormatUtils.TimeFormat)]
	public double TimeDevil { get; init; }

	[Format(FormatUtils.TimeFormat)]
	public double TimeLeviathan { get; init; }

	public bool IsArchived { get; init; }

	public DateTime? DateCreated { get; init; }

	public CustomLeaderboardCategory Category { get; init; }
}
