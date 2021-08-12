using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Utils;
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
		public byte[] HomingStoredData { get; set; } = null!;
		public byte[] HomingEatenData { get; set; } = null!;
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
			GemsCollectedData = CompressProperty(gs => gs.GemsCollected);
			EnemiesKilledData = CompressProperty(gs => gs.EnemiesKilled);
			DaggersFiredData = CompressProperty(gs => gs.DaggersFired);
			DaggersHitData = CompressProperty(gs => gs.DaggersHit);
			EnemiesAliveData = CompressProperty(gs => gs.EnemiesAlive);
			HomingStoredData = CompressProperty(gs => gs.HomingDaggers);
			HomingEatenData = CompressProperty(gs => gs.HomingDaggersEaten);
			GemsDespawnedData = CompressProperty(gs => gs.GemsDespawned);
			GemsEatenData = CompressProperty(gs => gs.GemsEaten);
			GemsTotalData = CompressProperty(gs => gs.GemsTotal);

			Skull1sAliveData = CompressProperty(gs => gs.Skull1sAlive);
			Skull2sAliveData = CompressProperty(gs => gs.Skull2sAlive);
			Skull3sAliveData = CompressProperty(gs => gs.Skull3sAlive);
			SpiderlingsAliveData = CompressProperty(gs => gs.SpiderlingsAlive);
			Skull4sAliveData = CompressProperty(gs => gs.Skull4sAlive);
			Squid1sAliveData = CompressProperty(gs => gs.Squid1sAlive);
			Squid2sAliveData = CompressProperty(gs => gs.Squid2sAlive);
			Squid3sAliveData = CompressProperty(gs => gs.Squid3sAlive);
			CentipedesAliveData = CompressProperty(gs => gs.CentipedesAlive);
			GigapedesAliveData = CompressProperty(gs => gs.GigapedesAlive);
			Spider1sAliveData = CompressProperty(gs => gs.Spider1sAlive);
			Spider2sAliveData = CompressProperty(gs => gs.Spider2sAlive);
			LeviathansAliveData = CompressProperty(gs => gs.LeviathansAlive);
			OrbsAliveData = CompressProperty(gs => gs.OrbsAlive);
			ThornsAliveData = CompressProperty(gs => gs.ThornsAlive);
			GhostpedesAliveData = CompressProperty(gs => gs.GhostpedesAlive);
			SpiderEggsAliveData = CompressProperty(gs => gs.SpiderEggsAlive);

			Skull1sKilledData = CompressProperty(gs => gs.Skull1sKilled);
			Skull2sKilledData = CompressProperty(gs => gs.Skull2sKilled);
			Skull3sKilledData = CompressProperty(gs => gs.Skull3sKilled);
			SpiderlingsKilledData = CompressProperty(gs => gs.SpiderlingsKilled);
			Skull4sKilledData = CompressProperty(gs => gs.Skull4sKilled);
			Squid1sKilledData = CompressProperty(gs => gs.Squid1sKilled);
			Squid2sKilledData = CompressProperty(gs => gs.Squid2sKilled);
			Squid3sKilledData = CompressProperty(gs => gs.Squid3sKilled);
			CentipedesKilledData = CompressProperty(gs => gs.CentipedesKilled);
			GigapedesKilledData = CompressProperty(gs => gs.GigapedesKilled);
			Spider1sKilledData = CompressProperty(gs => gs.Spider1sKilled);
			Spider2sKilledData = CompressProperty(gs => gs.Spider2sKilled);
			LeviathansKilledData = CompressProperty(gs => gs.LeviathansKilled);
			OrbsKilledData = CompressProperty(gs => gs.OrbsKilled);
			ThornsKilledData = CompressProperty(gs => gs.ThornsKilled);
			GhostpedesKilledData = CompressProperty(gs => gs.GhostpedesKilled);
			SpiderEggsKilledData = CompressProperty(gs => gs.SpiderEggsKilled);

			byte[] CompressProperty(Func<GameState, int> propertySelector)
				=> IntegerArrayCompressor.CompressData(gameStates.Select(propertySelector).ToArray());
		}
	}
}
