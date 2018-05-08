using System.Collections.Generic;

namespace DevilDaggersWebsite.Models.Spawnset
{
	public class Spawnset
	{
		public List<Spawn> Spawns { get; set; }
		public float[,] ArenaTiles { get; set; }
		public float ShrinkStart { get; set; }
		public float ShrinkEnd { get; set; }
		public float ShrinkRate { get; set; }
		public float Brightness { get; set; }

		public Spawnset(List<Spawn> spawns, float[,] arenaTiles, float shrinkStart, float shrinkEnd, float shrinkRate, float brightness)
		{
			Spawns = spawns;
			ArenaTiles = arenaTiles;
			ShrinkStart = shrinkStart;
			ShrinkEnd = shrinkEnd;
			ShrinkRate = shrinkRate;
			Brightness = brightness;
		}
	}
}