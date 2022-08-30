using DevilDaggersInfo.Core.Wiki.Objects;
using DevilDaggersInfo.Types.Core.Spawnsets;

namespace DevilDaggersInfo.Core.Spawnset.Extensions;

public static class HandLevelExtensions
{
	public static Upgrade? GetUpgradeByHandLevel(this HandLevel handLevel)
	{
		Upgrade upgrade = Upgrades.All.FirstOrDefault(u => u.Level == (byte)handLevel);
		return upgrade == default ? null : upgrade;
	}
}
