namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("CustomEntryData")]
public class CustomEntryDataEntity
{
	[Key]
	public int Id { get; init; }

	public int CustomEntryId { get; set; }

	[ForeignKey(nameof(CustomEntryId))]
	public CustomEntryEntity? CustomEntry { get; set; }

	public byte[] GemsCollectedData { get; set; } = Array.Empty<byte>();
	public byte[] EnemiesKilledData { get; set; } = Array.Empty<byte>();
	public byte[] DaggersFiredData { get; set; } = Array.Empty<byte>();
	public byte[] DaggersHitData { get; set; } = Array.Empty<byte>();
	public byte[] EnemiesAliveData { get; set; } = Array.Empty<byte>();
	public byte[] HomingStoredData { get; set; } = Array.Empty<byte>();
	public byte[] HomingEatenData { get; set; } = Array.Empty<byte>();
	public byte[] GemsDespawnedData { get; set; } = Array.Empty<byte>();
	public byte[] GemsEatenData { get; set; } = Array.Empty<byte>();
	public byte[] GemsTotalData { get; set; } = Array.Empty<byte>();

	public byte[] Skull1sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] Skull2sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] Skull3sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] SpiderlingsAliveData { get; set; } = Array.Empty<byte>();
	public byte[] Skull4sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] Squid1sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] Squid2sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] Squid3sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] CentipedesAliveData { get; set; } = Array.Empty<byte>();
	public byte[] GigapedesAliveData { get; set; } = Array.Empty<byte>();
	public byte[] Spider1sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] Spider2sAliveData { get; set; } = Array.Empty<byte>();
	public byte[] LeviathansAliveData { get; set; } = Array.Empty<byte>();
	public byte[] OrbsAliveData { get; set; } = Array.Empty<byte>();
	public byte[] ThornsAliveData { get; set; } = Array.Empty<byte>();
	public byte[] GhostpedesAliveData { get; set; } = Array.Empty<byte>();
	public byte[] SpiderEggsAliveData { get; set; } = Array.Empty<byte>();

	public byte[] Skull1sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] Skull2sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] Skull3sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] SpiderlingsKilledData { get; set; } = Array.Empty<byte>();
	public byte[] Skull4sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] Squid1sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] Squid2sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] Squid3sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] CentipedesKilledData { get; set; } = Array.Empty<byte>();
	public byte[] GigapedesKilledData { get; set; } = Array.Empty<byte>();
	public byte[] Spider1sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] Spider2sKilledData { get; set; } = Array.Empty<byte>();
	public byte[] LeviathansKilledData { get; set; } = Array.Empty<byte>();
	public byte[] OrbsKilledData { get; set; } = Array.Empty<byte>();
	public byte[] ThornsKilledData { get; set; } = Array.Empty<byte>();
	public byte[] GhostpedesKilledData { get; set; } = Array.Empty<byte>();
	public byte[] SpiderEggsKilledData { get; set; } = Array.Empty<byte>();
}
