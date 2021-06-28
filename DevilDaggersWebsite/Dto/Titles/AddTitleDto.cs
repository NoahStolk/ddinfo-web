using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.Titles
{
	public class AddTitleDto : IAdminDto
	{
		[StringLength(16)]
		public string Name { get; init; } = null!;

		public List<int>? PlayerIds { get; init; }

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(Name), Name);
			dictionary.Add(nameof(PlayerIds), PlayerIds != null ? string.Join(", ", PlayerIds) : string.Empty);
			return dictionary;
		}
	}
}
