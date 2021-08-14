using DevilDaggersInfo.Core.Wiki.Enums;
using DevilDaggersInfo.Core.Wiki.Structs;

namespace DevilDaggersInfo.Core.Wiki.Objects
{
	public record Upgrade(GameVersions GameVersions, string Name, Color Color, byte Level, Damage DefaultDamage, Damage HomingDamage, UpgradeUnlockType UpgradeUnlockType)
		: DevilDaggersObject(GameVersions, Name, Color)
	{
	}
}
