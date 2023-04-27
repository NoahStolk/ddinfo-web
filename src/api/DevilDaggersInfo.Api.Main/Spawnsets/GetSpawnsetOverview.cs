namespace DevilDaggersInfo.Api.Main.Spawnsets;

public record GetSpawnsetOverview
{
	public required int Id { get; init; }

	public required string Name { get; init; }

	public required string AuthorName { get; init; }

	public required DateTime LastUpdated { get; init; }

	public required GameMode GameMode { get; init; }

	public required float? LoopLength { get; init; }

	public required int LoopSpawnCount { get; init; }

	public required float? PreLoopLength { get; init; }

	public required int PreLoopSpawnCount { get; init; }

	public required HandLevel Hand { get; init; }

	public required int AdditionalGems { get; init; }
}
