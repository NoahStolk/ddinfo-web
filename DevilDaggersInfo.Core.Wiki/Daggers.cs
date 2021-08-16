namespace DevilDaggersInfo.Core.Wiki;

public static class Daggers
{
	public static readonly Dagger Default = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Default", DaggerColors.Default, 0);
	public static readonly Dagger Bronze = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Bronze", DaggerColors.Bronze, 60);
	public static readonly Dagger Silver = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Silver", DaggerColors.Silver, 120);
	public static readonly Dagger Golden = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Golden", DaggerColors.Golden, 250);
	public static readonly Dagger Devil = new(GameVersions.V1 | GameVersions.V2 | GameVersions.V3 | GameVersions.V31, "Devil", DaggerColors.Devil, 500);
	public static readonly Dagger Leviathan = new(GameVersions.V31, "Leviathan", DaggerColors.Leviathan, 1000);
}
