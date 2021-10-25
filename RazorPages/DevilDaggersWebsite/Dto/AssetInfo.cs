using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class AssetInfo
	{
		public AssetInfo(string name, string description, List<string> tags)
		{
			Name = name;
			Description = description;
			Tags = tags;
		}

		public string Name { get; }
		public string Description { get; }
		public List<string> Tags { get; }
	}
}
