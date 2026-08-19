namespace DevilDaggersInfo.Web.ApiSpec.Admin.Caches;

public sealed record GetCacheEntry
{
	public required string Name { get; init; }

	public required int Count { get; init; }
}
