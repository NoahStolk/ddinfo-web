namespace DevilDaggersInfo.Core.Wiki;

public static class DaggersV2_0
{
	public static readonly Dagger Default = new(GameVersion.V2_0, "Default", DaggerColors.Default, 0);
	public static readonly Dagger Bronze = new(GameVersion.V2_0, "Bronze", DaggerColors.Bronze, 60);
	public static readonly Dagger Silver = new(GameVersion.V2_0, "Silver", DaggerColors.Silver, 120);
	public static readonly Dagger Golden = new(GameVersion.V2_0, "Golden", DaggerColors.Golden, 250);
	public static readonly Dagger Devil = new(GameVersion.V2_0, "Devil", DaggerColors.Devil, 500);

	internal static readonly IReadOnlyList<Dagger> All = new List<Dagger>
	{
		Default,
		Bronze,
		Silver,
		Golden,
		Default,
	};
}
