using DevilDaggersWebsite.Models.Game;

namespace DevilDaggersWebsite.Utils
{
	public static class GameHelper
	{
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

		public static Enemy Squid1 = new Enemy("Squid I", "4E3000", 10, 1, 1, Purged, 1, 1);
		public static Enemy Squid2 = new Enemy("Squid II", "804E00", 20, 2, 2, Desecrated, 2, 1);
		public static Enemy Squid3 = new Enemy("Squid III", "AF6B00", 90, 3, 3, Sacrificed, 3, 9);
		public static Enemy Centipede = new Enemy("Centipede", "837E75", 75, 25, 25, Eviscerated, 25, 25);
		public static Enemy Gigapede = new Enemy("Gigapede", "478B41", 250, 50, 50, Annihilated, 50, 50);
		public static Enemy Ghostpede = new Enemy("Ghostpede", "FFFFFF", 500, 10, 10, Intoxicated, null, null);
		public static Enemy Leviathan = new Enemy("Leviathan", "FF0000", 1500, 6, 6, Incarnated, 1500, 1500);
		public static Enemy Thorn = new Enemy("Thorn", "771D00", 120, 0, 1, Barbed, 12, 12);
		public static Enemy Spider1 = new Enemy("Spider I", "097A00", 25, 1, 1, Intoxicated, 3, 3);
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

		public static Enemy SpiderEgg1 = new Enemy("Spider Egg I", "99A100", 3, 0, 1, Intoxicated, 3, 3, Spider1);
		public static Enemy SpiderEgg2 = new Enemy("Spider Egg II", "657A00", 3, 0, 1, Envenmonated, 3, 3, Spider2);
		public static Enemy Spiderling = new Enemy("Spiderling", "DCCB00", 3, 0, 1, Infested, 1, 1, SpiderEgg1, SpiderEgg2);

		public static Upgrade Level1 = new Upgrade(1, 20, 10, null, null, "BB5500", "N/A");
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
		public static Death[] Deaths = { Fallen, Swarmed, Impaled, Gored, Infested, Opened, Purged, Desecrated, Sacrificed, Eviscerated, Annihilated, Intoxicated, Envenmonated, Incarnated, Discarnated, Barbed };
	}
}