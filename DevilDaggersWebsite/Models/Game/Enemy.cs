using System.Collections.Generic;

namespace DevilDaggersWebsite.Models.Game
{
	public class Enemy
	{
		public string Name { get; set; }
		public string ColorCode { get; set; }
		public int HP { get; set; }
		public int Gems { get; set; }
		public int HitPartCount { get; set; }
		public Death Death { get; set; }
		public float? Homing3 { get; set; }
		public float? Homing4 { get; set; }
		public Enemy[] SpawnedBy { get; set; }

		public Enemy(string name, string colorCode, int hp, int gems, int hitPartCount, Death death, float? homing3, float? homing4, params Enemy[] spawnedBy)
		{
			Name = name;
			ColorCode = colorCode;
			HP = hp;
			Gems = gems;
			HitPartCount = hitPartCount;
			Death = death;
			Homing3 = homing3;
			Homing4 = homing4;
			SpawnedBy = spawnedBy;
		}
	}
}