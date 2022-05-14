namespace DevilDaggersInfo.Web.Shared.Dto.Admin.Caches;

public record GetCacheEntry
{
	public GetCacheEntry(string name, int count)
	{
		Name = name;
		Count = count;
	}

	public string Name { get; set; }

	public int Count { get; set; }
}
