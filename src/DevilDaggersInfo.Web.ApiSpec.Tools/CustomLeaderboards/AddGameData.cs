namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public record AddGameData
{
	public required List<int> GemsCollected { get; init; }
	public required List<int> EnemiesKilled { get; init; }
	public required List<int> DaggersFired { get; init; }
	public required List<int> DaggersHit { get; init; }
	public required List<int> EnemiesAlive { get; init; }
	public required List<int> HomingStored { get; init; }
	public required List<int> HomingEaten { get; init; }
	public required List<int> GemsDespawned { get; init; }
	public required List<int> GemsEaten { get; init; }
	public required List<int> GemsTotal { get; init; }

	public required List<ushort> Skull1sAlive { get; init; }
	public required List<ushort> Skull2sAlive { get; init; }
	public required List<ushort> Skull3sAlive { get; init; }
	public required List<ushort> SpiderlingsAlive { get; init; }
	public required List<ushort> Skull4sAlive { get; init; }
	public required List<ushort> Squid1sAlive { get; init; }
	public required List<ushort> Squid2sAlive { get; init; }
	public required List<ushort> Squid3sAlive { get; init; }
	public required List<ushort> CentipedesAlive { get; init; }
	public required List<ushort> GigapedesAlive { get; init; }
	public required List<ushort> Spider1sAlive { get; init; }
	public required List<ushort> Spider2sAlive { get; init; }
	public required List<ushort> LeviathansAlive { get; init; }
	public required List<ushort> OrbsAlive { get; init; }
	public required List<ushort> ThornsAlive { get; init; }
	public required List<ushort> GhostpedesAlive { get; init; }
	public required List<ushort> SpiderEggsAlive { get; init; }

	public required List<ushort> Skull1sKilled { get; init; }
	public required List<ushort> Skull2sKilled { get; init; }
	public required List<ushort> Skull3sKilled { get; init; }
	public required List<ushort> SpiderlingsKilled { get; init; }
	public required List<ushort> Skull4sKilled { get; init; }
	public required List<ushort> Squid1sKilled { get; init; }
	public required List<ushort> Squid2sKilled { get; init; }
	public required List<ushort> Squid3sKilled { get; init; }
	public required List<ushort> CentipedesKilled { get; init; }
	public required List<ushort> GigapedesKilled { get; init; }
	public required List<ushort> Spider1sKilled { get; init; }
	public required List<ushort> Spider2sKilled { get; init; }
	public required List<ushort> LeviathansKilled { get; init; }
	public required List<ushort> OrbsKilled { get; init; }
	public required List<ushort> ThornsKilled { get; init; }
	public required List<ushort> GhostpedesKilled { get; init; }
	public required List<ushort> SpiderEggsKilled { get; init; }
}
