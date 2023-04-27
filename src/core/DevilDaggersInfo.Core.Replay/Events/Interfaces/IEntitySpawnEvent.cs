using DevilDaggersInfo.Core.Replay.Events.Enums;

namespace DevilDaggersInfo.Core.Replay.Events.Interfaces;

public interface IEntitySpawnEvent : IEvent
{
	int EntityId { get; }

	EntityType EntityType { get; }
}
