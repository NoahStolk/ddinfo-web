namespace DevilDaggersInfo.Core.Wiki;

public static class Deaths
{
	public static List<Death> GetDeaths(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => DeathsV1_0.All,
		GameVersion.V2_0 => DeathsV2_0.All,
		GameVersion.V3_0 => DeathsV3_0.All,
		GameVersion.V3_1 => DeathsV3_1.All,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};

	public static Death? GetDeathByName(GameVersion gameVersion, string name)
		=> GetDeaths(gameVersion).Find(d => d.Name == name);

	public static Death? GetDeathByLeaderboardType(GameVersion gameVersion, byte leaderboardDeathType)
		=> GetDeaths(gameVersion).Find(e => e.LeaderboardDeathType == leaderboardDeathType);
}
