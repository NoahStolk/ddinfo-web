namespace DevilDaggersInfo.Core.Spawnset.Summary;

public record struct SpawnSectionInfo
{
	public SpawnSectionInfo(int spawnCount, float? length)
	{
		SpawnCount = spawnCount;
		Length = length;
	}

	public int SpawnCount { get; }
	public float? Length { get; }
}
