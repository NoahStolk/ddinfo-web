using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.BlazorWasm.Server.Entities
{
	public class CustomEntryData : IEntity
	{
		[Key]
		public int Id { get; init; }

		public int CustomEntryId { get; set; }

		[ForeignKey(nameof(CustomEntryId))]
		public CustomEntry CustomEntry { get; set; } = null!;

		public byte[] GemsCollectedData { get; set; } = null!;
		public byte[] EnemiesKilledData { get; set; } = null!;
		public byte[] DaggersFiredData { get; set; } = null!;
		public byte[] DaggersHitData { get; set; } = null!;
		public byte[] EnemiesAliveData { get; set; } = null!;
		public byte[] HomingDaggersData { get; set; } = null!; // TODO: Rename to HomingStoredData.
		public byte[] HomingDaggersEatenData { get; set; } = null!; // TODO: Rename to HomingEatenData.
		public byte[] GemsDespawnedData { get; set; } = null!;
		public byte[] GemsEatenData { get; set; } = null!;
		public byte[] GemsTotalData { get; set; } = null!;

		public byte[] Skull1sAliveData { get; set; } = null!;
		public byte[] Skull2sAliveData { get; set; } = null!;
		public byte[] Skull3sAliveData { get; set; } = null!;
		public byte[] SpiderlingsAliveData { get; set; } = null!;
		public byte[] Skull4sAliveData { get; set; } = null!;
		public byte[] Squid1sAliveData { get; set; } = null!;
		public byte[] Squid2sAliveData { get; set; } = null!;
		public byte[] Squid3sAliveData { get; set; } = null!;
		public byte[] CentipedesAliveData { get; set; } = null!;
		public byte[] GigapedesAliveData { get; set; } = null!;
		public byte[] Spider1sAliveData { get; set; } = null!;
		public byte[] Spider2sAliveData { get; set; } = null!;
		public byte[] LeviathansAliveData { get; set; } = null!;
		public byte[] OrbsAliveData { get; set; } = null!;
		public byte[] ThornsAliveData { get; set; } = null!;
		public byte[] GhostpedesAliveData { get; set; } = null!;
		public byte[] SpiderEggsAliveData { get; set; } = null!;

		public byte[] Skull1sKilledData { get; set; } = null!;
		public byte[] Skull2sKilledData { get; set; } = null!;
		public byte[] Skull3sKilledData { get; set; } = null!;
		public byte[] SpiderlingsKilledData { get; set; } = null!;
		public byte[] Skull4sKilledData { get; set; } = null!;
		public byte[] Squid1sKilledData { get; set; } = null!;
		public byte[] Squid2sKilledData { get; set; } = null!;
		public byte[] Squid3sKilledData { get; set; } = null!;
		public byte[] CentipedesKilledData { get; set; } = null!;
		public byte[] GigapedesKilledData { get; set; } = null!;
		public byte[] Spider1sKilledData { get; set; } = null!;
		public byte[] Spider2sKilledData { get; set; } = null!;
		public byte[] LeviathansKilledData { get; set; } = null!;
		public byte[] OrbsKilledData { get; set; } = null!;
		public byte[] ThornsKilledData { get; set; } = null!;
		public byte[] GhostpedesKilledData { get; set; } = null!;
		public byte[] SpiderEggsKilledData { get; set; } = null!;
	}
}
