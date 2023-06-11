using DevilDaggersInfo.Core.Replay.Events.Enums;
using DevilDaggersInfo.Core.Replay.Events.Interfaces;

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
	private ushort _skull1AliveCount;
	private ushort _skull2AliveCount;
	private ushort _skull3AliveCount;
	private ushort _spiderlingAliveCount;
	private ushort _skull4AliveCount;
	private ushort _squid1AliveCount;
	private ushort _squid2AliveCount;
	private ushort _squid3AliveCount;
	private ushort _centipedeAliveCount;
	private ushort _gigapedeAliveCount;
	private ushort _spider1AliveCount;
	private ushort _spider2AliveCount;
	private ushort _leviathanAliveCount;
	private ushort _orbAliveCount;
	private ushort _thornAliveCount;
	private ushort _ghostpedeAliveCount;
	private ushort _spiderEggAliveCount;
	private ushort _skull1KillCount;
	private ushort _skull2KillCount;
	private ushort _skull3KillCount;
	private ushort _spiderlingKillCount;
	private ushort _skull4KillCount;
	private ushort _squid1KillCount;
	private ushort _squid2KillCount;
	private ushort _squid3KillCount;
	private ushort _centipedeKillCount;
	private ushort _gigapedeKillCount;
	private ushort _spider1KillCount;
	private ushort _spider2KillCount;
	private ushort _leviathanKillCount;
	private ushort _orbKillCount;
	private ushort _thornKillCount;
	private ushort _ghostpedeKillCount;
	private ushort _spiderEggKillCount;

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
						case BoidType.Skull1: _skull1AliveCount++; break;
						case BoidType.Skull2: _skull2AliveCount++; break;
						case BoidType.Skull3: _skull3AliveCount++; break;
						case BoidType.Spiderling: _spiderlingAliveCount++; break;
						case BoidType.Skull4: _skull4AliveCount++; break;
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
					_leviathanAliveCount++;
					break;
				case PedeSpawnEvent pede:
					_enemiesAlive++;
					switch (pede.PedeType)
					{
						case PedeType.Centipede: _centipedeAliveCount++; break;
						case PedeType.Gigapede: _gigapedeAliveCount++; break;
						case PedeType.Ghostpede: _ghostpedeAliveCount++; break;
					}

					break;
				case SpiderEggSpawnEvent:
					_enemiesAlive++;
					_spiderEggAliveCount++;
					break;
				case SpiderSpawnEvent spider:
					_enemiesAlive++;
					switch (spider.SpiderType)
					{
						case SpiderType.Spider1: _spider1AliveCount++; break;
						case SpiderType.Spider2: _spider2AliveCount++; break;
					}

					break;
				case SquidSpawnEvent squid:
					_enemiesAlive++;
					switch (squid.SquidType)
					{
						case SquidType.Squid1: _squid1AliveCount++; break;
						case SquidType.Squid2: _squid2AliveCount++; break;
						case SquidType.Squid3: _squid3AliveCount++; break;
					}

					break;
				case ThornSpawnEvent:
					_enemiesAlive++;
					_thornAliveCount++;
					break;
				case IInputsEvent:
					currentTick++;
					if (currentTick % 60 == 0)
						entries.Add(FlushCurrentState());
					break;
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
		Skull1AliveCount: _skull1AliveCount,
		Skull2AliveCount: _skull2AliveCount,
		Skull3AliveCount: _skull3AliveCount,
		SpiderlingAliveCount: _spiderlingAliveCount,
		Skull4AliveCount: _skull4AliveCount,
		Squid1AliveCount: _squid1AliveCount,
		Squid2AliveCount: _squid2AliveCount,
		Squid3AliveCount: _squid3AliveCount,
		CentipedeAliveCount: _centipedeAliveCount,
		GigapedeAliveCount: _gigapedeAliveCount,
		Spider1AliveCount: _spider1AliveCount,
		Spider2AliveCount: _spider2AliveCount,
		LeviathanAliveCount: _leviathanAliveCount,
		OrbAliveCount: _orbAliveCount,
		ThornAliveCount: _thornAliveCount,
		GhostpedeAliveCount: _ghostpedeAliveCount,
		SpiderEggAliveCount: _spiderEggAliveCount,
		Skull1KillCount: _skull1KillCount,
		Skull2KillCount: _skull2KillCount,
		Skull3KillCount: _skull3KillCount,
		SpiderlingKillCount: _spiderlingKillCount,
		Skull4KillCount: _skull4KillCount,
		Squid1KillCount: _squid1KillCount,
		Squid2KillCount: _squid2KillCount,
		Squid3KillCount: _squid3KillCount,
		CentipedeKillCount: _centipedeKillCount,
		GigapedeKillCount: _gigapedeKillCount,
		Spider1KillCount: _spider1KillCount,
		Spider2KillCount: _spider2KillCount,
		LeviathanKillCount: _leviathanKillCount,
		OrbKillCount: _orbKillCount,
		ThornKillCount: _thornKillCount,
		GhostpedeKillCount: _ghostpedeKillCount,
		SpiderEggKillCount: _spiderEggKillCount);

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
		_skull1AliveCount = 0;
		_skull2AliveCount = 0;
		_skull3AliveCount = 0;
		_spiderlingAliveCount = 0;
		_skull4AliveCount = 0;
		_squid1AliveCount = 0;
		_squid2AliveCount = 0;
		_squid3AliveCount = 0;
		_centipedeAliveCount = 0;
		_gigapedeAliveCount = 0;
		_spider1AliveCount = 0;
		_spider2AliveCount = 0;
		_leviathanAliveCount = 0;
		_orbAliveCount = 0;
		_thornAliveCount = 0;
		_ghostpedeAliveCount = 0;
		_spiderEggAliveCount = 0;
		_skull1KillCount = 0;
		_skull2KillCount = 0;
		_skull3KillCount = 0;
		_spiderlingKillCount = 0;
		_skull4KillCount = 0;
		_squid1KillCount = 0;
		_squid2KillCount = 0;
		_squid3KillCount = 0;
		_centipedeKillCount = 0;
		_gigapedeKillCount = 0;
		_spider1KillCount = 0;
		_spider2KillCount = 0;
		_leviathanKillCount = 0;
		_orbKillCount = 0;
		_thornKillCount = 0;
		_ghostpedeKillCount = 0;
		_spiderEggKillCount = 0;
	}

	#endregion Utils
}
