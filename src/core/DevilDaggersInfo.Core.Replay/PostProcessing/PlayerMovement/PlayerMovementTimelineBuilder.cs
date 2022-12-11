using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.PlayerMovement;

public static class PlayerMovementTimelineBuilder
{
	public static PlayerMovementTimeline Build(ReplayEventsData replayEventsData)
	{
		const float defaultHeight = 4; // This probably needs to be the same as the height of the player spawn tile.
		List<PlayerMovementSnapshot> snapshots = new()
		{
			new(default, new(0, defaultHeight, 0)),
		};

		int ticks = 0;
		foreach (IEvent @event in replayEventsData.Events)
		{
			ticks++;

			if (@event is not EntityPositionEvent { EntityId: 0 } entityPositionEvent)
				continue;

			const float divisor = 16f;
			Vector3 position = new()
			{
				X = entityPositionEvent.Position.X / divisor,
				Y = entityPositionEvent.Position.Y / divisor + defaultHeight,
				Z = entityPositionEvent.Position.Z / divisor,
			};
			snapshots.Add(new(ticks / 60f, position));
		}

		return new(snapshots);
	}
}
