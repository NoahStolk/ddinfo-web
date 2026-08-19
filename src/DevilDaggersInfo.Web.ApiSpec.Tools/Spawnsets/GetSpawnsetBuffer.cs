namespace DevilDaggersInfo.Web.ApiSpec.Tools.Spawnsets;

public sealed record GetSpawnsetBuffer
{
	public required byte[] Data { get; init; }
}
