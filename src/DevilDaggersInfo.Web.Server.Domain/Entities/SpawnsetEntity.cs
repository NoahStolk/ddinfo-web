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

	[Obsolete("Practice spawnsets are now always generated and there's no need for them to be marked as such.")]
	public bool IsPractice { get; init; }

	[MaxLength(70 * 1024)]
	public required byte[] File { get; init; }

	#region File data needed for querying

	/// <summary>
	/// MD5 hash of <see cref="File"/>.
	/// </summary>
	[MaxLength(16)]
	public required byte[] Md5Hash { get; init; }

	public required SpawnsetGameMode GameMode { get; init; }

	public required int SpawnVersion { get; init; }

	public required int WorldVersion { get; init; }

	public required int PreLoopSpawnCount { get; init; }

	public required int? PreLoopLength { get; init; }

	public required int LoopSpawnCount { get; init; }

	public required int? LoopLength { get; init; }

	public required SpawnsetHandLevel HandLevel { get; init; }

	public required int AdditionalGems { get; init; }

	public required int TimerStart { get; init; }

	public required SpawnsetHandLevel EffectiveHandLevel { get; init; }

	public required int EffectiveGemsOrHoming { get; init; }

	public required SpawnsetHandLevel EffectiveHandMesh { get; init; }

	#endregion
}
