using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct EntityPositionEvent(int EntityId, Int16Vec3 Position) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x01);
		bw.Write(EntityId);
		bw.Write(Position);
	}
}
