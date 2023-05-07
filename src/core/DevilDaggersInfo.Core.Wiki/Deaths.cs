namespace DevilDaggersInfo.Core.Wiki;

// TODO: Rewrite this class to not allocate memory on every call, and remove the skipUnknown parameter.
public static class Deaths
{
	public static IReadOnlyList<Death> GetDeaths(GameVersion gameVersion, bool skipUnknown = true)
	{
		IReadOnlyList<Death> all = gameVersion switch
		{
			GameVersion.V1_0 => DeathsV1_0.All,
			GameVersion.V2_0 => DeathsV2_0.All,
			GameVersion.V3_0 => DeathsV3_0.All,
			GameVersion.V3_1 => DeathsV3_1.All,
			GameVersion.V3_2 => DeathsV3_2.All,
			_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
		};

		if (skipUnknown)
			all = all.Where(d => !string.Equals(d.Name, "unknown", StringComparison.OrdinalIgnoreCase)).ToList();

		return all;
	}

	public static Death? GetDeathByName(GameVersion gameVersion, string name, bool skipUnknown = true)
	{
		Death death = GetDeaths(gameVersion, skipUnknown).FirstOrDefault(d => d.Name == name);
		return death == default ? null : death;
	}

	// TODO: Rename to GetDeathByType.
	public static Death? GetDeathByLeaderboardType(GameVersion gameVersion, byte leaderboardDeathType, bool skipUnknown = true)
	{
		Death death = GetDeaths(gameVersion, skipUnknown).FirstOrDefault(d => d.LeaderboardDeathType == leaderboardDeathType);
		return death == default ? null : death;
	}
}
