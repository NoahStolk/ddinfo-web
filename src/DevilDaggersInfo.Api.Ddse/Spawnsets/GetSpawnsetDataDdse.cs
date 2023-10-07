namespace DevilDaggersInfo.Api.Ddse.Spawnsets;

public record GetSpawnsetDataDdse
{
	public required int SpawnVersion { get; init; }

	public required int WorldVersion { get; init; }

	public required GameModeDdse GameMode { get; init; }

	public required int NonLoopSpawnCount { get; init; }

	public required int LoopSpawnCount { get; init; }

	public required float? NonLoopLength { get; init; }

	public required float? LoopLength { get; init; }

	public required byte? Hand { get; init; }

	public required int? AdditionalGems { get; init; }

	public required float? TimerStart { get; init; }
}
