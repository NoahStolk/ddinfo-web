using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class UploadSuccess
	{
		public string Message { get; init; } = string.Empty;

		public int TotalPlayers { get; init; }
		public CustomLeaderboard Leaderboard { get; init; } = null!;
		public CustomLeaderboardCategory Category { get; init; }
		public List<CustomEntry> Entries { get; init; } = new();
		public bool IsNewPlayerOnThisLeaderboard { get; init; }

		public int Rank { get; init; }
		public int RankDiff { get; init; }

		public int Time { get; init; }
		public int TimeDiff { get; init; }

		public int GemsCollected { get; init; }
		public int GemsCollectedDiff { get; init; }

		public int EnemiesKilled { get; init; }
		public int EnemiesKilledDiff { get; init; }

		public int DaggersFired { get; init; }
		public int DaggersFiredDiff { get; init; }

		public int DaggersHit { get; init; }
		public int DaggersHitDiff { get; init; }

		public int EnemiesAlive { get; init; }
		public int EnemiesAliveDiff { get; init; }

		public int HomingDaggers { get; init; }
		public int HomingDaggersDiff { get; init; }

		public int GemsDespawned { get; init; }
		public int GemsDespawnedDiff { get; init; }

		public int GemsEaten { get; init; }
		public int GemsEatenDiff { get; init; }

		public int GemsTotal { get; init; }
		public int GemsTotalDiff { get; init; }

		public int LevelUpTime2 { get; init; }
		public int LevelUpTime2Diff { get; init; }

		public int LevelUpTime3 { get; init; }
		public int LevelUpTime3Diff { get; init; }

		public int LevelUpTime4 { get; init; }
		public int LevelUpTime4Diff { get; init; }
	}
}
