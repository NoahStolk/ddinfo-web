using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

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

	public DateTime DateCreated { get; set; }

	public int TotalRunsSubmitted { get; set; }

	public bool IsFeatured { get; set; }

	public List<CustomEntryEntity>? CustomEntries { get; set; }

	public CustomLeaderboardDagger? GetDaggerFromTime(int time)
		=> IsFeatured ? CustomLeaderboardUtils.GetDaggerFromTime(Category, time, TimeLeviathan, TimeDevil, TimeGolden, TimeSilver, TimeBronze) : null;
}
