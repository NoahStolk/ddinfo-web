using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.Api.Main.Spawnsets;

public record GetSpawnsetOverview
{
	public int Id { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public DateTime LastUpdated { get; init; }

	public GameMode GameMode { get; init; }

	public float? LoopLength { get; init; }

	public int LoopSpawnCount { get; init; }

	public float? PreLoopLength { get; init; }

	public int PreLoopSpawnCount { get; init; }

	public HandLevel Hand { get; init; }

	public int AdditionalGems { get; init; }
}
