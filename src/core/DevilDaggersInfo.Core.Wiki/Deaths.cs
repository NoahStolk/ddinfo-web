namespace DevilDaggersInfo.Core.Wiki;

public static class Deaths
{
	public static List<Death> GetDeaths(GameVersion gameVersion, bool skipUnknown = true)
	{
		List<Death> all = gameVersion switch
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
		Death death = GetDeaths(gameVersion, skipUnknown).Find(d => d.Name == name);
		return death == default ? null : death;
	}

	public static Death? GetDeathByLeaderboardType(GameVersion gameVersion, byte leaderboardDeathType, bool skipUnknown = true)
	{
		Death death = GetDeaths(gameVersion, skipUnknown).Find(d => d.LeaderboardDeathType == leaderboardDeathType);
		return death == default ? null : death;
	}
}
