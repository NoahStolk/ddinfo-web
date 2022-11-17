namespace DevilDaggersInfo.Api.Ddcl.Spawnsets;

public record GetSpawnsetBuffer
{
	public required byte[] Data { get; init; }
}
