using DevilDaggersInfo.Core.Replay.Events.Interfaces;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.Statistics;

public class ReplayStatisticsBuilder
{
	private int _gemsCollected;
	private int _enemiesKilled;
	private int _daggersFired;
	private int _daggersHit;
	private int _enemiesAlive;
	private int _homingStored;
	private int _homingEaten;
	private int _gemsDespawned;
	private int _gemsEaten;
	private int _gemsTotal;
	private ushort _skull1sAlive;
	private ushort _skull2sAlive;
	private ushort _skull3sAlive;
	private ushort _spiderlingsAlive;
	private ushort _skull4sAlive;
	private ushort _squid1sAlive;
	private ushort _squid2sAlive;
	private ushort _squid3sAlive;
	private ushort _centipedesAlive;
	private ushort _gigapedesAlive;
	private ushort _spider1sAlive;
	private ushort _spider2sAlive;
	private ushort _leviathansAlive;
	private ushort _orbsAlive;
	private ushort _thornsAlive;
	private ushort _ghostpedesAlive;
	private ushort _spiderEggsAlive;
	private ushort _skull1sKilled;
	private ushort _skull2sKilled;
	private ushort _skull3sKilled;
	private ushort _spiderlingsKilled;
	private ushort _skull4sKilled;
	private ushort _squid1sKilled;
	private ushort _squid2sKilled;
	private ushort _squid3sKilled;
	private ushort _centipedesKilled;
	private ushort _gigapedesKilled;
	private ushort _spider1sKilled;
	private ushort _spider2sKilled;
	private ushort _leviathansKilled;
	private ushort _orbsKilled;
	private ushort _thornsKilled;
	private ushort _ghostpedesKilled;
	private ushort _spiderEggsKilled;

	private ReplayStatisticsEntry FlushCurrentState() => new(
		GemsCollected: _gemsCollected,
		EnemiesKilled: _enemiesKilled,
		DaggersFired: _daggersFired,
		DaggersHit: _daggersHit,
		EnemiesAlive: _enemiesAlive,
		HomingStored: _homingStored,
		HomingEaten: _homingEaten,
		GemsDespawned: _gemsDespawned,
		GemsEaten: _gemsEaten,
		GemsTotal: _gemsTotal,
		Skull1sAlive: _skull1sAlive,
		Skull2sAlive: _skull2sAlive,
		Skull3sAlive: _skull3sAlive,
		SpiderlingsAlive: _spiderlingsAlive,
		Skull4sAlive: _skull4sAlive,
		Squid1sAlive: _squid1sAlive,
		Squid2sAlive: _squid2sAlive,
		Squid3sAlive: _squid3sAlive,
		CentipedesAlive: _centipedesAlive,
		GigapedesAlive: _gigapedesAlive,
		Spider1sAlive: _spider1sAlive,
		Spider2sAlive: _spider2sAlive,
		LeviathansAlive: _leviathansAlive,
		OrbsAlive: _orbsAlive,
		ThornsAlive: _thornsAlive,
		GhostpedesAlive: _ghostpedesAlive,
		SpiderEggsAlive: _spiderEggsAlive,
		Skull1sKilled: _skull1sKilled,
		Skull2sKilled: _skull2sKilled,
		Skull3sKilled: _skull3sKilled,
		SpiderlingsKilled: _spiderlingsKilled,
		Skull4sKilled: _skull4sKilled,
		Squid1sKilled: _squid1sKilled,
		Squid2sKilled: _squid2sKilled,
		Squid3sKilled: _squid3sKilled,
		CentipedesKilled: _centipedesKilled,
		GigapedesKilled: _gigapedesKilled,
		Spider1sKilled: _spider1sKilled,
		Spider2sKilled: _spider2sKilled,
		LeviathansKilled: _leviathansKilled,
		OrbsKilled: _orbsKilled,
		ThornsKilled: _thornsKilled,
		GhostpedesKilled: _ghostpedesKilled,
		SpiderEggsKilled: _spiderEggsKilled);

	/// <summary>
	/// Builds statistics in the same manner as the "stats" memory block in game memory, but based purely on replay events.
	/// There is an entry for every second, as well as an additional entry at the end of the run.
	/// </summary>
	public List<ReplayStatisticsEntry> Build(List<IEvent> events)
	{
		int currentTick = 0;

		List<ReplayStatisticsEntry> entries = new() { FlushCurrentState() };
		foreach (IEvent e in events)
		{
			if (e is BoidSpawnEvent boid)
			{
				_enemiesAlive++;
				switch (boid.BoidType)
				{
					case BoidType.Skull1: _skull1sAlive++; break;
					case BoidType.Skull2: _skull2sAlive++; break;
					case BoidType.Skull3: _skull3sAlive++; break;
					case BoidType.Spiderling: _spiderlingsAlive++; break;
					case BoidType.Skull4: _skull4sAlive++; break;
				}
			}
			else if (e is DaggerSpawnEvent)
			{
				_daggersFired++;
			}
			else if (e is GemEvent)
			{
				_gemsCollected++;
			}
			else if (e is HitEvent)
			{
				// TODO: Look up entity ID.
			}
			else if (e is LeviathanSpawnEvent)
			{
				_enemiesAlive++;
				_leviathansAlive++;
			}
			else if (e is PedeSpawnEvent pede)
			{
				_enemiesAlive++;
				switch (pede.PedeType)
				{
					case PedeType.Centipede: _centipedesAlive++; break;
					case PedeType.Gigapede: _gigapedesAlive++; break;
					case PedeType.Ghostpede: _ghostpedesAlive++; break;
				}
			}
			else if (e is SpiderEggSpawnEvent)
			{
				_enemiesAlive++;
				_spiderEggsAlive++;
			}
			else if (e is SpiderSpawnEvent spider)
			{
				_enemiesAlive++;
				switch (spider.SpiderType)
				{
					case SpiderType.Spider1: _spider1sAlive++; break;
					case SpiderType.Spider2: _spider2sAlive++; break;
				}
			}
			else if (e is SquidSpawnEvent squid)
			{
				_enemiesAlive++;
				switch (squid.SquidType)
				{
					case SquidType.Squid1: _squid1sAlive++; break;
					case SquidType.Squid2: _squid2sAlive++; break;
					case SquidType.Squid3: _squid3sAlive++; break;
				}
			}
			else if (e is ThornSpawnEvent)
			{
				_enemiesAlive++;
				_thornsAlive++;
			}
			else if (e is IInputsEvent)
			{
				currentTick++;
				if (currentTick % 60 == 0)
					entries.Add(FlushCurrentState());
			}
			else if (e is EndEvent)
			{
				entries.Add(FlushCurrentState());
			}
		}

		return entries;
	}
}
