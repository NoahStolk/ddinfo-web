using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Titles
{
	public class EditTitle
	{
		[StringLength(16)]
		public string Name { get; init; } = null!;

		public List<int>? PlayerIds { get; init; }
	}
}
