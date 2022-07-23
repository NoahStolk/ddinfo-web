using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct EntityTargetEvent(int EntityId, Int16Vec3 TargetPosition) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x04);
		bw.Write(EntityId);
		bw.Write(TargetPosition);
	}
}
