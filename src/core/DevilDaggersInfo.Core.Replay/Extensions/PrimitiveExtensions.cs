namespace DevilDaggersInfo.Core.Replay.Extensions;

public static class PrimitiveExtensions
{
	public static DaggerType ToDaggerType(this byte value) => value switch
	{
		1 => DaggerType.Level1,
		2 => DaggerType.Level2,
		3 => DaggerType.Level3,
		4 => DaggerType.Level3Homing,
		5 => DaggerType.Level4,
		6 => DaggerType.Level4Homing,
		7 => DaggerType.Level4HomingSplash,
		_ => throw new InvalidOperationException($"Invalid {nameof(DaggerType)} '{value}'."),
	};

	public static BoidType ToBoidType(this byte value) => value switch
	{
		0x01 => BoidType.Skull1,
		0x02 => BoidType.Skull2,
		0x03 => BoidType.Skull3,
		0x04 => BoidType.Spiderling,
		0x05 => BoidType.Skull4,
		_ => throw new InvalidOperationException($"Invalid {nameof(BoidType)} '{value}'."),
	};

	public static ShootType ToShootType(this byte value) => value switch
	{
		0 => ShootType.None,
		1 => ShootType.Hold,
		2 => ShootType.Release,
		_ => throw new InvalidOperationException($"Invalid {nameof(ShootType)} '{value}'."),
	};

	public static JumpType ToJumpType(this byte value) => value switch
	{
		0 => JumpType.None,
		1 => JumpType.Hold,
		2 => JumpType.StartedPress,
		_ => throw new InvalidOperationException($"Invalid {nameof(JumpType)} '{value}'."),
	};
}
