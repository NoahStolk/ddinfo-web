namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Upgrade(GameVersionFlags GameVersions, string Name, Color Color, byte Level, Damage DefaultDamage, Damage HomingDamage, UpgradeUnlock UpgradeUnlock)
	: DevilDaggersObject(GameVersions, Name, Color)
{
}
