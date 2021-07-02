using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.Titles
{
	public class AddTitleDto
	{
		[StringLength(16)]
		public string Name { get; init; } = null!;

		public List<int>? PlayerIds { get; init; }
	}
}
