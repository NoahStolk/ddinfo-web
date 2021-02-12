using System;

namespace DevilDaggersWebsite.Dto
{
	public class CustomEntry
	{
		public int PlayerId { get; init; }
		public string Username { get; init; } = null!;
		public int Time { get; init; }
		public int GemsCollected { get; init; }
		public int GemsDespawned { get; init; }
		public int GemsEaten { get; init; }
		public int Kills { get; init; }
		public int DeathType { get; init; }
		public int DaggersHit { get; init; }
		public int DaggersFired { get; init; }
		public int EnemiesAlive { get; init; }
		public int HomingDaggers { get; init; }
		public int LevelUpTime2 { get; init; }
		public int LevelUpTime3 { get; init; }
		public int LevelUpTime4 { get; init; }
		public DateTime SubmitDate { get; init; }
		public string? ClientVersion { get; init; }
	}
}
