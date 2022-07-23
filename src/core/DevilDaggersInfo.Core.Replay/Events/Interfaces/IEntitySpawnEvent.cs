namespace DevilDaggersInfo.Core.Replay.Events.Interfaces;

public interface IEntitySpawnEvent : IEvent
{
	EntityType EntityType { get; }
}
