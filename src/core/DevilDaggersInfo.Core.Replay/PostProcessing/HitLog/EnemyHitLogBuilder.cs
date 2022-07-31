using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.HitLog;

public static class EnemyHitLogBuilder
{
	public static EnemyHitLog? Build(IReadOnlyList<IEvent> events, int enemyEntityId)
	{
		int currentTick = 0;
		EnemyHitLogBuildContext? buildContext = null;
		Dictionary<int, EntityType> daggers = new();

		foreach (IEvent e in events)
		{
			if (e is DaggerSpawnEvent dagger)
			{
				daggers.Add(dagger.EntityId, dagger.EntityType);
			}
			else if (e is IEntitySpawnEvent spawn)
			{
				if (spawn.EntityId != enemyEntityId)
					continue;

				buildContext = new(spawn.EntityType, currentTick);
			}
			else if (e is HitEvent hit && hit.EntityIdA == enemyEntityId)
			{
				if (buildContext == null)
					continue;

				EntityType? daggerEntityType = daggers.ContainsKey(hit.EntityIdB) ? daggers[hit.EntityIdB] : null;
				if (daggerEntityType == null)
					continue;

				int damage = buildContext.EntityType.GetDamage(daggerEntityType.Value);
				buildContext.CurrentHp -= damage;
				buildContext.Events.Add(new(currentTick, buildContext.CurrentHp, damage, daggerEntityType.Value.GetDaggerType(), hit.UserData));
			}
			else if (e is TransmuteEvent transmute && transmute.EntityId == enemyEntityId)
			{
				if (buildContext == null)
					continue;

				buildContext.Transmute();
			}
			else if (e is IInputsEvent)
			{
				currentTick++;
			}
		}

		if (buildContext == null)
			return null;

		List<EnemyHitLogEvent> hitLogEvents = buildContext.Events.ConvertAll(e => new EnemyHitLogEvent
		{
			DaggerType = e.DaggerType,
			Damage = e.Damage,
			Hp = e.Hp,
			Tick = e.Tick,
			UserData = e.UserData,
		});

		return new(enemyEntityId, buildContext.EntityType, buildContext.SpawnTick, hitLogEvents);
	}

	private sealed class EnemyHitLogBuildContext
	{
		public EnemyHitLogBuildContext(EntityType entityType, int spawnTick)
		{
			EntityType = entityType;
			SpawnTick = spawnTick;

			CurrentHp = entityType.GetInitialHp();
		}

		public EntityType EntityType { get; }

		public int SpawnTick { get; }

		public List<EnemyHitLogEvent> Events { get; } = new();

		public int CurrentHp { get; set; }

		public void Transmute()
		{
			CurrentHp = EntityType.GetInitialTransmuteHp();
		}
	}
}
