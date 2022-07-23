using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct HitEvent(int EntityIdA, int EntityIdB, int C) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x05);
		bw.Write(EntityIdA);
		bw.Write(EntityIdB);
		bw.Write(C);
	}
}
