namespace DevilDaggersWebsite.Models.Spawnset
{
	public class SpawnData
	{
		public int NonLoopSpawns { get; set; }
		public int LoopSpawns { get; set; }

		public float NonLoopSeconds { get; set; }
		public float LoopSeconds { get; set; }

		public float LoopStart { get { return LoopSpawns == 0 ? 0 : NonLoopSeconds; } }
	}
}