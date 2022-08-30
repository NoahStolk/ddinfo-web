namespace DevilDaggersInfo.Core.Wiki;

public static class Enemies
{
	private static readonly List<Enemy> _all = new();

	static Enemies()
	{
		_all.AddRange(EnemiesV1_0.All);
		_all.AddRange(EnemiesV2_0.All);
		_all.AddRange(EnemiesV3_0.All);
		_all.AddRange(EnemiesV3_1.All);
		_all.AddRange(EnemiesV3_2.All);
	}

	public static IReadOnlyList<Enemy> All => _all;

	public static IReadOnlyList<Enemy> GetEnemies(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => EnemiesV1_0.All,
		GameVersion.V2_0 => EnemiesV2_0.All,
		GameVersion.V3_0 => EnemiesV3_0.All,
		GameVersion.V3_1 => EnemiesV3_1.All,
		GameVersion.V3_2 => EnemiesV3_2.All,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};

	public static Enemy? GetEnemyByName(GameVersion gameVersion, string name)
		=> GetEnemies(gameVersion).FirstOrDefault(e => e.Name == name);

	public static IEnumerable<GameVersion> GetAppearances(string enemyName)
	{
		if (EnemiesV1_0.All.Any(e => e.Name == enemyName))
			yield return GameVersion.V1_0;
		if (EnemiesV2_0.All.Any(e => e.Name == enemyName))
			yield return GameVersion.V2_0;
		if (EnemiesV3_0.All.Any(e => e.Name == enemyName))
			yield return GameVersion.V3_0;
	}
}
