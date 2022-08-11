using DevilDaggersInfo.Core.Replay.Events.Interfaces;
using DevilDaggersInfo.Types.Core.Replays;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct ThornSpawnEvent(int EntityId, int A, Vector3 Position, float RotationInRadians) : IEntitySpawnEvent
{
	public EntityType EntityType => EntityType.Thorn;

	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x0d);

		bw.Write(A);
		bw.Write(Position);
		bw.Write(RotationInRadians);
	}
}
