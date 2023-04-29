using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("SpawnsetFiles")]
public class SpawnsetEntity : IAuditable
{
	[Key]
	public int Id { get; init; }

	public int PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity? Player { get; set; }

	[StringLength(64)]
	public required string Name { get; set; }

	public int? MaxDisplayWaves { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	public DateTime LastUpdated { get; init; }

	public bool IsPractice { get; set; }

	[MaxLength(70 * 1024)]
	public required byte[] File { get; init; }

	#region File data needed for querying

	/// <summary>
	/// MD5 hash of <see cref="File"/>.
	/// </summary>
	[MaxLength(16)]
	public required byte[] Md5Hash { get; init; }

	// TODO: Use init for all these fields after migration.
	public required SpawnsetGameMode GameMode { get; set; }
	public required int SpawnVersion { get; set; }
	public required int WorldVersion { get; set; }
	public required int PreLoopSpawnCount { get; set; }
	public required int? PreLoopLength { get; set; }
	public required int LoopSpawnCount { get; set; }
	public required int? LoopLength { get; set; }
	public required SpawnsetHandLevel HandLevel { get; set; }
	public required int AdditionalGems { get; set; }
	public required int TimerStart { get; set; }
	public required SpawnsetHandLevel EffectiveHandLevel { get; set; }
	public required int EffectiveGemsOrHoming { get; set; }
	public required SpawnsetHandLevel EffectiveHandMesh { get; set; }

	#endregion
}
