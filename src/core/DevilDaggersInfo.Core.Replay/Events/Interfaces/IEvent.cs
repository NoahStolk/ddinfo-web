namespace DevilDaggersInfo.Core.Replay.Events.Interfaces;

public interface IEvent
{
	void Write(BinaryWriter bw);
}
