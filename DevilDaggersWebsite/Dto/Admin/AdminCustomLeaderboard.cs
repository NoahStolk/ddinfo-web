using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminCustomLeaderboard : IAdminDto
	{
		public CustomLeaderboardCategory Category { get; init; }
		public int SpawnsetFileId { get; init; }

		[Range(10000, 15000000)]
		public int TimeBronze { get; init; }

		[Range(10000, 15000000)]
		public int TimeSilver { get; init; }

		[Range(10000, 15000000)]
		public int TimeGolden { get; init; }

		[Range(10000, 15000000)]
		public int TimeDevil { get; init; }

		[Range(10000, 15000000)]
		public int TimeLeviathan { get; init; }

		public bool IsArchived { get; init; }

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
			dictionary.Add(nameof(IsArchived), IsArchived.ToString());
			return dictionary;
		}

		public string? ValidateTimes()
		{
			if (Category.IsAscending())
			{
				if (TimeLeviathan >= TimeDevil)
					return nameof(TimeLeviathan);
				if (TimeDevil >= TimeGolden)
					return nameof(TimeDevil);
				if (TimeGolden >= TimeSilver)
					return nameof(TimeGolden);
				if (TimeSilver >= TimeBronze)
					return nameof(TimeSilver);
			}

			if (TimeLeviathan <= TimeDevil)
				return nameof(TimeLeviathan);
			if (TimeDevil <= TimeGolden)
				return nameof(TimeDevil);
			if (TimeGolden <= TimeSilver)
				return nameof(TimeGolden);
			if (TimeSilver <= TimeBronze)
				return nameof(TimeSilver);

			return null;
		}
	}
}
