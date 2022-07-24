namespace DevilDaggersInfo.Core.Replay.PostProcessing.Timeline;

public record EnemyTimeline(int EntityId, IReadOnlyList<EnemyTimelineEvent> Events)
{
	public int? GetHp(int tick)
	{
		IEnumerable<EnemyTimelineEvent> events = Events.Where(e => e.Tick < tick).OrderBy(e => e.Tick);
		if (events.Any())
			return events.Last().Hp;

		return null;
	}
}
