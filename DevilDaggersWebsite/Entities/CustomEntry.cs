using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class CustomEntry
	{
		[Key]
		public int Id { get; init; }

		public int CustomLeaderboardId { get; set; }

		[ForeignKey(nameof(CustomLeaderboardId))]
		public CustomLeaderboard CustomLeaderboard { get; set; } = null!;

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int Time { get; set; }

		public int GemsCollected { get; set; }

		public int EnemiesKilled { get; set; }

		public int DaggersFired { get; set; }

		public int DaggersHit { get; set; }

		public int EnemiesAlive { get; set; }

		public int HomingDaggers { get; set; }

		public int HomingDaggersEaten { get; set; }

		public int GemsDespawned { get; set; }

		public int GemsEaten { get; set; }

		public int GemsTotal { get; set; }

		public byte DeathType { get; set; }

		public int LevelUpTime2 { get; set; }

		public int LevelUpTime3 { get; set; }

		public int LevelUpTime4 { get; set; }

		public DateTime SubmitDate { get; set; }

		[StringLength(16)]
		public string ClientVersion { get; set; } = null!;

		public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;
	}
}
