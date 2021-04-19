using DevilDaggersWebsite.Enumerators;
using System;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class CustomLeaderboard
	{
		public CustomLeaderboard(CustomLeaderboardCategory category, string spawnsetName, string authorName, int timeBronze, int timeSilver, int timeGolden, int timeDevil, int timeLeviathan, int worldRecord, DateTime? dateLastPlayed, DateTime? dateCreated, int totalRunsSubmitted, int totalPlayers)
		{
			Category = category;
			SpawnsetName = spawnsetName;
			AuthorName = authorName;
			TimeBronze = timeBronze;
			TimeSilver = timeSilver;
			TimeGolden = timeGolden;
			TimeDevil = timeDevil;
			TimeLeviathan = timeLeviathan;
			WorldRecord = worldRecord;
			DateLastPlayed = dateLastPlayed;
			DateCreated = dateCreated;
			TotalRunsSubmitted = totalRunsSubmitted;
			TotalPlayers = totalPlayers;
		}

		public CustomLeaderboardCategory Category { get; set; }

		public string SpawnsetName { get; set; }
		public string AuthorName { get; set; }

		public int TimeBronze { get; set; }
		public int TimeSilver { get; set; }
		public int TimeGolden { get; set; }
		public int TimeDevil { get; set; }
		public int TimeLeviathan { get; set; }
		public int WorldRecord { get; set; }

		public DateTime? DateLastPlayed { get; set; }
		public DateTime? DateCreated { get; set; }
		public int TotalRunsSubmitted { get; set; }
		public int TotalPlayers { get; set; }
	}
}
