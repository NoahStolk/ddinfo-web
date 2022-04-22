namespace DevilDaggersInfo.Core.Replay.Events;

// TODO: One of these vectors is likely the skull position when it is transmuted.
public readonly record struct TransmuteEvent(int EntityId, Int16Vec3 A, Int16Vec3 B, Int16Vec3 C, Int16Vec3 D) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x07);
		bw.Write(EntityId);
		bw.Write(A);
		bw.Write(B);
		bw.Write(C);
		bw.Write(D);
	}
}
