﻿namespace DevilDaggersInfo.Core.Wiki;

public static class Daggers
{
	public static readonly Dagger Default = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "Default", DaggerColors.Default, 0);
	public static readonly Dagger Bronze = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "Bronze", DaggerColors.Bronze, 60);
	public static readonly Dagger Silver = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "Silver", DaggerColors.Silver, 120);
	public static readonly Dagger Golden = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "Golden", DaggerColors.Golden, 250);
	public static readonly Dagger Devil = new(GameVersions.V1_0 | GameVersions.V2_0 | GameVersions.V3_0 | GameVersions.V3_1, "Devil", DaggerColors.Devil, 500);
	public static readonly Dagger Leviathan = new(GameVersions.V3_1, "Leviathan", DaggerColors.Leviathan, 1000);

	private static readonly List<Dagger> _all = typeof(Daggers).GetFields().Where(f => f.FieldType == typeof(Dagger)).Select(f => (Dagger)f.GetValue(null)!).ToList();
	private static readonly List<Dagger> _allV1_0_V2_0_V3_0 = _all.Where(ddo => ddo != Leviathan).ToList();

	public static List<Dagger> GetDaggers(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 or GameVersion.V2_0 or GameVersion.V3_0 => _allV1_0_V2_0_V3_0,
		GameVersion.V3_1 => _all,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};
}
