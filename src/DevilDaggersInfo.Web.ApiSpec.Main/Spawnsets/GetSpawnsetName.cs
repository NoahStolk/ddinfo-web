namespace DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

public sealed record GetSpawnsetName
{
	public required int Id { get; init; }

	public required string Name { get; init; }
}
