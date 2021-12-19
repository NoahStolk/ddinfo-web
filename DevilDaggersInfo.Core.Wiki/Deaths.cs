namespace DevilDaggersInfo.Core.Wiki;

public static class Deaths
{
	public static List<Death> GetDeaths(GameVersion gameVersion)
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

		return all
			.Where(d => !string.Equals(d.Name, "unknown", StringComparison.OrdinalIgnoreCase))
			.ToList();
	}

	public static Death? GetDeathByName(GameVersion gameVersion, string name)
	{
		Death death = GetDeaths(gameVersion).Find(d => d.Name == name);
		return death == default ? null : death;
	}

	public static Death? GetDeathByLeaderboardType(GameVersion gameVersion, byte leaderboardDeathType)
	{
		Death death = GetDeaths(gameVersion).Find(d => d.LeaderboardDeathType == leaderboardDeathType);
		return death == default ? null : death;
	}
}
