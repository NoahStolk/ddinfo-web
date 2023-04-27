using DevilDaggersInfo.Core.Replay.Events.Enums;
using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay;

public class ReplayEventsData
{
	private readonly List<IEvent> _events = new();
	private readonly List<int> _eventOffsetsPerTick = new() { 0, 0 };
	private readonly List<EntityType> _entityTypes = new() { EntityType.Zero };

	public IReadOnlyList<IEvent> Events => _events;

	public IReadOnlyList<int> EventOffsetsPerTick => _eventOffsetsPerTick;

	public IReadOnlyList<EntityType> EntityTypes => _entityTypes;

	public int TickCount => EventOffsetsPerTick.Count - 1;

	public void Clear()
	{
		_events.Clear();

		_eventOffsetsPerTick.Clear();
		_eventOffsetsPerTick.Add(0);
		_eventOffsetsPerTick.Add(0);

		_entityTypes.Clear();
		_entityTypes.Add(EntityType.Zero);
	}

	public void AddEvent(IEvent e)
	{
		_events.Add(e);
		_eventOffsetsPerTick[^1]++;

		if (e is IInputsEvent)
			_eventOffsetsPerTick.Add(_events.Count);
		else if (e is IEntitySpawnEvent ese)
			_entityTypes.Add(ese.EntityType);
	}
}
