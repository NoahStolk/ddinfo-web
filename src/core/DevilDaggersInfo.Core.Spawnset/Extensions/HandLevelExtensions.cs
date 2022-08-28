using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.Spawnsets;
using DevilDaggersInfo.Types.Core.Wiki;

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
		Upgrade upgrade = Upgrades.GetUpgrades(gameVersion).FirstOrDefault(u => u.Level == (byte)handLevel);
		return upgrade == default ? null : upgrade;
	}

	public static string ToDisplayString(this HandLevel handLevel) => handLevel switch
	{
		HandLevel.Level1 => "Level 1",
		HandLevel.Level2 => "Level 2",
		HandLevel.Level3 => "Level 3",
		HandLevel.Level4 => "Level 4",
		_ => throw new InvalidEnumConversionException(handLevel),
	};
}
