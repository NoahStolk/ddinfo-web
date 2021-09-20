namespace DevilDaggersInfo.Core.Wiki.Objects;

public readonly record struct Upgrade(GameVersion GameVersion, string Name, Color Color, byte Level, Damage DefaultDamage, Damage HomingDamage, UpgradeUnlock UpgradeUnlock);
