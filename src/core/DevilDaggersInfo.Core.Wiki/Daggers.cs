namespace DevilDaggersInfo.Core.Wiki;

public static class Daggers
{
	public static readonly Dagger Default = new(GameVersion.V3_2, "Default", DaggerColors.Default, 0);
	public static readonly Dagger Bronze = new(GameVersion.V3_2, "Bronze", DaggerColors.Bronze, 60);
	public static readonly Dagger Silver = new(GameVersion.V3_2, "Silver", DaggerColors.Silver, 120);
	public static readonly Dagger Golden = new(GameVersion.V3_2, "Golden", DaggerColors.Golden, 250);
	public static readonly Dagger Devil = new(GameVersion.V3_2, "Devil", DaggerColors.Devil, 500);
	public static readonly Dagger Leviathan = new(GameVersion.V3_2, "Leviathan", DaggerColors.Leviathan, 1000);

	public static readonly IReadOnlyList<Dagger> All = new List<Dagger>
	{
		Default,
		Bronze,
		Silver,
		Golden,
		Devil,
		Leviathan,
	};

	public static Dagger? GetDaggerByName(string name)
	{
		Dagger dagger = All.FirstOrDefault(d => d.Name == name);
		return dagger == default ? null : dagger;
	}

	public static Dagger GetDaggerFromSeconds(GameVersion gameVersion, double timeInSeconds)
	{
		// Exclude Leviathan dagger from V3 and earlier.
		int offset = gameVersion is GameVersion.V1_0 or GameVersion.V2_0 or GameVersion.V3_0 ? 2 : 1;
		for (int i = All.Count - offset; i >= 0; i--)
		{
			if (timeInSeconds >= All[i].UnlockSecond)
				return All[i];
		}

		throw new ArgumentOutOfRangeException(nameof(timeInSeconds), $"Could not determine dagger based on time '{timeInSeconds:0.0000}'.");
	}
}
