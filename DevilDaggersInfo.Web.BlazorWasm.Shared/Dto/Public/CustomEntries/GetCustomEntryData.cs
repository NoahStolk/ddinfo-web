namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

public class GetCustomEntryData
{
	public int PlayerId { get; init; }

	public string PlayerName { get; init; } = null!;

	public string SpawnsetName { get; init; } = null!;

	public double Time { get; init; }

	public CustomLeaderboardDagger CustomLeaderboardDagger { get; init; }

	public int GemsCollected { get; init; }

	public int EnemiesKilled { get; init; }

	public int DaggersFired { get; init; }

	public int DaggersHit { get; init; }

	public int EnemiesAlive { get; init; }

	public int HomingStored { get; init; }

	public int HomingEaten { get; init; }

	public int GemsDespawned { get; init; }

	public int GemsEaten { get; init; }

	public int GemsTotal { get; init; }

	public byte DeathType { get; init; }

	public double LevelUpTime2 { get; init; }

	public double LevelUpTime3 { get; init; }

	public double LevelUpTime4 { get; init; }

	public DateTime SubmitDate { get; init; }

	public string? ClientVersion { get; init; }

	public int[]? GemsCollectedData { get; init; }
	public int[]? EnemiesKilledData { get; init; }
	public int[]? DaggersFiredData { get; init; }
	public int[]? DaggersHitData { get; init; }
	public int[]? EnemiesAliveData { get; init; }
	public int[]? HomingStoredData { get; init; }
	public int[]? HomingEatenData { get; init; }
	public int[]? GemsDespawnedData { get; init; }
	public int[]? GemsEatenData { get; init; }
	public int[]? GemsTotalData { get; init; }

	public ushort[]? Skull1sAliveData { get; init; }
	public ushort[]? Skull2sAliveData { get; init; }
	public ushort[]? Skull3sAliveData { get; init; }
	public ushort[]? SpiderlingsAliveData { get; init; }
	public ushort[]? Skull4sAliveData { get; init; }
	public ushort[]? Squid1sAliveData { get; init; }
	public ushort[]? Squid2sAliveData { get; init; }
	public ushort[]? Squid3sAliveData { get; init; }
	public ushort[]? CentipedesAliveData { get; init; }
	public ushort[]? GigapedesAliveData { get; init; }
	public ushort[]? Spider1sAliveData { get; init; }
	public ushort[]? Spider2sAliveData { get; init; }
	public ushort[]? LeviathansAliveData { get; init; }
	public ushort[]? OrbsAliveData { get; init; }
	public ushort[]? ThornsAliveData { get; init; }
	public ushort[]? GhostpedesAliveData { get; init; }
	public ushort[]? SpiderEggsAliveData { get; init; }

	public ushort[]? Skull1sKilledData { get; init; }
	public ushort[]? Skull2sKilledData { get; init; }
	public ushort[]? Skull3sKilledData { get; init; }
	public ushort[]? SpiderlingsKilledData { get; init; }
	public ushort[]? Skull4sKilledData { get; init; }
	public ushort[]? Squid1sKilledData { get; init; }
	public ushort[]? Squid2sKilledData { get; init; }
	public ushort[]? Squid3sKilledData { get; init; }
	public ushort[]? CentipedesKilledData { get; init; }
	public ushort[]? GigapedesKilledData { get; init; }
	public ushort[]? Spider1sKilledData { get; init; }
	public ushort[]? Spider2sKilledData { get; init; }
	public ushort[]? LeviathansKilledData { get; init; }
	public ushort[]? OrbsKilledData { get; init; }
	public ushort[]? ThornsKilledData { get; init; }
	public ushort[]? GhostpedesKilledData { get; init; }
	public ushort[]? SpiderEggsKilledData { get; init; }
}
