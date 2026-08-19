namespace DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;

public sealed record GetSpawnsetName
{
	public required int Id { get; init; }

	public required string Name { get; init; }
}
