namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

[Obsolete("DDCL 1.8.3 will be removed.")]
public record AddGameData
{
	public List<int> GemsCollected { get; init; } = new();
	public List<int> EnemiesKilled { get; init; } = new();
	public List<int> DaggersFired { get; init; } = new();
	public List<int> DaggersHit { get; init; } = new();
	public List<int> EnemiesAlive { get; init; } = new();
	public List<int> HomingStored { get; init; } = new();
	public List<int> HomingEaten { get; init; } = new();
	public List<int> GemsDespawned { get; init; } = new();
	public List<int> GemsEaten { get; init; } = new();
	public List<int> GemsTotal { get; init; } = new();

	public List<ushort> Skull1sAlive { get; init; } = new();
	public List<ushort> Skull2sAlive { get; init; } = new();
	public List<ushort> Skull3sAlive { get; init; } = new();
	public List<ushort> SpiderlingsAlive { get; init; } = new();
	public List<ushort> Skull4sAlive { get; init; } = new();
	public List<ushort> Squid1sAlive { get; init; } = new();
	public List<ushort> Squid2sAlive { get; init; } = new();
	public List<ushort> Squid3sAlive { get; init; } = new();
	public List<ushort> CentipedesAlive { get; init; } = new();
	public List<ushort> GigapedesAlive { get; init; } = new();
	public List<ushort> Spider1sAlive { get; init; } = new();
	public List<ushort> Spider2sAlive { get; init; } = new();
	public List<ushort> LeviathansAlive { get; init; } = new();
	public List<ushort> OrbsAlive { get; init; } = new();
	public List<ushort> ThornsAlive { get; init; } = new();
	public List<ushort> GhostpedesAlive { get; init; } = new();
	public List<ushort> SpiderEggsAlive { get; init; } = new();

	public List<ushort> Skull1sKilled { get; init; } = new();
	public List<ushort> Skull2sKilled { get; init; } = new();
	public List<ushort> Skull3sKilled { get; init; } = new();
	public List<ushort> SpiderlingsKilled { get; init; } = new();
	public List<ushort> Skull4sKilled { get; init; } = new();
	public List<ushort> Squid1sKilled { get; init; } = new();
	public List<ushort> Squid2sKilled { get; init; } = new();
	public List<ushort> Squid3sKilled { get; init; } = new();
	public List<ushort> CentipedesKilled { get; init; } = new();
	public List<ushort> GigapedesKilled { get; init; } = new();
	public List<ushort> Spider1sKilled { get; init; } = new();
	public List<ushort> Spider2sKilled { get; init; } = new();
	public List<ushort> LeviathansKilled { get; init; } = new();
	public List<ushort> OrbsKilled { get; init; } = new();
	public List<ushort> ThornsKilled { get; init; } = new();
	public List<ushort> GhostpedesKilled { get; init; } = new();
	public List<ushort> SpiderEggsKilled { get; init; } = new();
}
