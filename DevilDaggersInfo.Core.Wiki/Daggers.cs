namespace DevilDaggersInfo.Core.Wiki;

public static class Daggers
{
	public static List<Dagger> GetDaggers(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => DaggersV1_0.All,
		GameVersion.V2_0 => DaggersV2_0.All,
		GameVersion.V3_0 => DaggersV3_0.All,
		GameVersion.V3_1 => DaggersV3_1.All,
		GameVersion.V3_2 => DaggersV3_2.All,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};

	public static Dagger? GetDaggerByName(GameVersion gameVersion, string name)
		=> GetDaggers(gameVersion).Find(d => d.Name == name);

	public static Dagger GetDaggerFromTenthsOfMilliseconds(GameVersion gameVersion, int timeInTenthsOfMilliseconds)
		=> GetDaggerFromSeconds(gameVersion, timeInTenthsOfMilliseconds.ToSecondsTime());

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
