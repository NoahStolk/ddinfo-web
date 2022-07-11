namespace DevilDaggersInfo.Api.Ddcl.Spawnsets;

public record GetSpawnsetBuffer
{
	public byte[] Data { get; init; } = null!;
}
