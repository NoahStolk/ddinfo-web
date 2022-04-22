namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct LeviathanSpawnEvent(int EntityId, int A) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x0b);

		bw.Write(A);
	}
}
