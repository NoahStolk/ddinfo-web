using System;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class CustomEntry
	{
		public int PlayerId { get; set; }
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
		public DateTime SubmitDate { get; set; }
		public string? ClientVersion { get; set; }
	}
}