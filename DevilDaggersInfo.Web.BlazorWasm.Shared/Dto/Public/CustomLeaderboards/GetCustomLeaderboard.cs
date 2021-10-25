using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

public class GetCustomLeaderboard
{
	public string SpawnsetName { get; init; } = null!;

	public string SpawnsetAuthorName { get; init; } = null!;

	public double TimeBronze { get; init; }

	public double TimeSilver { get; init; }

	public double TimeGolden { get; init; }

	public double TimeDevil { get; init; }

	public double TimeLeviathan { get; init; }

	public DateTime? DateLastPlayed { get; init; }

	public DateTime? DateCreated { get; init; }

	public int? TotalRunsSubmitted { get; init; }

	public bool IsArchived { get; init; }

	public CustomLeaderboardCategory Category { get; init; }

	public List<GetCustomEntry> CustomEntries { get; set; } = new();
}
