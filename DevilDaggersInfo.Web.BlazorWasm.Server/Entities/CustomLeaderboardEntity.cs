using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("CustomLeaderboards")]
public class CustomLeaderboardEntity
{
	[Key]
	public int Id { get; init; }

	[Column("SpawnsetFileId")]
	public int SpawnsetId { get; set; }

	[ForeignKey(nameof(SpawnsetId))]
	public SpawnsetEntity Spawnset { get; set; } = null!;

	public CustomLeaderboardCategory Category { get; set; }

	public int TimeBronze { get; set; }

	public int TimeSilver { get; set; }

	public int TimeGolden { get; set; }

	public int TimeDevil { get; set; }

	public int TimeLeviathan { get; set; }

	public DateTime? DateLastPlayed { get; set; }

	public DateTime? DateCreated { get; set; }

	public int TotalRunsSubmitted { get; set; }

	public bool IsArchived { get; set; }

	public List<CustomEntryEntity>? CustomEntries { get; set; }

	public CustomLeaderboardDagger GetDaggerFromTime(int time)
		=> CustomLeaderboardUtils.GetDaggerFromTime(Category, time, TimeLeviathan, TimeDevil, TimeGolden, TimeSilver, TimeBronze);
}
