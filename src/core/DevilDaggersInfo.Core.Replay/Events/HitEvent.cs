namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct HitEvent(int EntityId, int B, int C) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x05);
		bw.Write(EntityId);
		bw.Write(B);
		bw.Write(C);
	}
}
