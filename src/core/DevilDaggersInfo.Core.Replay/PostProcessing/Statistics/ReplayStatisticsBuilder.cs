using DevilDaggersInfo.Core.Replay.Events.Interfaces;
using DevilDaggersInfo.Types.Core.Replays;

namespace DevilDaggersInfo.Core.Replay.PostProcessing.Statistics;

public class ReplayStatisticsBuilder
{
	#region Fields

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

	#endregion Fields

	/// <summary>
	/// Builds statistics in the same manner as the "stats" memory block in game memory, but based purely on replay events.
	/// There is an entry for every second, as well as an additional entry at the end of the run.
	/// </summary>
	public List<ReplayStatisticsEntry> Build(List<IEvent> events)
	{
		Clear();
		int currentTick = 0;

		List<ReplayStatisticsEntry> entries = new() { FlushCurrentState() };
		foreach (IEvent e in events)
		{
			switch (e)
			{
				case BoidSpawnEvent boid:
					_enemiesAlive++;
					switch (boid.BoidType)
					{
						case BoidType.Skull1: _skull1sAlive++; break;
						case BoidType.Skull2: _skull2sAlive++; break;
						case BoidType.Skull3: _skull3sAlive++; break;
						case BoidType.Spiderling: _spiderlingsAlive++; break;
						case BoidType.Skull4: _skull4sAlive++; break;
					}

					break;
				case DaggerSpawnEvent:
					_daggersFired++;
					break;
				case GemEvent:
					_gemsCollected++;
					break;
				case HitEvent:
					// TODO: Look up entity ID.
					break;
				case LeviathanSpawnEvent:
					_enemiesAlive++;
					_leviathansAlive++;
					break;
				case PedeSpawnEvent pede:
					_enemiesAlive++;
					switch (pede.PedeType)
					{
						case PedeType.Centipede: _centipedesAlive++; break;
						case PedeType.Gigapede: _gigapedesAlive++; break;
						case PedeType.Ghostpede: _ghostpedesAlive++; break;
					}

					break;
				case SpiderEggSpawnEvent:
					_enemiesAlive++;
					_spiderEggsAlive++;
					break;
				case SpiderSpawnEvent spider:
					_enemiesAlive++;
					switch (spider.SpiderType)
					{
						case SpiderType.Spider1: _spider1sAlive++; break;
						case SpiderType.Spider2: _spider2sAlive++; break;
					}

					break;
				case SquidSpawnEvent squid:
					_enemiesAlive++;
					switch (squid.SquidType)
					{
						case SquidType.Squid1: _squid1sAlive++; break;
						case SquidType.Squid2: _squid2sAlive++; break;
						case SquidType.Squid3: _squid3sAlive++; break;
					}

					break;
				case ThornSpawnEvent:
					_enemiesAlive++;
					_thornsAlive++;
					break;
				case IInputsEvent:
				{
					currentTick++;
					if (currentTick % 60 == 0)
						entries.Add(FlushCurrentState());
					break;
				}
				case EndEvent:
					entries.Add(FlushCurrentState());
					break;
			}
		}

		return entries;
	}

	#region Utils

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

	private void Clear()
	{
		_gemsCollected = 0;
		_enemiesKilled = 0;
		_daggersFired = 0;
		_daggersHit = 0;
		_enemiesAlive = 0;
		_homingStored = 0;
		_homingEaten = 0;
		_gemsDespawned = 0;
		_gemsEaten = 0;
		_gemsTotal = 0;
		_skull1sAlive = 0;
		_skull2sAlive = 0;
		_skull3sAlive = 0;
		_spiderlingsAlive = 0;
		_skull4sAlive = 0;
		_squid1sAlive = 0;
		_squid2sAlive = 0;
		_squid3sAlive = 0;
		_centipedesAlive = 0;
		_gigapedesAlive = 0;
		_spider1sAlive = 0;
		_spider2sAlive = 0;
		_leviathansAlive = 0;
		_orbsAlive = 0;
		_thornsAlive = 0;
		_ghostpedesAlive = 0;
		_spiderEggsAlive = 0;
		_skull1sKilled = 0;
		_skull2sKilled = 0;
		_skull3sKilled = 0;
		_spiderlingsKilled = 0;
		_skull4sKilled = 0;
		_squid1sKilled = 0;
		_squid2sKilled = 0;
		_squid3sKilled = 0;
		_centipedesKilled = 0;
		_gigapedesKilled = 0;
		_spider1sKilled = 0;
		_spider2sKilled = 0;
		_leviathansKilled = 0;
		_orbsKilled = 0;
		_thornsKilled = 0;
		_ghostpedesKilled = 0;
		_spiderEggsKilled = 0;
	}

	#endregion Utils
}
