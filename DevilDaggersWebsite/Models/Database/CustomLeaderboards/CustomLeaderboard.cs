using DevilDaggersCore.Game;

namespace DevilDaggersWebsite.Models.Database.CustomLeaderboards
{
	public class CustomLeaderboard
	{
		public int ID { get; set; }
		public string SpawnsetFileName { get; set; }
		public string SpawnsetHash { get; set; }
		public float Bronze { get; set; }
		public float Silver { get; set; }
		public float Golden { get; set; }
		public float Devil { get; set; }

		public Dagger GetDagger(float time)
		{
			if (time > Devil)
				return Game.V3.Devil;
			if (time > Golden)
				return Game.V3.Golden;
			if (time > Silver)
				return Game.V3.Silver;
			if (time > Bronze)
				return Game.V3.Bronze;
			return Game.V3.Default;
		}
	}
}