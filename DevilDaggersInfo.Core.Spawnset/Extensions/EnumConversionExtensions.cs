namespace DevilDaggersInfo.Core.Spawnset.Extensions;

public static class EnumConversionExtensions
{
	public static HandLevel ToHandLevel(this byte value) => value switch
	{
		< 2 => HandLevel.Level1,
		2 => HandLevel.Level2,
		3 => HandLevel.Level3,
		_ => HandLevel.Level4,
	};

	public static EnemyType ToEnemyType(this int value) => value switch
	{
		0 => EnemyType.Squid1,
		1 => EnemyType.Squid2,
		2 => EnemyType.Centipede,
		3 => EnemyType.Spider1,
		4 => EnemyType.Leviathan,
		5 => EnemyType.Gigapede,
		6 => EnemyType.Squid3,
		7 => EnemyType.Thorn,
		8 => EnemyType.Spider2,
		9 => EnemyType.Ghostpede,
		_ => EnemyType.Empty,
	};

	public static GameMode ToGameMode(this int value) => value switch
	{
		1 => GameMode.TimeAttack,
		2 => GameMode.Race,
		_ => GameMode.Default,
	};
}
