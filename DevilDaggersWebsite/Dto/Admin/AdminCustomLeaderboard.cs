using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminCustomLeaderboard : IAdminDto
	{
		public CustomLeaderboardCategory Category { get; init; }
		public int SpawnsetFileId { get; init; }

		public int TimeBronze { get; init; }
		public int TimeSilver { get; init; }
		public int TimeGolden { get; init; }
		public int TimeDevil { get; init; }
		public int TimeLeviathan { get; init; }

		public Dictionary<string, string> Log()
		{
			Dictionary<string, string> dictionary = new();
			dictionary.Add(nameof(Category), Category.ToString());
			dictionary.Add(nameof(SpawnsetFileId), SpawnsetFileId.ToString());
			dictionary.Add(nameof(TimeBronze), TimeBronze.ToString());
			dictionary.Add(nameof(TimeSilver), TimeSilver.ToString());
			dictionary.Add(nameof(TimeGolden), TimeGolden.ToString());
			dictionary.Add(nameof(TimeDevil), TimeDevil.ToString());
			dictionary.Add(nameof(TimeLeviathan), TimeLeviathan.ToString());
			return dictionary;
		}
	}
}
