using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Extensions
{
	public static class EntityExtensions
	{
		public static void Populate(this CustomEntryData customEntryData, List<GameState> gameStates)
		{
			// TODO: Add a smart way to compress this data:
			// If all values are 0, use an empty byte array.
			// If all values are under 256, write 0x00 header byte, then write all data as bytes.
			// If all values are under 65536, write 0x01 header byte, then write all data as ushorts.
			// Otherwise, write 0x02 header byte, then write all data as ints (dd doesn't use uints).
			customEntryData.GemsCollectedData = gameStates.Select(gs => gs.GemsCollected).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.EnemiesKilledData = gameStates.Select(gs => gs.EnemiesKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.DaggersFiredData = gameStates.Select(gs => gs.DaggersFired).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.DaggersHitData = gameStates.Select(gs => gs.DaggersHit).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.EnemiesAliveData = gameStates.Select(gs => gs.EnemiesAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.HomingDaggersData = gameStates.Select(gs => gs.HomingDaggers).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.HomingDaggersEatenData = gameStates.Select(gs => gs.HomingDaggersEaten).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.GemsDespawnedData = gameStates.Select(gs => gs.GemsDespawned).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.GemsEatenData = gameStates.Select(gs => gs.GemsEaten).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.GemsTotalData = gameStates.Select(gs => gs.GemsTotal).SelectMany(BitConverter.GetBytes).ToArray();

			customEntryData.Skull1sAliveData = gameStates.Select(gs => gs.Skull1sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Skull2sAliveData = gameStates.Select(gs => gs.Skull2sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Skull3sAliveData = gameStates.Select(gs => gs.Skull3sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.SpiderlingsAliveData = gameStates.Select(gs => gs.SpiderlingsAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Skull4sAliveData = gameStates.Select(gs => gs.Skull4sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Squid1sAliveData = gameStates.Select(gs => gs.Squid1sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Squid2sAliveData = gameStates.Select(gs => gs.Squid2sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Squid3sAliveData = gameStates.Select(gs => gs.Squid3sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.CentipedesAliveData = gameStates.Select(gs => gs.CentipedesAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.GigapedesAliveData = gameStates.Select(gs => gs.GigapedesAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Spider1sAliveData = gameStates.Select(gs => gs.Spider1sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Spider2sAliveData = gameStates.Select(gs => gs.Spider2sAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.LeviathansAliveData = gameStates.Select(gs => gs.LeviathansAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.OrbsAliveData = gameStates.Select(gs => gs.OrbsAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.ThornsAliveData = gameStates.Select(gs => gs.ThornsAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.GhostpedesAliveData = gameStates.Select(gs => gs.GhostpedesAlive).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.SpiderEggsAliveData = gameStates.Select(gs => gs.SpiderEggsAlive).SelectMany(BitConverter.GetBytes).ToArray();

			customEntryData.Skull1sKilledData = gameStates.Select(gs => gs.Skull1sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Skull2sKilledData = gameStates.Select(gs => gs.Skull2sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Skull3sKilledData = gameStates.Select(gs => gs.Skull3sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.SpiderlingsKilledData = gameStates.Select(gs => gs.SpiderlingsKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Skull4sKilledData = gameStates.Select(gs => gs.Skull4sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Squid1sKilledData = gameStates.Select(gs => gs.Squid1sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Squid2sKilledData = gameStates.Select(gs => gs.Squid2sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Squid3sKilledData = gameStates.Select(gs => gs.Squid3sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.CentipedesKilledData = gameStates.Select(gs => gs.CentipedesKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.GigapedesKilledData = gameStates.Select(gs => gs.GigapedesKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Spider1sKilledData = gameStates.Select(gs => gs.Spider1sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.Spider2sKilledData = gameStates.Select(gs => gs.Spider2sKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.LeviathansKilledData = gameStates.Select(gs => gs.LeviathansKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.OrbsKilledData = gameStates.Select(gs => gs.OrbsKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.ThornsKilledData = gameStates.Select(gs => gs.ThornsKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.GhostpedesKilledData = gameStates.Select(gs => gs.GhostpedesKilled).SelectMany(BitConverter.GetBytes).ToArray();
			customEntryData.SpiderEggsKilledData = gameStates.Select(gs => gs.SpiderEggsKilled).SelectMany(BitConverter.GetBytes).ToArray();
		}
	}
}
