namespace DevilDaggersInfo.Api.Ddcl.Spawnsets;

[Obsolete("DDCL 1.8.3 will be removed.")]
public record GetSpawnsetBuffer
{
	public required byte[] Data { get; init; }
}
