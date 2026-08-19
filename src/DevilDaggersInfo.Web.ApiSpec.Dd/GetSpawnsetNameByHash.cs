namespace DevilDaggersInfo.Web.ApiSpec.Dd;

public sealed record GetSpawnsetNameByHash
{
	public required string Name { get; init; }
}
