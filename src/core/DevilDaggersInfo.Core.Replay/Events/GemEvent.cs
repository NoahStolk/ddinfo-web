namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct GemEvent : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x06);
	}
}
