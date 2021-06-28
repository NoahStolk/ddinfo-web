using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.Titles
{
	public class GetTitleDto
	{
		public int Id { get; init; }

		public string Name { get; init; } = null!;

		public List<int>? PlayerIds { get; init; }
	}
}
