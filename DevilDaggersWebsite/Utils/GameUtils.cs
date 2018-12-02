using DevilDaggersWebsite.Models.Game;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Utils
{
	public static class GameUtils
	{
		public static Dictionary<int, DateTime> ReleaseDates = new Dictionary<int, DateTime>
		{
			{ 1, new DateTime(2016, 2, 18) },
			{ 2, new DateTime(2016, 7, 5) },
			{ 3, new DateTime(2016, 9, 19) }
		};

		public static Death Unknown = new Death("?", "DDDDDD", -1);

		public static Death Fallen = new Death("FALLEN", "DDDDDD", 0);
		public static Death Swarmed = new Death("SWARMED", "2E1C00", 1);
		public static Death Impaled = new Death("IMPALED", "4E3000", 2);
		public static Death Gored = new Death("GORED", "804E00", 3);
		public static Death Infested = new Death("INFESTED", "DCCB00", 4);
		public static Death Opened = new Death("OPENED", "AF6B00", 5);
		public static Death Purged = new Death("PURGED", "4E3000", 6);
		public static Death Desecrated = new Death("DESECRATED", "804E00", 7);
		public static Death Sacrificed = new Death("SACRIFICED", "AF6B00", 8);
		public static Death Eviscerated = new Death("EVISCERATED", "837E75", 9);
		public static Death Annihilated = new Death("ANNIHILATED", "478B41", 10);
		public static Death Intoxicated = new Death("INTOXICATED", "99A100", 11);
		public static Death Envenmonated = new Death("ENVENMONATED", "657A00", 12);
		public static Death Incarnated = new Death("INCARNATED", "FF0000", 13);
		public static Death Discarnated = new Death("DISCARNATED", "FF3131", 14);
		public static Death Barbed = new Death("BARBED", "771D00", 15);

		//public static Death Macroed = new Death("MACROED", "FF6EDA", 16);

		public static Death Stricken = new Death("STRICKEN", "DCCB00", 16); // Spiderling in V2
		public static Death Devastated = new Death("DEVASTATED", "FF0000", 17); // Leviathan in V2
		public static Death Dismembered = new Death("DISMEMBERED", "000FFF", 18); // ??? in V1

		public static Enemy Squid1 = new Enemy("Squid I", "4E3000", 10, 1, 1, Purged, 1, 1);
		public static Enemy Squid2 = new Enemy("Squid II", "804E00", 20, 2, 2, Desecrated, 2, 1);
		public static Enemy Squid3 = new Enemy("Squid III", "AF6B00", 90, 3, 3, Sacrificed, 3, 9);
		public static Enemy Centipede = new Enemy("Centipede", "837E75", 75, 25, 25, Eviscerated, 25, 25);
		public static Enemy Gigapede = new Enemy("Gigapede", "478B41", 250, 50, 50, Annihilated, 50, 50);
		public static Enemy Ghostpede = new Enemy("Ghostpede", "FFFFFF", 500, 10, 10, Intoxicated, null, null);
		public static Enemy Leviathan = new Enemy("Leviathan", "FF0000", 1500, 6, 6, Incarnated, 1500, 1500);
		public static Enemy Thorn = new Enemy("Thorn", "771D00", 120, 0, 1, Barbed, 12, 12);
		public static Enemy Spider1 = new Enemy("Spider I", "097A00", 25, 1, 1, Intoxicated /*Infested in V2*/, 3, 3);
		public static Enemy Spider2 = new Enemy("Spider II", "13FF00", 200, 1, 1, Envenmonated, 20, 20);

		public static Enemy TheOrb = new Enemy("The Orb", "FF3131", 2400, 0, 1, Discarnated, 2400, 2400, Leviathan);

		public static Enemy Skull1 = new Enemy("Skull I", "2E1C00", 1, 0, 1, Swarmed, 0.25f, 0.25f, Squid1, Squid2, Squid3);
		public static Enemy Skull2 = new Enemy("Skull II", "4E3000", 5, 1, 1, Impaled, 1, 1, Squid1);
		public static Enemy Skull3 = new Enemy("Skull III", "804E00", 10, 1, 1, Gored, 1, 1, Squid2);
		public static Enemy Skull4 = new Enemy("Skull IV", "AF6B00", 100, 0, 1, Opened, 10, 10, Squid3);

		public static Enemy TransmutedSkull1 = new Enemy("Transmuted Skull I", "7F0000", 10, 0, 1, Swarmed, 0.25f, 10, Leviathan, TheOrb);
		public static Enemy TransmutedSkull2 = new Enemy("Transmuted Skull II", "9B0000", 20, 1, 1, Impaled, 2, 2, Leviathan, TheOrb);
		public static Enemy TransmutedSkull3 = new Enemy("Transmuted Skull III", "B80000", 100, 1, 1, Gored, 10, 10, Leviathan, TheOrb);
		public static Enemy TransmutedSkull4 = new Enemy("Transmuted Skull IV", "F00000", 300, 0, 1, Opened, 30, 30, Leviathan, TheOrb);

		public static Enemy SpiderEgg1 = new Enemy("Spider Egg I", "99A100", 3, 0, 1, Intoxicated /*Infested in V2*/, 3, 3, Spider1);
		public static Enemy SpiderEgg2 = new Enemy("Spider Egg II", "657A00", 3, 0, 1, Envenmonated, 3, 3, Spider2);
		public static Enemy Spiderling = new Enemy("Spiderling", "DCCB00", 3, 0, 1, Infested, 1, 1, SpiderEgg1, SpiderEgg2);

		public static Upgrade Level1 = new Upgrade(1, 20, 10, null, null, "BB5500", RazorUtils.NAString);
		public static Upgrade Level2 = new Upgrade(2, 40, 20, null, null, "FFAA00", "10 gems");
		public static Upgrade Level3 = new Upgrade(3, 80, 40, 40, 20, "00FFFF", "70 gems");
		public static Upgrade Level4 = new Upgrade(4, 106f + 2f / 3f, 60, 40, 30, "FF0099", "150 stored homing daggers");

		public static Dagger Default = new Dagger("Default", "444444", null);
		public static Dagger Bronze = new Dagger("Bronze", "CD7F32", 60);
		public static Dagger Silver = new Dagger("Silver", "DDDDDD", 120);
		public static Dagger Golden = new Dagger("Golden", "FFDF00", 250);
		public static Dagger Devil = new Dagger("Devil", "FF0000", 500);

		public static Enemy[] Enemies = { Squid1, Squid2, Squid3, Centipede, Gigapede, Ghostpede, Leviathan, Thorn, Spider1, Spider2, Skull1, Skull2, Skull3, Skull4, TransmutedSkull1, TransmutedSkull2, TransmutedSkull3, TransmutedSkull4, SpiderEgg1, SpiderEgg2, Spiderling, TheOrb };
		public static Upgrade[] Upgrades = { Level1, Level2, Level3, Level4 };
		public static Dagger[] Daggers = { Default, Bronze, Silver, Golden, Devil };
		public static Death[] Deaths = { Fallen, Swarmed, Impaled, Gored, Infested, Opened, Purged, Desecrated, Sacrificed, Eviscerated, Annihilated, Intoxicated, Envenmonated, Incarnated, Discarnated, Barbed, Stricken, Devastated, Dismembered };

		public static Dictionary<Enemy, string> EnemyInfo { get; set; } = new Dictionary<Enemy, string>
		{
			{ Squid1, "Spawns at the edge of the arena\nMoves slowly and rotates clockwise\nSpawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" },
			{ Squid2, "Spawns at the edge of the arena\nMoves slowly and rotates clockwise\nSpawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" },
			{ Squid3, "Spawns at the edge of the arena\nMoves slowly and rotates clockwise\nSpawns 15 Skull Is and 1 Skull IV every 20 seconds (starting 3 seconds after its initial appearance)" },
			{ Centipede, "Emerges and flies idly for a while, then starts chasing the player\nRegularly dives down and moves underground for a while" },
			{ Gigapede, "Emerges and starts chasing the player immediately" },
			{ Ghostpede, "Emerges and starts flying in circles high above the arena\nAttracts and consumes all homing daggers, making them useless" },
			{ Leviathan, "Attracts and transmutes all skulls every 20 seconds\nRotates counter-clockwise\nDrops The Orb after dying" },
			{ Thorn, "Takes up space" },
			{ Spider1, "Spawns at the edge of the arena and starts lifting its head, then faces the player\nAttracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time\nHides its head when shot and left nharmed for 1 second\nBegins moving randomly in an unpredictable jittery fashion after initially raising its head" },
			{ Spider2, "Spawns at the edge of the arena and starts lifting its head (though much slower than Spider I), then faces the player\nAttracts and consumes gems when facing the player, ejecting them as Spider Egg II one at a timenHides its head when shot and left unharmed for 1 second\nBegins moving randomly in an unpredictable jittery fashion after initially raising its head (though barely noticeable due to its size)" },
			{ Skull1, "Slowly chases the player" },
			{ Skull2, "Moves randomly" },
			{ Skull3, "Chases the player fast" },
			{ Skull4, "Chases the player fast" },
			{ TransmutedSkull1, "Slowly chases the player" },
			{ TransmutedSkull2, "Moves randomly" },
			{ TransmutedSkull3, "Chases the player fast" },
			{ TransmutedSkull4, "Chases the player fast" },
			{ SpiderEgg1, "Hatches into 5 Spiderlings after 10 seconds" },
			{ SpiderEgg2, "Hatches into 5 Spiderlings after 10 seconds" },
			{ Spiderling, "Darts towards the player in bursts with random offsets" },
			{ TheOrb, "Behaves like an eyeball, will look at the player, then attract and transmute all skulls every 2.5 seconds\nBecomes stunned under constant fire, cannot look or attract skulls while stunned" }
		};

		public static int GetDeathTypeFromDeathName(string deathName)
		{
			foreach (Death death in Deaths)
				if (death.Name == deathName)
					return death.Type;
			throw new Exception($"Could not parse death type \"{deathName}\" to a valid death type.");
		}

		public static Dagger GetDaggerFromTime(int time)
		{
			for (int i = Daggers.Length - 1; i >= 0; i--)
				if (time >= Daggers[i].UnlockSecond * 10000)
					return Daggers[i];
			return Default;
		}
	}
}