namespace DevilDaggersInfo.Core.Wiki;

public static class DaggersV3_1
{
	public static readonly Dagger Default = new(GameVersion.V3_1, "Default", DaggerColors.Default, 0);
	public static readonly Dagger Bronze = new(GameVersion.V3_1, "Bronze", DaggerColors.Bronze, 60);
	public static readonly Dagger Silver = new(GameVersion.V3_1, "Silver", DaggerColors.Silver, 120);
	public static readonly Dagger Golden = new(GameVersion.V3_1, "Golden", DaggerColors.Golden, 250);
	public static readonly Dagger Devil = new(GameVersion.V3_1, "Devil", DaggerColors.Devil, 500);
	public static readonly Dagger Leviathan = new(GameVersion.V3_1, "Leviathan", DaggerColors.Leviathan, 1000);

	internal static readonly IReadOnlyList<Dagger> All = new List<Dagger>
	{
		Default,
		Bronze,
		Silver,
		Golden,
		Default,
		Leviathan,
	};
}
