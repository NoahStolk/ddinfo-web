using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.CustomEntries
{
	public class AddUploadRequestPublic
	{
		public byte[] SurvivalHashMd5 { get; init; } = null!;
		public int PlayerId { get; init; }
		public string PlayerName { get; init; } = null!;
		public int Time { get; set; } // Use set to fix replay times.
		public int GemsCollected { get; init; }
		public int EnemiesKilled { get; init; }
		public int DaggersFired { get; init; }
		public int DaggersHit { get; init; }
		public int EnemiesAlive { get; init; }
		public int HomingDaggers { get; set; } // Use set to fix negative values.
		public int HomingDaggersEaten { get; init; }
		public int GemsDespawned { get; init; }
		public int GemsEaten { get; init; }
		public int GemsTotal { get; init; }
		public byte DeathType { get; init; }
		public int LevelUpTime2 { get; init; }
		public int LevelUpTime3 { get; init; }
		public int LevelUpTime4 { get; init; }
		public string ClientVersion { get; init; } = null!;
		public OperatingSystem OperatingSystem { get; init; }
		public BuildMode BuildMode { get; init; }
		public string Validation { get; set; } = null!; // Use set for unit tests.
		public bool IsReplay { get; init; }
		public bool ProhibitedMods { get; init; }
		public List<AddGameStatePublic> GameStates { get; init; } = new();
	}
}
