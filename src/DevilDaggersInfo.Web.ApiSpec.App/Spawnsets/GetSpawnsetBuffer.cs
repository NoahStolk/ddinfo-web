namespace DevilDaggersInfo.Web.ApiSpec.App.Spawnsets;

public record GetSpawnsetBuffer
{
	public required byte[] Data { get; init; }
}
