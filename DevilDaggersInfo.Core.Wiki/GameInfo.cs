namespace DevilDaggersInfo.Core.Wiki
{
	public static class GameInfo
	{
		private static readonly GameVersions[] _gameVersions = (GameVersions[])Enum.GetValues(typeof(GameVersions));

		private static readonly Dictionary<Enemy, string[]> _enemyDescriptions = new()
		{
			{ EnemiesV1_0.Squid1, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV1_0.Squid2, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV1_0.Centipede, new[] { "Emerges approximately 3 seconds after its spawn, starts flying idly for a while, then starts chasing the player", "Regularly dives down and moves underground for a while" } },
			{ EnemiesV1_0.Gigapede, new[] { "Emerges approximately 3 seconds after its spawn, then starts flying around the arena", "Regularly dives down and moves underground for a while" } },
			{ EnemiesV1_0.Leviathan, new[] { "Activates 8.5333 seconds after its initial appearance", "Attracts and transmutes all skulls by beckoning every 20 seconds, starting 13.5333 seconds after its spawn (5 seconds after becoming active)", "Rotates counter-clockwise" } },
			{ EnemiesV1_0.Spider1, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 3 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head" } },
			{ EnemiesV1_0.Skull1, new[] { "Slowly chases the player" } },
			{ EnemiesV1_0.Skull2, new[] { "Moves randomly" } },
			{ EnemiesV1_0.Skull3, new[] { "Chases the player fast" } },
			{ EnemiesV1_0.TransmutedSkull2, new[] { "Moves randomly" } },
			{ EnemiesV1_0.TransmutedSkull3, new[] { "Chases the player fast" } },
			{ EnemiesV1_0.TransmutedSkull4, new[] { "Chases the player fast" } },
			{ EnemiesV1_0.SpiderEgg1, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ EnemiesV1_0.Spiderling, new[] { "Darts towards the player in bursts with random offsets" } },
			{ EnemiesV2_0.Squid1, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV2_0.Squid2, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV2_0.Squid3, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 15 Skull Is and 1 Skull IV every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV2_0.Centipede, new[] { "Emerges approximately 3 seconds after its spawn, starts flying idly for a while, then starts chasing the player", "Regularly dives down and moves underground for a while" } },
			{ EnemiesV2_0.Gigapede, new[] { "Emerges approximately 3 seconds after its spawn, then starts chasing the player immediately" } },
			{ EnemiesV2_0.Leviathan, new[] { "Activates 8.5333 seconds after its initial appearance", "Attracts and transmutes all skulls by beckoning every 20 seconds, starting 13.5333 seconds after its spawn (5 seconds after becoming active)", "Rotates counter-clockwise" } },
			{ EnemiesV2_0.Spider1, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 3 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head" } },
			{ EnemiesV2_0.Spider2, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 9 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg II one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head (though barely noticeable due to its size)" } },
			{ EnemiesV2_0.Skull1, new[] { "Slowly chases the player" } },
			{ EnemiesV2_0.Skull2, new[] { "Moves randomly" } },
			{ EnemiesV2_0.Skull3, new[] { "Chases the player fast" } },
			{ EnemiesV2_0.Skull4, new[] { "Chases the player fast" } },
			{ EnemiesV2_0.TransmutedSkull1, new[] { "Slowly chases the player" } },
			{ EnemiesV2_0.TransmutedSkull2, new[] { "Moves randomly" } },
			{ EnemiesV2_0.TransmutedSkull3, new[] { "Chases the player fast" } },
			{ EnemiesV2_0.TransmutedSkull4, new[] { "Chases the player fast" } },
			{ EnemiesV2_0.SpiderEgg1, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ EnemiesV2_0.SpiderEgg2, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ EnemiesV2_0.Spiderling, new[] { "Darts towards the player in bursts with random offsets" } },
			{ EnemiesV2_0.Andras, new[] { "Unfinished enemy that was never added to the real game", "Only appears in V2, can only be spawned using an edited spawnset", "Has its own sounds", "Uses the model for Skull III, but is smaller in size", "Does nothing but attract and consume all homing daggers like Ghostpede ", "Only takes damage when shot from above, so the player will need to daggerjump", "The player doesn't die when touching it" } },
			{ EnemiesV3_0.Squid1, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV3_0.Squid2, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV3_0.Squid3, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 15 Skull Is and 1 Skull IV every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV3_0.Centipede, new[] { "Emerges approximately 3 seconds after its spawn, starts flying idly for a while, then starts chasing the player", "Regularly dives down and moves underground for a while" } },
			{ EnemiesV3_0.Gigapede, new[] { "Emerges approximately 3 seconds after its spawn, then starts chasing the player immediately" } },
			{ EnemiesV3_0.Ghostpede, new[] { "Emerges approximately 3 seconds after its spawn, then starts flying in circles high above the arena", "Attracts and consumes all homing daggers, making them useless" } },
			{ EnemiesV3_0.Leviathan, new[] { "Activates 8.5333 seconds after its initial appearance", "Attracts and transmutes all skulls by beckoning every 20 seconds, starting 13.5333 seconds after its spawn (5 seconds after becoming active)", "Rotates counter-clockwise", "Drops The Orb 3.3167 seconds after dying" } },
			{ EnemiesV3_0.Thorn, new[] { "Emerges approximately 3 seconds after its spawn", "Takes up space" } },
			{ EnemiesV3_0.Spider1, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 3 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head" } },
			{ EnemiesV3_0.Spider2, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 9 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg II one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head (though barely noticeable due to its size)" } },
			{ EnemiesV3_0.Skull1, new[] { "Slowly chases the player" } },
			{ EnemiesV3_0.Skull2, new[] { "Moves randomly" } },
			{ EnemiesV3_0.Skull3, new[] { "Chases the player fast" } },
			{ EnemiesV3_0.Skull4, new[] { "Chases the player fast" } },
			{ EnemiesV3_0.TransmutedSkull1, new[] { "Slowly chases the player" } },
			{ EnemiesV3_0.TransmutedSkull2, new[] { "Moves randomly" } },
			{ EnemiesV3_0.TransmutedSkull3, new[] { "Chases the player fast" } },
			{ EnemiesV3_0.TransmutedSkull4, new[] { "Chases the player fast" } },
			{ EnemiesV3_0.SpiderEgg1, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ EnemiesV3_0.SpiderEgg2, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ EnemiesV3_0.Spiderling, new[] { "Darts towards the player in bursts with random offsets" } },
			{ EnemiesV3_0.TheOrb, new[] { "Activates 10 seconds after Leviathan's death", "Behaves like an eyeball, will look at the player, then attract and transmute all skulls by beckoning every 2.5333 seconds", "Becomes stunned under constant fire, cannot look or attract skulls while stunned" } },
			{ EnemiesV3_1.Squid1, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV3_1.Squid2, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV3_1.Squid3, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 15 Skull Is and 1 Skull IV every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ EnemiesV3_1.Centipede, new[] { "Emerges approximately 3 seconds after its spawn, starts flying idly for a while, then starts chasing the player", "Regularly dives down and moves underground for a while" } },
			{ EnemiesV3_1.Gigapede, new[] { "Emerges approximately 3 seconds after its spawn, then starts chasing the player immediately" } },
			{ EnemiesV3_1.Ghostpede, new[] { "Emerges approximately 3 seconds after its spawn, then starts flying in circles high above the arena", "Attracts and consumes all homing daggers, making them useless" } },
			{ EnemiesV3_1.Leviathan, new[] { "Activates 8.5333 seconds after its initial appearance", "Attracts and transmutes all skulls by beckoning every 20 seconds, starting 13.5333 seconds after its spawn (5 seconds after becoming active)", "Rotates counter-clockwise", "Drops The Orb 3.3167 seconds after dying" } },
			{ EnemiesV3_1.Thorn, new[] { "Emerges approximately 3 seconds after its spawn", "Takes up space" } },
			{ EnemiesV3_1.Spider1, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 3 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head" } },
			{ EnemiesV3_1.Spider2, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 9 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg II one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head (though barely noticeable due to its size)" } },
			{ EnemiesV3_1.Skull1, new[] { "Slowly chases the player" } },
			{ EnemiesV3_1.Skull2, new[] { "Moves randomly" } },
			{ EnemiesV3_1.Skull3, new[] { "Chases the player fast" } },
			{ EnemiesV3_1.Skull4, new[] { "Chases the player fast" } },
			{ EnemiesV3_1.TransmutedSkull1, new[] { "Slowly chases the player" } },
			{ EnemiesV3_1.TransmutedSkull2, new[] { "Moves randomly" } },
			{ EnemiesV3_1.TransmutedSkull3, new[] { "Chases the player fast" } },
			{ EnemiesV3_1.TransmutedSkull4, new[] { "Chases the player fast" } },
			{ EnemiesV3_1.SpiderEgg1, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ EnemiesV3_1.SpiderEgg2, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ EnemiesV3_1.Spiderling, new[] { "Darts towards the player in bursts with random offsets" } },
			{ EnemiesV3_1.TheOrb, new[] { "Activates 10 seconds after Leviathan's death", "Behaves like an eyeball, will look at the player, then attract and transmute all skulls by beckoning every 2.5333 seconds", "Becomes stunned under constant fire, cannot look or attract skulls while stunned" } },
		};

		public static GameVersions? GetGameVersionFromDate(DateTime dateTime)
		{
			for (int i = 0; i < _gameVersions.Length; i++)
			{
				if (dateTime >= GetReleaseDate(_gameVersions[i]) && (i == _gameVersions.Length - 1 || dateTime < GetReleaseDate(_gameVersions[i + 1])))
					return _gameVersions[i];
			}

			return null;
		}

		public static DateTime? GetReleaseDate(GameVersions gameVersion) => gameVersion switch
		{
			GameVersions.V1_0 => new(2016, 2, 18),
			GameVersions.V2_0 => new(2016, 7, 5),
			GameVersions.V3_0 => new(2016, 9, 19),
			GameVersions.V3_1 => new(2021, 2, 19),
			_ => null,
		};

		public static string[] GetEnemyInfo(Enemy enemy)
		{
			foreach (KeyValuePair<Enemy, string[]> kvp in _enemyDescriptions)
			{
				if (kvp.Key == enemy)
					return kvp.Value;
			}

			throw new($"Could not find enemy info for {nameof(Enemy)} with name '{enemy.Name}' and version '{enemy.GameVersions}'.");
		}

		public static Dagger GetDaggerFromTenthsOfMilliseconds(GameVersion gameVersion, int timeInTenthsOfMilliseconds)
			=> GetDaggerFromSeconds(gameVersion, timeInTenthsOfMilliseconds / 10000.0);

		public static Dagger GetDaggerFromSeconds(GameVersion gameVersion, double timeInSeconds)
		{
			List<Dagger> daggers = Daggers.GetDaggers(gameVersion);
			for (int i = daggers.Count - 1; i >= 0; i--)
			{
				if (timeInSeconds >= daggers[i].UnlockSecond)
					return daggers[i];
			}

			throw new ArgumentOutOfRangeException(nameof(timeInSeconds), $"Could not determine dagger based on time '{timeInSeconds:0.0000}'.");
		}
	}
}
