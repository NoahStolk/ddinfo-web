using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminTitle : IAdminDto
	{
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
