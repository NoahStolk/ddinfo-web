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
	{
		Enemy enemy = GetEnemies(gameVersion).Find(e => e.Name == name);
		return enemy == default ? null : enemy;
	}

	public static GameVersion? GetFirstAppearance(string enemyName)
		=> All.Where(e => e.Name == enemyName).OrderBy(e => e.GameVersion).FirstOrDefault().GameVersion;
}
