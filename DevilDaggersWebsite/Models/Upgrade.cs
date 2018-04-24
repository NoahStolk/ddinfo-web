namespace DevilDaggersWebsite.Models
{
	public class Upgrade
	{
		public int Level { get; set; }
		public string ColorCode { get; set; }
		public float DefaultSpray { get; set; }
		public float DefaultShot { get; set; }
		public float? HomingSpray { get; set; }
		public float? HomingShot { get; set; }
		public string UnlocksAt { get; set; }

		public Upgrade(int level, float defaultSpray, float defaultShot, float? homingSpray, float? homingShot, string colorCode, string unlocksAt)
		{
			Level = level;
			ColorCode = colorCode;
			DefaultSpray = defaultSpray;
			DefaultShot = defaultShot;
			HomingSpray = homingSpray;
			HomingShot = homingShot;
			UnlocksAt = unlocksAt;
		}
	}
}