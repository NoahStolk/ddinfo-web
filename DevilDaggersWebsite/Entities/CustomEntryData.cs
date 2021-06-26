using DevilDaggersWebsite.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DevilDaggersWebsite.Entities
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
		public byte[] HomingDaggersData { get; set; } = null!;
		public byte[] HomingDaggersEatenData { get; set; } = null!;
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

		public void Populate(List<GameState> gameStates)
		{
			// TODO: Add a smart way to compress this data:
			// If all values are 0, use an empty byte array.
			// If all values are under 2, write 0x00 header byte, then write all data as 1-bit numbers (8 values squashed into a single byte).
			// If all values are under 4, write 0x01 header byte, then write all data as 2-bit numbers (4 values squashed into a single byte).
			// If all values are under 16, write 0x02 header byte, then write all data as as 4-bit numbers (2 values squashed into a single byte).
			// If all values are under 256, write 0x03 header byte, then write all data as bytes.
			// If all values are under 65536, write 0x04 header byte, then write all data as ushorts.
			// Otherwise, write 0x05 header byte, then write all data as ints (dd doesn't use uints).
			GemsCollectedData = CompressData(gameStates.Select(gs => gs.GemsCollected));
			EnemiesKilledData = CompressData(gameStates.Select(gs => gs.EnemiesKilled));
			DaggersFiredData = CompressData(gameStates.Select(gs => gs.DaggersFired));
			DaggersHitData = CompressData(gameStates.Select(gs => gs.DaggersHit));
			EnemiesAliveData = CompressData(gameStates.Select(gs => gs.EnemiesAlive));
			HomingDaggersData = CompressData(gameStates.Select(gs => gs.HomingDaggers));
			HomingDaggersEatenData = CompressData(gameStates.Select(gs => gs.HomingDaggersEaten));
			GemsDespawnedData = CompressData(gameStates.Select(gs => gs.GemsDespawned));
			GemsEatenData = CompressData(gameStates.Select(gs => gs.GemsEaten));
			GemsTotalData = CompressData(gameStates.Select(gs => gs.GemsTotal));

			Skull1sAliveData = CompressData(gameStates.Select(gs => (int)gs.Skull1sAlive));
			Skull2sAliveData = CompressData(gameStates.Select(gs => (int)gs.Skull2sAlive));
			Skull3sAliveData = CompressData(gameStates.Select(gs => (int)gs.Skull3sAlive));
			SpiderlingsAliveData = CompressData(gameStates.Select(gs => (int)gs.SpiderlingsAlive));
			Skull4sAliveData = CompressData(gameStates.Select(gs => (int)gs.Skull4sAlive));
			Squid1sAliveData = CompressData(gameStates.Select(gs => (int)gs.Squid1sAlive));
			Squid2sAliveData = CompressData(gameStates.Select(gs => (int)gs.Squid2sAlive));
			Squid3sAliveData = CompressData(gameStates.Select(gs => (int)gs.Squid3sAlive));
			CentipedesAliveData = CompressData(gameStates.Select(gs => (int)gs.CentipedesAlive));
			GigapedesAliveData = CompressData(gameStates.Select(gs => (int)gs.GigapedesAlive));
			Spider1sAliveData = CompressData(gameStates.Select(gs => (int)gs.Spider1sAlive));
			Spider2sAliveData = CompressData(gameStates.Select(gs => (int)gs.Spider2sAlive));
			LeviathansAliveData = CompressData(gameStates.Select(gs => (int)gs.LeviathansAlive));
			OrbsAliveData = CompressData(gameStates.Select(gs => (int)gs.OrbsAlive));
			ThornsAliveData = CompressData(gameStates.Select(gs => (int)gs.ThornsAlive));
			GhostpedesAliveData = CompressData(gameStates.Select(gs => (int)gs.GhostpedesAlive));
			SpiderEggsAliveData = CompressData(gameStates.Select(gs => (int)gs.SpiderEggsAlive));

			Skull1sKilledData = CompressData(gameStates.Select(gs => (int)gs.Skull1sKilled));
			Skull2sKilledData = CompressData(gameStates.Select(gs => (int)gs.Skull2sKilled));
			Skull3sKilledData = CompressData(gameStates.Select(gs => (int)gs.Skull3sKilled));
			SpiderlingsKilledData = CompressData(gameStates.Select(gs => (int)gs.SpiderlingsKilled));
			Skull4sKilledData = CompressData(gameStates.Select(gs => (int)gs.Skull4sKilled));
			Squid1sKilledData = CompressData(gameStates.Select(gs => (int)gs.Squid1sKilled));
			Squid2sKilledData = CompressData(gameStates.Select(gs => (int)gs.Squid2sKilled));
			Squid3sKilledData = CompressData(gameStates.Select(gs => (int)gs.Squid3sKilled));
			CentipedesKilledData = CompressData(gameStates.Select(gs => (int)gs.CentipedesKilled));
			GigapedesKilledData = CompressData(gameStates.Select(gs => (int)gs.GigapedesKilled));
			Spider1sKilledData = CompressData(gameStates.Select(gs => (int)gs.Spider1sKilled));
			Spider2sKilledData = CompressData(gameStates.Select(gs => (int)gs.Spider2sKilled));
			LeviathansKilledData = CompressData(gameStates.Select(gs => (int)gs.LeviathansKilled));
			OrbsKilledData = CompressData(gameStates.Select(gs => (int)gs.OrbsKilled));
			ThornsKilledData = CompressData(gameStates.Select(gs => (int)gs.ThornsKilled));
			GhostpedesKilledData = CompressData(gameStates.Select(gs => (int)gs.GhostpedesKilled));
			SpiderEggsKilledData = CompressData(gameStates.Select(gs => (int)gs.SpiderEggsKilled));
		}

		private static byte[] CompressData(IEnumerable<int> data)
		{
			if (!data.Any() || data.All(i => i == 0))
				return Array.Empty<byte>();

			int max = data.Max();
			byte header = max switch
			{
				> ushort.MaxValue => 5,
				> byte.MaxValue => 4,
				> 16 => 3,
				> 4 => 2,
				> 2 => 1,
				_ => 0,
			};

			if (header == 4)
				return data.Select(i => (byte)i).Prepend(header).ToArray();

			if (header == 5)
				return data.Select(i => (ushort)i).SelectMany(BitConverter.GetBytes).Prepend(header).ToArray();

			throw new NotImplementedException();
		}
	}
}
