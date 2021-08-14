using DevilDaggersInfo.Core.Spawnset.Enums;

namespace DevilDaggersInfo.Core.Spawnset.View
{
	public class SpawnView
	{
		public SpawnView(EnemyType enemyType, double seconds, int gems, int gemsTotal)
		{
			EnemyType = enemyType;
			Seconds = seconds;
			Gems = gems;
			GemsTotal = gemsTotal;
		}

		public EnemyType EnemyType { get; }
		public double Seconds { get; }
		public int Gems { get; }
		public int GemsTotal { get; }
	}
}
