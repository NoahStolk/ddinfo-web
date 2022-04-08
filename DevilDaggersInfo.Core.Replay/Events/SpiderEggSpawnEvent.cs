namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SpiderEggSpawnEvent(int EntityId, Vector3 Position) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)0x0a);

		bw.Write(0);
		bw.Write(Position);
		bw.Write(Vector3.Zero);
	}
}
