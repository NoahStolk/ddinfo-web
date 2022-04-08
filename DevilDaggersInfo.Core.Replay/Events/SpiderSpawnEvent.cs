namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct SpiderSpawnEvent(int EntityId, SpiderType SpiderType, int A, Vector3 Position) : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x00);
		bw.Write((byte)(SpiderType switch
		{
			SpiderType.Spider1 => 0x08,
			SpiderType.Spider2 => 0x09,
			_ => throw new InvalidOperationException($"Invalid {nameof(SpiderType)} '{SpiderType}'."),
		}));

		bw.Write(A);
		bw.Write(Position);
	}
}
