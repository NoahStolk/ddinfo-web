using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Assets
{
	public class GetAssetInfo
	{
		public string Name { get; init; } = null!;
		public string Description { get; init; } = null!;
		public List<string> Tags { get; init; } = null!;
	}
}
