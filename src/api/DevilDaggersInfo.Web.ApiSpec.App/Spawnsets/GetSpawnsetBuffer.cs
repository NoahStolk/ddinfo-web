namespace DevilDaggersInfo.Api.App.Spawnsets;

public record GetSpawnsetBuffer
{
	public required byte[] Data { get; init; }
}
