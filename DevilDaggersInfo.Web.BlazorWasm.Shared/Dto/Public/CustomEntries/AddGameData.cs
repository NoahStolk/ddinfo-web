namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

public record AddGameData
{
	public int[] GemsCollected { get; init; } = Array.Empty<int>();
	public int[] EnemiesKilled { get; init; } = Array.Empty<int>();
	public int[] DaggersFired { get; init; } = Array.Empty<int>();
	public int[] DaggersHit { get; init; } = Array.Empty<int>();
	public int[] EnemiesAlive { get; init; } = Array.Empty<int>();
	public int[] HomingDaggers { get; set; } = Array.Empty<int>(); // Use set to get rid of negative values.
	public int[] HomingDaggersEaten { get; init; } = Array.Empty<int>();
	public int[] GemsDespawned { get; init; } = Array.Empty<int>();
	public int[] GemsEaten { get; init; } = Array.Empty<int>();
	public int[] GemsTotal { get; init; } = Array.Empty<int>();

	public ushort[] Skull1sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] Skull2sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] Skull3sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] SpiderlingsAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] Skull4sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] Squid1sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] Squid2sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] Squid3sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] CentipedesAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] GigapedesAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] Spider1sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] Spider2sAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] LeviathansAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] OrbsAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] ThornsAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] GhostpedesAlive { get; init; } = Array.Empty<ushort>();
	public ushort[] SpiderEggsAlive { get; init; } = Array.Empty<ushort>();

	public ushort[] Skull1sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] Skull2sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] Skull3sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] SpiderlingsKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] Skull4sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] Squid1sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] Squid2sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] Squid3sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] CentipedesKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] GigapedesKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] Spider1sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] Spider2sKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] LeviathansKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] OrbsKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] ThornsKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] GhostpedesKilled { get; init; } = Array.Empty<ushort>();
	public ushort[] SpiderEggsKilled { get; init; } = Array.Empty<ushort>();
}
