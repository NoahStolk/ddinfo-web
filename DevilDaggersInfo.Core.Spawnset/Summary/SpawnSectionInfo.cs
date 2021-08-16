namespace DevilDaggersInfo.Core.Spawnset.Summary;

public struct SpawnSectionInfo
{
	public SpawnSectionInfo(int spawnCount, float? length)
	{
		SpawnCount = spawnCount;
		Length = length;
	}

	public int SpawnCount { get; }
	public float? Length { get; }
}
