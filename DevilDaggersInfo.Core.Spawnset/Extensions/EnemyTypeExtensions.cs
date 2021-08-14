using DevilDaggersInfo.Core.Spawnset.Enums;
using System;

namespace DevilDaggersInfo.Core.Spawnset.Extensions
{
	public static class EnemyTypeExtensions
	{
		public static int GetNoFarmGems(this EnemyType enemyType) => enemyType switch
		{
			EnemyType.Empty => 0,
			EnemyType.Squid1 => 2,
			EnemyType.Squid2 => 3,
			EnemyType.Centipede => 25,
			EnemyType.Spider1 => 1,
			EnemyType.Leviathan => 6,
			EnemyType.Gigapede => 50,
			EnemyType.Squid3 => 3,
			EnemyType.Thorn => 0,
			EnemyType.Spider2 => 1,
			EnemyType.Ghostpede => 10,
			_ => throw new NotSupportedException($"{nameof(EnemyType)} '{enemyType}' is not supported."),
		};
	}
}
