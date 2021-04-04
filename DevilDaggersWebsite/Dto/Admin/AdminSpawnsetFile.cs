using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminSpawnsetFile : IAdminDto
	{
		public string Name { get; init; } = null!;
		public int PlayerId { get; init; }
		public int? MaxDisplayWaves { get; init; }
		public string? HtmlDescription { get; init; }
		public bool IsPractice { get; init; }

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(Name), Name);
			dictionary.Add(nameof(PlayerId), PlayerId.ToString());
			dictionary.Add(nameof(MaxDisplayWaves), MaxDisplayWaves.ToString() ?? string.Empty);
			dictionary.Add(nameof(HtmlDescription), HtmlDescription ?? string.Empty);
			dictionary.Add(nameof(IsPractice), IsPractice.ToString());
			return dictionary;
		}
	}
}
