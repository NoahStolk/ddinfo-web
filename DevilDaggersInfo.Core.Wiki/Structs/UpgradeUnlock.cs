namespace DevilDaggersInfo.Core.Wiki.Structs;

public struct UpgradeUnlock
{
	public UpgradeUnlock(UpgradeUnlockType upgradeUnlockType, int value)
	{
		UpgradeUnlockType = upgradeUnlockType;
		Value = value;
	}

	public UpgradeUnlockType UpgradeUnlockType { get; }
	public int Value { get; }
}
