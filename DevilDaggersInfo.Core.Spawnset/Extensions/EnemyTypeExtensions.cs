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

	public static Color GetColor(this EnemyType enemyType, GameVersion gameVersion) => enemyType switch
	{
		EnemyType.Empty => EnemyColors.Void,
		EnemyType.Squid1 => EnemyColors.Squid1,
		EnemyType.Squid2 => EnemyColors.Squid2,
		EnemyType.Centipede => EnemyColors.Centipede,
		EnemyType.Spider1 => EnemyColors.Spider1,
		EnemyType.Leviathan => EnemyColors.Leviathan,
		EnemyType.Gigapede => gameVersion is GameVersion.V3_0 or GameVersion.V3_1 ? EnemyColors.Gigapede : EnemyColors.GigapedeRed,
		EnemyType.Squid3 => gameVersion is not GameVersion.V1_0 ? EnemyColors.Squid3 : EnemyColors.Unknown,
		EnemyType.Thorn => gameVersion is GameVersion.V3_0 or GameVersion.V3_1 ? EnemyColors.Thorn : gameVersion is GameVersion.V2_0 ? EnemyColors.Andras : EnemyColors.Unknown,
		EnemyType.Spider2 => gameVersion is not GameVersion.V1_0 ? EnemyColors.Spider2 : EnemyColors.Unknown,
		EnemyType.Ghostpede => gameVersion is GameVersion.V3_0 or GameVersion.V3_1 ? EnemyColors.Ghostpede : EnemyColors.Unknown,
		_ => throw new NotSupportedException($"{nameof(EnemyType)} '{enemyType}' is not supported."),
	};

	public static string GetName(this EnemyType enemyType, GameVersion gameVersion) => enemyType switch
	{
		EnemyType.Empty => "Empty",
		EnemyType.Squid1 => EnemiesV3_1.Squid1.Name,
		EnemyType.Squid2 => EnemiesV3_1.Squid2.Name,
		EnemyType.Centipede => EnemiesV3_1.Centipede.Name,
		EnemyType.Spider1 => EnemiesV3_1.Spider1.Name,
		EnemyType.Leviathan => EnemiesV3_1.Leviathan.Name,
		EnemyType.Gigapede => EnemiesV3_1.Gigapede.Name,
		EnemyType.Squid3 => gameVersion is not GameVersion.V1_0 ? EnemiesV3_1.Squid3.Name : "Unsupported",
		EnemyType.Thorn => gameVersion is GameVersion.V3_0 or GameVersion.V3_1 ? EnemiesV3_1.Thorn.Name : gameVersion is GameVersion.V2_0 ? EnemiesV2_0.Andras.Name : "Unsupported",
		EnemyType.Spider2 => gameVersion is not GameVersion.V1_0 ? EnemiesV3_1.Spider2.Name : "Unsupported",
		EnemyType.Ghostpede => gameVersion is GameVersion.V3_0 or GameVersion.V3_1 ? EnemiesV3_1.Ghostpede.Name : "Unsupported",
		_ => "Unsupported",
	};
}
