using DevilDaggersInfo.Core.Spawnset.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;

public class GetSpawnsetOverview
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public string AuthorName { get; init; } = null!;

	public DateTime LastUpdated { get; init; }

	public string GameVersion { get; init; } = null!;

	public GameMode GameMode { get; init; }

	public float? LoopLength { get; init; }

	public int LoopSpawnCount { get; init; }

	public float? PreLoopLength { get; init; }

	public int PreLoopSpawnCount { get; init; }

	public byte Hand { get; init; }

	public int AdditionalGems { get; init; }

	public float TimerStart { get; init; }
}
