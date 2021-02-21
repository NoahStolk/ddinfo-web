using DevilDaggersWebsite.Enumerators;
using System;

namespace DevilDaggersWebsite.Dto
{
	public class CustomLeaderboard
	{
		public string SpawnsetName { get; set; } = null!;
		public string SpawnsetAuthorName { get; set; } = null!;
		public int TimeBronze { get; set; }
		public int TimeSilver { get; set; }
		public int TimeGolden { get; set; }
		public int TimeDevil { get; set; }
		public int TimeLeviathan { get; set; }
		public DateTime? DateLastPlayed { get; set; }
		public DateTime? DateCreated { get; set; }
		public CustomLeaderboardCategory Category { get; set; }
		public bool IsAscending { get; set; }
	}
}
