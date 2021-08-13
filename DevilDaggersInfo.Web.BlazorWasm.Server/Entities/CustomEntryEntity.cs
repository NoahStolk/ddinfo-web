using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities
{
	[Table("CustomEntries")]
	public class CustomEntryEntity
	{
		[Key]
		public int Id { get; init; }

		public int CustomLeaderboardId { get; set; }

		[ForeignKey(nameof(CustomLeaderboardId))]
		public CustomLeaderboardEntity CustomLeaderboard { get; set; } = null!;

		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public PlayerEntity Player { get; set; } = null!;

		public int Time { get; set; }

		public int GemsCollected { get; set; }

		public int EnemiesKilled { get; set; }

		public int DaggersFired { get; set; }

		public int DaggersHit { get; set; }

		public int EnemiesAlive { get; set; }

		public int HomingStored { get; set; }

		public int HomingEaten { get; set; }

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
