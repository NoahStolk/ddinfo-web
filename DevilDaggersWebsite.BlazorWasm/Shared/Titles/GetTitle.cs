using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Titles
{
	public class GetTitle : IGetDto
	{
		public int Id { get; init; }

		public string Name { get; init; } = null!;

		public List<int>? PlayerIds { get; init; }
	}
}
