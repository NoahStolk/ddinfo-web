namespace DevilDaggersInfo.Core.Wiki;

public static class Enemies
{
	public static List<Enemy> GetEnemies(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => EnemiesV1_0.All,
		GameVersion.V2_0 => EnemiesV2_0.All,
		GameVersion.V3_0 => EnemiesV3_0.All,
		GameVersion.V3_1 => EnemiesV3_1.All,
		GameVersion.V3_2 => EnemiesV3_2.All,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};

	public static Enemy? GetEnemyByName(GameVersion gameVersion, string name)
		=> GetEnemies(gameVersion).Find(d => d.Name == name);
}
