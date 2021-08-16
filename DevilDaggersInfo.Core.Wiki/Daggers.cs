namespace DevilDaggersInfo.Core.Wiki;

public static class Daggers
{
	public static readonly Dagger Default = new(GameVersionFlags.V1_0 | GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Default", DaggerColors.Default, 0);
	public static readonly Dagger Bronze = new(GameVersionFlags.V1_0 | GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Bronze", DaggerColors.Bronze, 60);
	public static readonly Dagger Silver = new(GameVersionFlags.V1_0 | GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Silver", DaggerColors.Silver, 120);
	public static readonly Dagger Golden = new(GameVersionFlags.V1_0 | GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Golden", DaggerColors.Golden, 250);
	public static readonly Dagger Devil = new(GameVersionFlags.V1_0 | GameVersionFlags.V2_0 | GameVersionFlags.V3_0 | GameVersionFlags.V3_1, "Devil", DaggerColors.Devil, 500);
	public static readonly Dagger Leviathan = new(GameVersionFlags.V3_1, "Leviathan", DaggerColors.Leviathan, 1000);

	private static readonly List<Dagger> _all = typeof(Daggers).GetFields().Where(f => f.FieldType == typeof(Dagger)).Select(f => (Dagger)f.GetValue(null)!).ToList();
	private static readonly List<Dagger> _allV1_0_V2_0_V3_0 = _all.Where(ddo => ddo != Leviathan).ToList();

	public static List<Dagger> GetDaggers(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 or GameVersion.V2_0 or GameVersion.V3_0 => _allV1_0_V2_0_V3_0,
		GameVersion.V3_1 => _all,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};

	public static Dagger GetDaggerFromTenthsOfMilliseconds(GameVersion gameVersion, int timeInTenthsOfMilliseconds)
		=> GetDaggerFromSeconds(gameVersion, timeInTenthsOfMilliseconds / 10000.0); // TODO: Use time extension.

	public static Dagger GetDaggerFromSeconds(GameVersion gameVersion, double timeInSeconds)
	{
		List<Dagger> daggers = GetDaggers(gameVersion);
		for (int i = daggers.Count - 1; i >= 0; i--)
		{
			if (timeInSeconds >= daggers[i].UnlockSecond)
				return daggers[i];
		}

		throw new ArgumentOutOfRangeException(nameof(timeInSeconds), $"Could not determine dagger based on time '{timeInSeconds:0.0000}'.");
	}
}
