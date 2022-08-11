using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.Api.Ddse.Spawnsets;

public record GetSpawnsetDataDdse
{
	public int SpawnVersion { get; init; }

	public int WorldVersion { get; init; }

	public GameMode GameMode { get; init; }

	public int NonLoopSpawnCount { get; init; }

	public int LoopSpawnCount { get; init; }

	public float? NonLoopLength { get; init; }

	public float? LoopLength { get; init; }

	public byte? Hand { get; init; }

	public int? AdditionalGems { get; init; }

	public float? TimerStart { get; init; }
}
