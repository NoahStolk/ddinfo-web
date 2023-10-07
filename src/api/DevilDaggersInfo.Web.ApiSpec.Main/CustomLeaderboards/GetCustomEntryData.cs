using DevilDaggersInfo.Web.ApiSpec.Main.Spawnsets;

namespace DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;

public record GetCustomEntryData
{
	public required int CustomEntryId { get; init; }

	public required int PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required string SpawnsetName { get; init; }

	public required double Time { get; init; }

	public required CustomLeaderboardDagger? CustomLeaderboardDagger { get; init; }

	public required int GemsCollected { get; init; }

	public required int EnemiesKilled { get; init; }

	public required int DaggersFired { get; init; }

	public required int DaggersHit { get; init; }

	public required int EnemiesAlive { get; init; }

	public required int HomingStored { get; init; }

	public required int HomingEaten { get; init; }

	public required int GemsDespawned { get; init; }

	public required int GemsEaten { get; init; }

	public required int GemsTotal { get; init; }

	public required byte DeathType { get; init; }

	public required double LevelUpTime2 { get; init; }

	public required double LevelUpTime3 { get; init; }

	public required double LevelUpTime4 { get; init; }

	public required DateTime SubmitDate { get; init; }

	public required string? ClientVersion { get; init; }

	public required int[]? GemsCollectedData { get; init; }
	public required int[]? EnemiesKilledData { get; init; }
	public required int[]? DaggersFiredData { get; init; }
	public required int[]? DaggersHitData { get; init; }
	public required int[]? EnemiesAliveData { get; init; }
	public required int[]? HomingStoredData { get; init; }
	public required int[]? HomingEatenData { get; init; }
	public required int[]? GemsDespawnedData { get; init; }
	public required int[]? GemsEatenData { get; init; }
	public required int[]? GemsTotalData { get; init; }

	public required ushort[]? Skull1sAliveData { get; init; }
	public required ushort[]? Skull2sAliveData { get; init; }
	public required ushort[]? Skull3sAliveData { get; init; }
	public required ushort[]? SpiderlingsAliveData { get; init; }
	public required ushort[]? Skull4sAliveData { get; init; }
	public required ushort[]? Squid1sAliveData { get; init; }
	public required ushort[]? Squid2sAliveData { get; init; }
	public required ushort[]? Squid3sAliveData { get; init; }
	public required ushort[]? CentipedesAliveData { get; init; }
	public required ushort[]? GigapedesAliveData { get; init; }
	public required ushort[]? Spider1sAliveData { get; init; }
	public required ushort[]? Spider2sAliveData { get; init; }
	public required ushort[]? LeviathansAliveData { get; init; }
	public required ushort[]? OrbsAliveData { get; init; }
	public required ushort[]? ThornsAliveData { get; init; }
	public required ushort[]? GhostpedesAliveData { get; init; }
	public required ushort[]? SpiderEggsAliveData { get; init; }

	public required ushort[]? Skull1sKilledData { get; init; }
	public required ushort[]? Skull2sKilledData { get; init; }
	public required ushort[]? Skull3sKilledData { get; init; }
	public required ushort[]? SpiderlingsKilledData { get; init; }
	public required ushort[]? Skull4sKilledData { get; init; }
	public required ushort[]? Squid1sKilledData { get; init; }
	public required ushort[]? Squid2sKilledData { get; init; }
	public required ushort[]? Squid3sKilledData { get; init; }
	public required ushort[]? CentipedesKilledData { get; init; }
	public required ushort[]? GigapedesKilledData { get; init; }
	public required ushort[]? Spider1sKilledData { get; init; }
	public required ushort[]? Spider2sKilledData { get; init; }
	public required ushort[]? LeviathansKilledData { get; init; }
	public required ushort[]? OrbsKilledData { get; init; }
	public required ushort[]? ThornsKilledData { get; init; }
	public required ushort[]? GhostpedesKilledData { get; init; }
	public required ushort[]? SpiderEggsKilledData { get; init; }

	public required HandLevel StartingLevel { get; init; }

	public required bool HasReplay { get; init; }
}
