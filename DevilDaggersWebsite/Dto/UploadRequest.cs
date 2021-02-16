using DevilDaggersWebsite.Enumerators;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class UploadRequest
	{
		public string SpawnsetHash { get; set; } = null!;
		public int PlayerId { get; set; }
		public string Username { get; set; } = null!;
		public int Time { get; set; }
		public int Gems { get; set; }
		public int Kills { get; set; }
		public int DeathType { get; set; }
		public int DaggersHit { get; set; }
		public int DaggersFired { get; set; }
		public int EnemiesAlive { get; set; }
		public int Homing { get; set; }
		public int LevelUpTime2 { get; set; }
		public int LevelUpTime3 { get; set; }
		public int LevelUpTime4 { get; set; }
		public string ClientVersion { get; set; } = null!;
		public OperatingSystem OperatingSystem { get; set; }
		public BuildMode BuildMode { get; set; }
		public string Validation { get; set; } = null!;
		public List<GameState> GameStates { get; set; } = new();
	}
}
