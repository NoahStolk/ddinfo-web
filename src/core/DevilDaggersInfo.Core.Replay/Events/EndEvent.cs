using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.Events;

public readonly record struct EndEvent : IEvent
{
	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)0x0b);
	}
}
