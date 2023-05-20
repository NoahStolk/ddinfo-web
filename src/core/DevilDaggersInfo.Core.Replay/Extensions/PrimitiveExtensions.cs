using DevilDaggersInfo.Core.Replay.Events.Enums;

namespace DevilDaggersInfo.Core.Replay.Extensions;

public static class PrimitiveExtensions
{
	public static DaggerType ToDaggerType(this byte value) => value switch
	{
		0x01 => DaggerType.Level1,
		0x02 => DaggerType.Level2,
		0x03 => DaggerType.Level3,
		0x04 => DaggerType.Level3Homing,
		0x05 => DaggerType.Level4,
		0x06 => DaggerType.Level4Homing,
		0x07 => DaggerType.Level4HomingSplash,
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
		0x00 => ShootType.None,
		0x01 => ShootType.Hold,
		0x02 => ShootType.Release,
		_ => throw new InvalidOperationException($"Invalid {nameof(ShootType)} '{value}'."),
	};

	public static JumpType ToJumpType(this byte value) => value switch
	{
		0x00 => JumpType.None,
		0x01 => JumpType.Hold,
		0x02 => JumpType.StartedPress,
		_ => throw new InvalidOperationException($"Invalid {nameof(JumpType)} '{value}'."),
	};
}
