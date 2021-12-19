using DevilDaggersInfo.Core.Wiki.Objects;

namespace DevilDaggersInfo.Core.Spawnset.Extensions;

public static class HandLevelExtensions
{
	public static int GetStartGems(this HandLevel handLevel) => handLevel switch
	{
		HandLevel.Level2 => 10,
		HandLevel.Level3 => 70,
		HandLevel.Level4 => 220,
		_ => 0,
	};

	public static Upgrade? GetUpgradeByHandLevel(this HandLevel handLevel, GameVersion gameVersion = GameConstants.CurrentVersion)
	{
		Upgrade upgrade = Upgrades.GetUpgrades(gameVersion).Find(u => u.Level == (byte)handLevel);
		return upgrade == default ? null : upgrade;
	}
}
