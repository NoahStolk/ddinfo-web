using DevilDaggersInfo.Types.Core.Replays;

namespace DevilDaggersInfo.Core.Replay.Events.Interfaces;

public interface IEntitySpawnEvent : IEvent
{
	int EntityId { get; }

	EntityType EntityType { get; }
}
