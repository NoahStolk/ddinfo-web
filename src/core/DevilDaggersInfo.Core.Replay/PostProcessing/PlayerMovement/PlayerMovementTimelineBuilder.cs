using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.PlayerMovement;

public static class PlayerMovementTimelineBuilder
{
	public static PlayerMovementTimeline Build(float spawnTileHeight, ReplayEventsData replayEventsData)
	{
		const float playerHeight = 4;
		List<PlayerMovementSnapshot> snapshots = new()
		{
			new(0, new(0, spawnTileHeight + playerHeight, 0)),
		};

		int ticks = 0;
		foreach (IEvent e in replayEventsData.Events)
		{
			ticks++;

			if (e is EntityPositionEvent { EntityId: 0 } entityPositionEvent)
			{
				const float divisor = 16f;
				Vector3 position = new()
				{
					X = entityPositionEvent.Position.X / divisor,
					Y = entityPositionEvent.Position.Y / divisor + playerHeight,
					Z = entityPositionEvent.Position.Z / divisor,
				};
				snapshots.Add(new(ticks / 60f, position));
			}
		}

		return new(snapshots);
	}
}
