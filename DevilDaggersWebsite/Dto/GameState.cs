namespace DevilDaggersWebsite.Dto
{
	public class GameState
	{
		public ushort GemsCollected { get; set; }
		public ushort EnemiesKilled { get; set; }
		public int DaggersFired { get; set; }
		public int DaggersHit { get; set; }
		public ushort EnemiesAlive { get; set; }
		public ushort HomingDaggers { get; set; }
		public ushort GemsDespawned { get; set; }
		public ushort GemsEaten { get; set; }
		public ushort GemsTotal { get; set; }
	}
}
