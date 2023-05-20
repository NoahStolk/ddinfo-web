namespace DevilDaggersInfo.Core.Replay.PostProcessing.Timeline;

public record EnemyTimeline(int EntityId, IReadOnlyList<EnemyTimelineEvent> Events)
{
	public int? GetHp(int tick)
	{
		// TODO: Optimize this.
		List<EnemyTimelineEvent> events = Events.Where(e => e.Tick < tick).OrderBy(e => e.Tick).ToList();
		if (events.Count > 0)
			return events[^1].Hp;

		return null;
	}
}
