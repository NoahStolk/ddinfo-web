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
		public int Id { get; set; }

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
			// If all values are under 256, write 0x00 header byte, then write all data as bytes.
			// If all values are under 65536, write 0x01 header byte, then write all data as ushorts.
			// Otherwise, write 0x02 header byte, then write all data as ints (dd doesn't use uints).
			GemsCollectedData = gameStates.Select(gs => gs.GemsCollected).SelectMany(BitConverter.GetBytes).ToArray();
			EnemiesKilledData = gameStates.Select(gs => gs.EnemiesKilled).SelectMany(BitConverter.GetBytes).ToArray();
			DaggersFiredData = gameStates.Select(gs => gs.DaggersFired).SelectMany(BitConverter.GetBytes).ToArray();
			DaggersHitData = gameStates.Select(gs => gs.DaggersHit).SelectMany(BitConverter.GetBytes).ToArray();
			EnemiesAliveData = gameStates.Select(gs => gs.EnemiesAlive).SelectMany(BitConverter.GetBytes).ToArray();
			HomingDaggersData = gameStates.Select(gs => gs.HomingDaggers).SelectMany(BitConverter.GetBytes).ToArray();
			HomingDaggersEatenData = gameStates.Select(gs => gs.HomingDaggersEaten).SelectMany(BitConverter.GetBytes).ToArray();
			GemsDespawnedData = gameStates.Select(gs => gs.GemsDespawned).SelectMany(BitConverter.GetBytes).ToArray();
			GemsEatenData = gameStates.Select(gs => gs.GemsEaten).SelectMany(BitConverter.GetBytes).ToArray();
			GemsTotalData = gameStates.Select(gs => gs.GemsTotal).SelectMany(BitConverter.GetBytes).ToArray();

			Skull1sAliveData = gameStates.Select(gs => gs.Skull1sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			Skull2sAliveData = gameStates.Select(gs => gs.Skull2sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			Skull3sAliveData = gameStates.Select(gs => gs.Skull3sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			SpiderlingsAliveData = gameStates.Select(gs => gs.SpiderlingsAlive).SelectMany(BitConverter.GetBytes).ToArray();
			Skull4sAliveData = gameStates.Select(gs => gs.Skull4sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			Squid1sAliveData = gameStates.Select(gs => gs.Squid1sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			Squid2sAliveData = gameStates.Select(gs => gs.Squid2sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			Squid3sAliveData = gameStates.Select(gs => gs.Squid3sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			CentipedesAliveData = gameStates.Select(gs => gs.CentipedesAlive).SelectMany(BitConverter.GetBytes).ToArray();
			GigapedesAliveData = gameStates.Select(gs => gs.GigapedesAlive).SelectMany(BitConverter.GetBytes).ToArray();
			Spider1sAliveData = gameStates.Select(gs => gs.Spider1sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			Spider2sAliveData = gameStates.Select(gs => gs.Spider2sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			LeviathansAliveData = gameStates.Select(gs => gs.LeviathansAlive).SelectMany(BitConverter.GetBytes).ToArray();
			OrbsAliveData = gameStates.Select(gs => gs.OrbsAlive).SelectMany(BitConverter.GetBytes).ToArray();
			ThornsAliveData = gameStates.Select(gs => gs.ThornsAlive).SelectMany(BitConverter.GetBytes).ToArray();
			GhostpedesAliveData = gameStates.Select(gs => gs.GhostpedesAlive).SelectMany(BitConverter.GetBytes).ToArray();
			SpiderEggsAliveData = gameStates.Select(gs => gs.SpiderEggsAlive).SelectMany(BitConverter.GetBytes).ToArray();

			Skull1sKilledData = gameStates.Select(gs => gs.Skull1sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			Skull2sKilledData = gameStates.Select(gs => gs.Skull2sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			Skull3sKilledData = gameStates.Select(gs => gs.Skull3sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			SpiderlingsKilledData = gameStates.Select(gs => gs.SpiderlingsKilled).SelectMany(BitConverter.GetBytes).ToArray();
			Skull4sKilledData = gameStates.Select(gs => gs.Skull4sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			Squid1sKilledData = gameStates.Select(gs => gs.Squid1sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			Squid2sKilledData = gameStates.Select(gs => gs.Squid2sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			Squid3sKilledData = gameStates.Select(gs => gs.Squid3sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			CentipedesKilledData = gameStates.Select(gs => gs.CentipedesKilled).SelectMany(BitConverter.GetBytes).ToArray();
			GigapedesKilledData = gameStates.Select(gs => gs.GigapedesKilled).SelectMany(BitConverter.GetBytes).ToArray();
			Spider1sKilledData = gameStates.Select(gs => gs.Spider1sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			Spider2sKilledData = gameStates.Select(gs => gs.Spider2sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			LeviathansKilledData = gameStates.Select(gs => gs.LeviathansKilled).SelectMany(BitConverter.GetBytes).ToArray();
			OrbsKilledData = gameStates.Select(gs => gs.OrbsKilled).SelectMany(BitConverter.GetBytes).ToArray();
			ThornsKilledData = gameStates.Select(gs => gs.ThornsKilled).SelectMany(BitConverter.GetBytes).ToArray();
			GhostpedesKilledData = gameStates.Select(gs => gs.GhostpedesKilled).SelectMany(BitConverter.GetBytes).ToArray();
			SpiderEggsKilledData = gameStates.Select(gs => gs.SpiderEggsKilled).SelectMany(BitConverter.GetBytes).ToArray();
		}
	}
}
