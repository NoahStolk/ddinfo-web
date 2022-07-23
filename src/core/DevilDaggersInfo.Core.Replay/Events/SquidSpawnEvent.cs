using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SquidSpawnEvent(int EntityId, SquidType SquidType, int A, Vector3 Position, Vector3 Direction, float RotationInRadians) : IEntitySpawnEvent
{
	public EntityType EntityType => SquidType switch
	{
		SquidType.Squid1 => EntityType.Squid1,
		SquidType.Squid2 => EntityType.Squid2,
		SquidType.Squid3 => EntityType.Squid3,
		_ => throw new InvalidEnumConversionException(SquidType),
	};

	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)(SquidType switch
		{
			SquidType.Squid1 => 0x03,
			SquidType.Squid2 => 0x04,
			SquidType.Squid3 => 0x05,
			_ => throw new InvalidEnumConversionException(SquidType),
		}));

		bw.Write(A);
		bw.Write(Position);
		bw.Write(Direction);
		bw.Write(RotationInRadians);
	}
}
