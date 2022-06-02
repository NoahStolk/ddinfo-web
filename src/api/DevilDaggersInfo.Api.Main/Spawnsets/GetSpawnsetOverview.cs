namespace DevilDaggersInfo.Api.Main.Spawnsets;

public record GetSpawnsetOverview
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string AuthorName { get; init; } = null!;

	public DateTime LastUpdated { get; init; }

	public GameMode GameMode { get; init; }

	public float? LoopLength { get; init; }

	public int LoopSpawnCount { get; init; }

	public float? PreLoopLength { get; init; }

	public int PreLoopSpawnCount { get; init; }

	public HandLevel Hand { get; init; }

	public int AdditionalGems { get; init; }
}
