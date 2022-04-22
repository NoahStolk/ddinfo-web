namespace DevilDaggersInfo.Core.Wiki.Structs;

public readonly record struct UpgradeUnlock(UpgradeUnlockType UpgradeUnlockType, int Value)
{
	public override string ToString() => $"{Value} {(UpgradeUnlockType == UpgradeUnlockType.Gems ? "gems" : "homing daggers")}";
}
