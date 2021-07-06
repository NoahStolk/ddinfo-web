using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Titles
{
	public class AddTitle
	{
		[StringLength(16)]
		public string Name { get; init; } = null!;

		public List<int>? PlayerIds { get; init; }
	}
}
