namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct LeviathanSpawnEvent(int EntityId) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x0b);

		bw.Write(0);
	}
}
