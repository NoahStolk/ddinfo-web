namespace DevilDaggersInfo.Web.Server.Domain.Entities;

[Table("CustomEntryData")]
public class CustomEntryDataEntity
{
	[Key]
	public int Id { get; init; }

	public int CustomEntryId { get; set; }

	[ForeignKey(nameof(CustomEntryId))]
	public CustomEntryEntity? CustomEntry { get; set; }

	public byte[] GemsCollectedData { get; set; } = [];
	public byte[] EnemiesKilledData { get; set; } = [];
	public byte[] DaggersFiredData { get; set; } = [];
	public byte[] DaggersHitData { get; set; } = [];
	public byte[] EnemiesAliveData { get; set; } = [];
	public byte[] HomingStoredData { get; set; } = [];
	public byte[] HomingEatenData { get; set; } = [];
	public byte[] GemsDespawnedData { get; set; } = [];
	public byte[] GemsEatenData { get; set; } = [];
	public byte[] GemsTotalData { get; set; } = [];

	public byte[] Skull1sAliveData { get; set; } = [];
	public byte[] Skull2sAliveData { get; set; } = [];
	public byte[] Skull3sAliveData { get; set; } = [];
	public byte[] SpiderlingsAliveData { get; set; } = [];
	public byte[] Skull4sAliveData { get; set; } = [];
	public byte[] Squid1sAliveData { get; set; } = [];
	public byte[] Squid2sAliveData { get; set; } = [];
	public byte[] Squid3sAliveData { get; set; } = [];
	public byte[] CentipedesAliveData { get; set; } = [];
	public byte[] GigapedesAliveData { get; set; } = [];
	public byte[] Spider1sAliveData { get; set; } = [];
	public byte[] Spider2sAliveData { get; set; } = [];
	public byte[] LeviathansAliveData { get; set; } = [];
	public byte[] OrbsAliveData { get; set; } = [];
	public byte[] ThornsAliveData { get; set; } = [];
	public byte[] GhostpedesAliveData { get; set; } = [];
	public byte[] SpiderEggsAliveData { get; set; } = [];

	public byte[] Skull1sKilledData { get; set; } = [];
	public byte[] Skull2sKilledData { get; set; } = [];
	public byte[] Skull3sKilledData { get; set; } = [];
	public byte[] SpiderlingsKilledData { get; set; } = [];
	public byte[] Skull4sKilledData { get; set; } = [];
	public byte[] Squid1sKilledData { get; set; } = [];
	public byte[] Squid2sKilledData { get; set; } = [];
	public byte[] Squid3sKilledData { get; set; } = [];
	public byte[] CentipedesKilledData { get; set; } = [];
	public byte[] GigapedesKilledData { get; set; } = [];
	public byte[] Spider1sKilledData { get; set; } = [];
	public byte[] Spider2sKilledData { get; set; } = [];
	public byte[] LeviathansKilledData { get; set; } = [];
	public byte[] OrbsKilledData { get; set; } = [];
	public byte[] ThornsKilledData { get; set; } = [];
	public byte[] GhostpedesKilledData { get; set; } = [];
	public byte[] SpiderEggsKilledData { get; set; } = [];
}
