namespace DevilDaggersInfo.Core.Wiki.Objects;

public record Upgrade(GameVersion GameVersion, string Name, Color Color, byte Level, Damage DefaultDamage, Damage HomingDamage, UpgradeUnlock UpgradeUnlock)
	: DevilDaggersObject(GameVersion, Name, Color)
{
}
