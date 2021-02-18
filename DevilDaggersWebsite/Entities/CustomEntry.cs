using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class CustomEntry
	{
		[Key]
		public int Id { get; set; }

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
		public int GemsDespawned { get; set; }
		public int GemsEaten { get; set; }
		public int GemsTotal { get; set; }
		public byte DeathType { get; set; }
		public int LevelUpTime2 { get; set; }
		public int LevelUpTime3 { get; set; }
		public int LevelUpTime4 { get; set; }
		public DateTime SubmitDate { get; set; }
		public string? ClientVersion { get; set; }

		public byte[]? GemsCollectedData { get; set; }
		public byte[]? EnemiesKilledData { get; set; }
		public byte[]? DaggersFiredData { get; set; }
		public byte[]? DaggersHitData { get; set; }
		public byte[]? EnemiesAliveData { get; set; }
		public byte[]? HomingDaggersData { get; set; }
		public byte[]? GemsDespawnedData { get; set; }
		public byte[]? GemsEatenData { get; set; }
		public byte[]? GemsTotalData { get; set; }

		public byte[]? Skull1sAliveData { get; set; }
		public byte[]? Skull2sAliveData { get; set; }
		public byte[]? Skull3sAliveData { get; set; }
		public byte[]? SpiderlingsAliveData { get; set; }
		public byte[]? Skull4sAliveData { get; set; }
		public byte[]? Squid1sAliveData { get; set; }
		public byte[]? Squid2sAliveData { get; set; }
		public byte[]? Squid3sAliveData { get; set; }
		public byte[]? CentipedesAliveData { get; set; }
		public byte[]? GigapedesAliveData { get; set; }
		public byte[]? Spider1sAliveData { get; set; }
		public byte[]? Spider2sAliveData { get; set; }
		public byte[]? LeviathansAliveData { get; set; }
		public byte[]? OrbsAliveData { get; set; }
		public byte[]? ThornsAliveData { get; set; }
		public byte[]? GhostpedesAliveData { get; set; }
		public byte[]? SpiderEggsAliveData { get; set; }

		public byte[]? Skull1sKilledData { get; set; }
		public byte[]? Skull2sKilledData { get; set; }
		public byte[]? Skull3sKilledData { get; set; }
		public byte[]? SpiderlingsKilledData { get; set; }
		public byte[]? Skull4sKilledData { get; set; }
		public byte[]? Squid1sKilledData { get; set; }
		public byte[]? Squid2sKilledData { get; set; }
		public byte[]? Squid3sKilledData { get; set; }
		public byte[]? CentipedesKilledData { get; set; }
		public byte[]? GigapedesKilledData { get; set; }
		public byte[]? Spider1sKilledData { get; set; }
		public byte[]? Spider2sKilledData { get; set; }
		public byte[]? LeviathansKilledData { get; set; }
		public byte[]? OrbsKilledData { get; set; }
		public byte[]? ThornsKilledData { get; set; }
		public byte[]? GhostpedesKilledData { get; set; }
		public byte[]? SpiderEggsKilledData { get; set; }

		public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;
	}
}
