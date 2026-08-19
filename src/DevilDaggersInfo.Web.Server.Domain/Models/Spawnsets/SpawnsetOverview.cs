using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;

/// <summary>
/// The subset of a spawnset needed for an overview listing. Querying this instead of the entity keeps the spawnset's
/// file bytes out of the result, which the overview does not use.
/// </summary>
public record SpawnsetOverview
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required SpawnsetGameMode GameMode { get; init; }

	public required int? LoopLength { get; init; }

	public required int LoopSpawnCount { get; init; }

	public required int? PreLoopLength { get; init; }

	public required int PreLoopSpawnCount { get; init; }

	public required SpawnsetHandLevel EffectiveHandLevel { get; init; }

	public required int EffectiveGemsOrHoming { get; init; }
}
