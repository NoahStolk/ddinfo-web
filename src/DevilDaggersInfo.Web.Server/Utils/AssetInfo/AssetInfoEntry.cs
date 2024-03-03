namespace DevilDaggersInfo.Web.Server.Utils.AssetInfo;

public record AssetInfoEntry
{
	public AssetInfoEntry(string name, string description, List<string> tags)
	{
		Name = name;
		Description = description;
		Tags = tags;
	}

	public string Name { get; }
	public string Description { get; }
	public List<string> Tags { get; }
}
