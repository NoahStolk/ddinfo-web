namespace DevilDaggersInfo.Api.Admin.Caches;

public record GetCacheEntry
{
	public required string Name { get; init; }

	public required int Count { get; init; }
}
