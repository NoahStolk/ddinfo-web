namespace DevilDaggersInfo.Core.Spawnset.Extensions;

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

	public static Color GetColor(this EnemyType enemyType) => enemyType switch
	{
		EnemyType.Empty => EnemyColors.Void,
		EnemyType.Squid1 => EnemyColors.Squid1,
		EnemyType.Squid2 => EnemyColors.Squid2,
		EnemyType.Centipede => EnemyColors.Centipede,
		EnemyType.Spider1 => EnemyColors.Spider1,
		EnemyType.Leviathan => EnemyColors.Leviathan,
		EnemyType.Gigapede => EnemyColors.Gigapede,
		EnemyType.Squid3 => EnemyColors.Squid3,
		EnemyType.Thorn => EnemyColors.Thorn,
		EnemyType.Spider2 => EnemyColors.Spider2,
		EnemyType.Ghostpede => EnemyColors.Ghostpede,
		_ => throw new NotSupportedException($"{nameof(EnemyType)} '{enemyType}' is not supported."),
	};
}
