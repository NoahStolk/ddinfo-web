namespace DevilDaggersInfo.Core.Replay.Events;

public interface IEntityEvent : IEvent
{
	int EntityId { get; }
}
