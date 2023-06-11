namespace DevilDaggersInfo.Core.Replay.PostProcessing.Statistics;

public readonly record struct ReplayStatisticsEntry(
	int GemsCollected,
	int EnemiesKilled,
	int DaggersFired,
	int DaggersHit,
	int EnemiesAlive,
	int HomingStored,
	int HomingEaten,
	int GemsDespawned,
	int GemsEaten,
	int GemsTotal,
	ushort Skull1AliveCount,
	ushort Skull2AliveCount,
	ushort Skull3AliveCount,
	ushort SpiderlingAliveCount,
	ushort Skull4AliveCount,
	ushort Squid1AliveCount,
	ushort Squid2AliveCount,
	ushort Squid3AliveCount,
	ushort CentipedeAliveCount,
	ushort GigapedeAliveCount,
	ushort Spider1AliveCount,
	ushort Spider2AliveCount,
	ushort LeviathanAliveCount,
	ushort OrbAliveCount,
	ushort ThornAliveCount,
	ushort GhostpedeAliveCount,
	ushort SpiderEggAliveCount,
	ushort Skull1KillCount,
	ushort Skull2KillCount,
	ushort Skull3KillCount,
	ushort SpiderlingKillCount,
	ushort Skull4KillCount,
	ushort Squid1KillCount,
	ushort Squid2KillCount,
	ushort Squid3KillCount,
	ushort CentipedeKillCount,
	ushort GigapedeKillCount,
	ushort Spider1KillCount,
	ushort Spider2KillCount,
	ushort LeviathanKillCount,
	ushort OrbKillCount,
	ushort ThornKillCount,
	ushort GhostpedeKillCount,
	ushort SpiderEggKillCount);
