namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

public class GetCustomLeaderboardForOverview : IGetDto
{
	public int Id { get; init; }

	[Display(Name = "Name")]
	public string SpawnsetName { get; init; } = null!;

	[Format(FormatUtils.TimeFormat)]
	[Display(Name = "Bronze")]
	public double TimeBronze { get; init; }

	[Format(FormatUtils.TimeFormat)]
	[Display(Name = "Silver")]
	public double TimeSilver { get; init; }

	[Format(FormatUtils.TimeFormat)]
	[Display(Name = "Golden")]
	public double TimeGolden { get; init; }

	[Format(FormatUtils.TimeFormat)]
	[Display(Name = "Devil")]
	public double TimeDevil { get; init; }

	[Format(FormatUtils.TimeFormat)]
	[Display(Name = "Levi")]
	public double TimeLeviathan { get; init; }

	public bool IsArchived { get; init; }

	public DateTime? DateCreated { get; init; }

	public CustomLeaderboardCategory Category { get; init; }
}
