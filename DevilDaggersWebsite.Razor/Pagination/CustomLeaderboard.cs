using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using System;

namespace DevilDaggersWebsite.Razor.Pagination
{
	public class CustomLeaderboard
	{
		public CustomLeaderboard(CustomLeaderboardCategory category, string spawnsetName, string authorName, int timeBronze, int timeSilver, int timeGolden, int timeDevil, int timeLeviathan, CustomEntry? worldRecord, string? worldRecordDaggerName, DateTime? dateLastPlayed, DateTime? dateCreated, int totalRunsSubmitted, int totalPlayers)
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
			WorldRecordDaggerName = worldRecordDaggerName;
			DateLastPlayed = dateLastPlayed;
			DateCreated = dateCreated;
			TotalRunsSubmitted = totalRunsSubmitted;
			TotalPlayers = totalPlayers;
		}

		public CustomLeaderboardCategory Category { get; }

		public string SpawnsetName { get; }
		public string AuthorName { get; }

		public int TimeBronze { get; }
		public int TimeSilver { get; }
		public int TimeGolden { get; }
		public int TimeDevil { get; }
		public int TimeLeviathan { get; }
		public CustomEntry? WorldRecord { get; }
		public string? WorldRecordDaggerName { get; }

		public DateTime? DateLastPlayed { get; }
		public DateTime? DateCreated { get; }
		public int TotalRunsSubmitted { get; }
		public int TotalPlayers { get; }
	}
}
