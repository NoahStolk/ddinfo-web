using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct DaggerSpawnEvent(int EntityId, int A, Int16Vec3 Position, Int16Mat3x3 Orientation, bool IsShot, DaggerType DaggerType) : IEntitySpawnEvent
{
	public EntityType EntityType => DaggerType switch
	{
		DaggerType.Level1 => EntityType.Level1Dagger,
		DaggerType.Level2 => EntityType.Level2Dagger,
		DaggerType.Level3 => EntityType.Level3Dagger,
		DaggerType.Level3Homing => EntityType.Level3HomingDagger,
		DaggerType.Level4 => EntityType.Level4Dagger,
		DaggerType.Level4Homing => EntityType.Level4HomingDagger,
		DaggerType.Level4HomingSplash => EntityType.Level4HomingSplash,
		_ => throw new InvalidEnumConversionException(DaggerType),
	};

	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x01);

		bw.Write(A);
		bw.Write(Position);
		bw.Write(Orientation);
		bw.Write(IsShot);
		bw.Write((byte)DaggerType);
	}
}
