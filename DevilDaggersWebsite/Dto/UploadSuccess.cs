using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class UploadSuccess
	{
		public string Message { get; set; }

		public int TotalPlayers { get; set; }
		public CustomLeaderboard Leaderboard { get; set; }
		public CustomLeaderboardCategory Category { get; set; }
		public List<CustomEntry> Entries { get; set; }
		public bool IsNewUserOnThisLeaderboard { get; set; }

		public int Rank { get; set; }
		public int RankDiff { get; set; }

		public int Time { get; set; }
		public int TimeDiff { get; set; }

		public int Kills { get; set; }
		public int KillsDiff { get; set; }

		public int Gems { get; set; }
		public int GemsDiff { get; set; }

		public int DaggersHit { get; set; }
		public int DaggersHitDiff { get; set; }

		public int DaggersFired { get; set; }
		public int DaggersFiredDiff { get; set; }

		public int EnemiesAlive { get; set; }
		public int EnemiesAliveDiff { get; set; }

		public int Homing { get; set; }
		public int HomingDiff { get; set; }

		public int LevelUpTime2 { get; set; }
		public int LevelUpTime2Diff { get; set; }

		public int LevelUpTime3 { get; set; }
		public int LevelUpTime3Diff { get; set; }

		public int LevelUpTime4 { get; set; }
		public int LevelUpTime4Diff { get; set; }
	}
}