using DevilDaggersInfo.Core.Replay.Events.Enums;
using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.Timeline;

public class EnemyTimelineBuilder
{
	private readonly List<EnemyTimelineBuildContext> _builds = new();
	private readonly Dictionary<int, EntityType> _daggers = new();

	public List<EnemyTimeline> Build(IReadOnlyList<IEvent> events)
	{
		_builds.Clear();
		_daggers.Clear();
		int currentTick = 0;

		foreach (IEvent e in events)
		{
			if (e is DaggerSpawnEvent dagger)
			{
				_daggers.Add(dagger.EntityId, dagger.EntityType);
			}
			else if (e is IEntitySpawnEvent spawn)
			{
				_builds.Add(new(spawn.EntityId, spawn.EntityType, currentTick));
			}
			else if (e is HitEvent hit)
			{
				EnemyTimelineBuildContext? enemy = _builds.Find(c => c.EntityId == hit.EntityIdA);
				if (enemy == null)
					continue;

				EntityType? daggerType = _daggers.ContainsKey(hit.EntityIdB) ? _daggers[hit.EntityIdB] : null;
				if (daggerType == null)
					continue;

				int damage = enemy.EntityType.GetDamage(daggerType.Value, hit.UserData);
				if (damage == 0)
					continue;

				if (enemy.HpPerTick.ContainsKey(currentTick))
					enemy.HpPerTick[currentTick] -= damage;
				else
					enemy.HpPerTick.Add(currentTick, enemy.CurrentHp - damage);

				enemy.CurrentHp -= damage;
			}
			else if (e is TransmuteEvent transmute)
			{
				EnemyTimelineBuildContext? enemy = _builds.Find(c => c.EntityId == transmute.EntityId);
				if (enemy == null)
					continue;

				enemy.Transmute();
			}
			else if (e is IInputsEvent)
			{
				currentTick++;
			}
		}

		return _builds.ConvertAll(b => new EnemyTimeline(b.EntityId, b.HpPerTick.Select(d => new EnemyTimelineEvent(d.Key, d.Value)).ToList()));
	}

	private sealed class EnemyTimelineBuildContext
	{
		public EnemyTimelineBuildContext(int entityId, EntityType entityType, int currentTick)
		{
			EntityId = entityId;
			EntityType = entityType;

			CurrentHp = entityType.GetInitialHp();
			HpPerTick.Add(currentTick, CurrentHp);
		}

		public int EntityId { get; }

		public EntityType EntityType { get; }

		public int CurrentHp { get; set; }

		public Dictionary<int, int> HpPerTick { get; } = new();

		public void Transmute()
		{
			CurrentHp = EntityType.GetInitialTransmuteHp();
		}
	}
}
