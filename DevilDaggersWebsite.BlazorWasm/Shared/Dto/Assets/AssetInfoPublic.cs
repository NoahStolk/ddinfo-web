using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Assets
{
	public class AssetInfoPublic
	{
		public string Name { get; init; } = null!;
		public string Description { get; init; } = null!;
		public List<string> Tags { get; init; } = null!;
	}
}
