namespace DevilDaggersInfo.Core.Replay.Events;

public interface IEvent
{
	void Write(BinaryWriter bw);
}
