namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;

public class AddGameData
{
	public int[] GemsCollected { get; init; } = null!;
	public int[] EnemiesKilled { get; init; } = null!;
	public int[] DaggersFired { get; init; } = null!;
	public int[] DaggersHit { get; init; } = null!;
	public int[] EnemiesAlive { get; init; } = null!;
	public int[] HomingDaggers { get; set; } = null!; // Use set to get rid of negative values.
	public int[] HomingDaggersEaten { get; init; } = null!;
	public int[] GemsDespawned { get; init; } = null!;
	public int[] GemsEaten { get; init; } = null!;
	public int[] GemsTotal { get; init; } = null!;

	public ushort[] Skull1sAlive { get; init; } = null!;
	public ushort[] Skull2sAlive { get; init; } = null!;
	public ushort[] Skull3sAlive { get; init; } = null!;
	public ushort[] SpiderlingsAlive { get; init; } = null!;
	public ushort[] Skull4sAlive { get; init; } = null!;
	public ushort[] Squid1sAlive { get; init; } = null!;
	public ushort[] Squid2sAlive { get; init; } = null!;
	public ushort[] Squid3sAlive { get; init; } = null!;
	public ushort[] CentipedesAlive { get; init; } = null!;
	public ushort[] GigapedesAlive { get; init; } = null!;
	public ushort[] Spider1sAlive { get; init; } = null!;
	public ushort[] Spider2sAlive { get; init; } = null!;
	public ushort[] LeviathansAlive { get; init; } = null!;
	public ushort[] OrbsAlive { get; init; } = null!;
	public ushort[] ThornsAlive { get; init; } = null!;
	public ushort[] GhostpedesAlive { get; init; } = null!;
	public ushort[] SpiderEggsAlive { get; init; } = null!;

	public ushort[] Skull1sKilled { get; init; } = null!;
	public ushort[] Skull2sKilled { get; init; } = null!;
	public ushort[] Skull3sKilled { get; init; } = null!;
	public ushort[] SpiderlingsKilled { get; init; } = null!;
	public ushort[] Skull4sKilled { get; init; } = null!;
	public ushort[] Squid1sKilled { get; init; } = null!;
	public ushort[] Squid2sKilled { get; init; } = null!;
	public ushort[] Squid3sKilled { get; init; } = null!;
	public ushort[] CentipedesKilled { get; init; } = null!;
	public ushort[] GigapedesKilled { get; init; } = null!;
	public ushort[] Spider1sKilled { get; init; } = null!;
	public ushort[] Spider2sKilled { get; init; } = null!;
	public ushort[] LeviathansKilled { get; init; } = null!;
	public ushort[] OrbsKilled { get; init; } = null!;
	public ushort[] ThornsKilled { get; init; } = null!;
	public ushort[] GhostpedesKilled { get; init; } = null!;
	public ushort[] SpiderEggsKilled { get; init; } = null!;
}
