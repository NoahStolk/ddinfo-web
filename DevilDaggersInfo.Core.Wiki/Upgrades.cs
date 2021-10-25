namespace DevilDaggersInfo.Core.Wiki;

public static class Upgrades
{
	// TODO: Figure out Level 3 homing spray for V1.
	public static List<Upgrade> GetUpgrades(GameVersion gameVersion) => gameVersion switch
	{
		GameVersion.V1_0 => UpgradesV1_0.All,
		GameVersion.V2_0 => UpgradesV2_0.All,
		GameVersion.V3_0 => UpgradesV3_0.All,
		GameVersion.V3_1 => UpgradesV3_1.All,
		_ => throw new ArgumentOutOfRangeException(nameof(gameVersion)),
	};
}
