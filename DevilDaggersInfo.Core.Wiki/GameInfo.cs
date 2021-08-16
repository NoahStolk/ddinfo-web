namespace DevilDaggersInfo.Core.Wiki
{
	public static class GameInfo
	{
		private static readonly GameVersions[] _gameVersions = (GameVersions[])Enum.GetValues(typeof(GameVersions));

		private static readonly IEnumerable<DevilDaggersObject> _entities = typeof(GameInfo).GetFields().Where(f => f.FieldType.IsSubclassOf(typeof(DevilDaggersObject))).Select(f => (DevilDaggersObject)f.GetValue(null)!);

		private static readonly Dictionary<Enemy, string[]> _enemyInfo = new()
		{
			{ V1Squid1, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V1Squid2, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V1Centipede, new[] { "Emerges approximately 3 seconds after its spawn, starts flying idly for a while, then starts chasing the player", "Regularly dives down and moves underground for a while" } },
			{ V1Gigapede, new[] { "Emerges approximately 3 seconds after its spawn, then starts flying around the arena", "Regularly dives down and moves underground for a while" } },
			{ V1Leviathan, new[] { "Activates 8.5333 seconds after its initial appearance", "Attracts and transmutes all skulls by beckoning every 20 seconds, starting 13.5333 seconds after its spawn (5 seconds after becoming active)", "Rotates counter-clockwise" } },
			{ V1Spider1, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 3 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head" } },
			{ V1Skull1, new[] { "Slowly chases the player" } },
			{ V1Skull2, new[] { "Moves randomly" } },
			{ V1Skull3, new[] { "Chases the player fast" } },
			{ V1TransmutedSkull2, new[] { "Moves randomly" } },
			{ V1TransmutedSkull3, new[] { "Chases the player fast" } },
			{ V1TransmutedSkull4, new[] { "Chases the player fast" } },
			{ V1SpiderEgg1, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ V1Spiderling, new[] { "Darts towards the player in bursts with random offsets" } },
			{ V2Squid1, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V2Squid2, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V2Squid3, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 15 Skull Is and 1 Skull IV every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V2Centipede, new[] { "Emerges approximately 3 seconds after its spawn, starts flying idly for a while, then starts chasing the player", "Regularly dives down and moves underground for a while" } },
			{ V2Gigapede, new[] { "Emerges approximately 3 seconds after its spawn, then starts chasing the player immediately" } },
			{ V2Leviathan, new[] { "Activates 8.5333 seconds after its initial appearance", "Attracts and transmutes all skulls by beckoning every 20 seconds, starting 13.5333 seconds after its spawn (5 seconds after becoming active)", "Rotates counter-clockwise" } },
			{ V2Spider1, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 3 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head" } },
			{ V2Spider2, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 9 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg II one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head (though barely noticeable due to its size)" } },
			{ V2Skull1, new[] { "Slowly chases the player" } },
			{ V2Skull2, new[] { "Moves randomly" } },
			{ V2Skull3, new[] { "Chases the player fast" } },
			{ V2Skull4, new[] { "Chases the player fast" } },
			{ V2TransmutedSkull1, new[] { "Slowly chases the player" } },
			{ V2TransmutedSkull2, new[] { "Moves randomly" } },
			{ V2TransmutedSkull3, new[] { "Chases the player fast" } },
			{ V2TransmutedSkull4, new[] { "Chases the player fast" } },
			{ V2SpiderEgg1, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ V2SpiderEgg2, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ V2Spiderling, new[] { "Darts towards the player in bursts with random offsets" } },
			{ V2Andras, new[] { "Unfinished enemy that was never added to the real game", "Only appears in V2, can only be spawned using an edited spawnset", "Has its own sounds", "Uses the model for Skull III, but is smaller in size", "Does nothing but attract and consume all homing daggers like Ghostpede ", "Only takes damage when shot from above, so the player will need to daggerjump", "The player doesn't die when touching it" } },
			{ V3Squid1, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V3Squid2, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V3Squid3, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 15 Skull Is and 1 Skull IV every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V3Centipede, new[] { "Emerges approximately 3 seconds after its spawn, starts flying idly for a while, then starts chasing the player", "Regularly dives down and moves underground for a while" } },
			{ V3Gigapede, new[] { "Emerges approximately 3 seconds after its spawn, then starts chasing the player immediately" } },
			{ V3Ghostpede, new[] { "Emerges approximately 3 seconds after its spawn, then starts flying in circles high above the arena", "Attracts and consumes all homing daggers, making them useless" } },
			{ V3Leviathan, new[] { "Activates 8.5333 seconds after its initial appearance", "Attracts and transmutes all skulls by beckoning every 20 seconds, starting 13.5333 seconds after its spawn (5 seconds after becoming active)", "Rotates counter-clockwise", "Drops The Orb 3.3167 seconds after dying" } },
			{ V3Thorn, new[] { "Emerges approximately 3 seconds after its spawn", "Takes up space" } },
			{ V3Spider1, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 3 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head" } },
			{ V3Spider2, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 9 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg II one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head (though barely noticeable due to its size)" } },
			{ V3Skull1, new[] { "Slowly chases the player" } },
			{ V3Skull2, new[] { "Moves randomly" } },
			{ V3Skull3, new[] { "Chases the player fast" } },
			{ V3Skull4, new[] { "Chases the player fast" } },
			{ V3TransmutedSkull1, new[] { "Slowly chases the player" } },
			{ V3TransmutedSkull2, new[] { "Moves randomly" } },
			{ V3TransmutedSkull3, new[] { "Chases the player fast" } },
			{ V3TransmutedSkull4, new[] { "Chases the player fast" } },
			{ V3SpiderEgg1, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ V3SpiderEgg2, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ V3Spiderling, new[] { "Darts towards the player in bursts with random offsets" } },
			{ V3TheOrb, new[] { "Activates 10 seconds after Leviathan's death", "Behaves like an eyeball, will look at the player, then attract and transmute all skulls by beckoning every 2.5333 seconds", "Becomes stunned under constant fire, cannot look or attract skulls while stunned" } },
			{ V31Squid1, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull II every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V31Squid2, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 10 Skull Is and 1 Skull III every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V31Squid3, new[] { "Spawns at the edge of the arena", "Moves slowly and rotates clockwise", "Spawns 15 Skull Is and 1 Skull IV every 20 seconds (starting 3 seconds after its initial appearance)" } },
			{ V31Centipede, new[] { "Emerges approximately 3 seconds after its spawn, starts flying idly for a while, then starts chasing the player", "Regularly dives down and moves underground for a while" } },
			{ V31Gigapede, new[] { "Emerges approximately 3 seconds after its spawn, then starts chasing the player immediately" } },
			{ V31Ghostpede, new[] { "Emerges approximately 3 seconds after its spawn, then starts flying in circles high above the arena", "Attracts and consumes all homing daggers, making them useless" } },
			{ V31Leviathan, new[] { "Activates 8.5333 seconds after its initial appearance", "Attracts and transmutes all skulls by beckoning every 20 seconds, starting 13.5333 seconds after its spawn (5 seconds after becoming active)", "Rotates counter-clockwise", "Drops The Orb 3.3167 seconds after dying" } },
			{ V31Thorn, new[] { "Emerges approximately 3 seconds after its spawn", "Takes up space" } },
			{ V31Spider1, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 3 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg I one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head" } },
			{ V31Spider2, new[] { "Spawns at the edge of the arena and starts lifting its head, faces the player after 9 seconds", "Attracts and consumes gems when facing the player, ejecting them as Spider Egg II one at a time", "Hides its head when shot and left unharmed for 1 second", "Begins moving randomly in an unpredictable jittery fashion after initially raising its head (though barely noticeable due to its size)" } },
			{ V31Skull1, new[] { "Slowly chases the player" } },
			{ V31Skull2, new[] { "Moves randomly" } },
			{ V31Skull3, new[] { "Chases the player fast" } },
			{ V31Skull4, new[] { "Chases the player fast" } },
			{ V31TransmutedSkull1, new[] { "Slowly chases the player" } },
			{ V31TransmutedSkull2, new[] { "Moves randomly" } },
			{ V31TransmutedSkull3, new[] { "Chases the player fast" } },
			{ V31TransmutedSkull4, new[] { "Chases the player fast" } },
			{ V31SpiderEgg1, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ V31SpiderEgg2, new[] { "Hatches into 5 Spiderlings after 10 seconds" } },
			{ V31Spiderling, new[] { "Darts towards the player in bursts with random offsets" } },
			{ V31TheOrb, new[] { "Activates 10 seconds after Leviathan's death", "Behaves like an eyeball, will look at the player, then attract and transmute all skulls by beckoning every 2.5333 seconds", "Becomes stunned under constant fire, cannot look or attract skulls while stunned" } },
		};

		private static readonly List<Dagger> _v1Daggers = _entities.OfType<Dagger>().Where(e => e.GameVersion == GameVersions.V1).ToList();
		private static readonly List<Death> _v1Deaths = _entities.OfType<Death>().Where(e => e.GameVersion == GameVersions.V1).ToList();
		private static readonly List<Enemy> _v1Enemies = _entities.OfType<Enemy>().Where(e => e.GameVersion == GameVersions.V1).ToList();
		private static readonly List<Upgrade> _v1Upgrades = _entities.OfType<Upgrade>().Where(e => e.GameVersion == GameVersions.V1).ToList();

		private static readonly List<Dagger> _v2Daggers = _entities.OfType<Dagger>().Where(e => e.GameVersion == GameVersions.V2).ToList();
		private static readonly List<Death> _v2Deaths = _entities.OfType<Death>().Where(e => e.GameVersion == GameVersions.V2).ToList();
		private static readonly List<Enemy> _v2Enemies = _entities.OfType<Enemy>().Where(e => e.GameVersion == GameVersions.V2).ToList();
		private static readonly List<Upgrade> _v2Upgrades = _entities.OfType<Upgrade>().Where(e => e.GameVersion == GameVersions.V2).ToList();

		private static readonly List<Dagger> _v3Daggers = _entities.OfType<Dagger>().Where(e => e.GameVersion == GameVersions.V3).ToList();
		private static readonly List<Death> _v3Deaths = _entities.OfType<Death>().Where(e => e.GameVersion == GameVersions.V3).ToList();
		private static readonly List<Enemy> _v3Enemies = _entities.OfType<Enemy>().Where(e => e.GameVersion == GameVersions.V3).ToList();
		private static readonly List<Upgrade> _v3Upgrades = _entities.OfType<Upgrade>().Where(e => e.GameVersion == GameVersions.V3).ToList();

		private static readonly List<Dagger> _v31Daggers = _entities.OfType<Dagger>().Where(e => e.GameVersion == GameVersions.V31).ToList();
		private static readonly List<Death> _v31Deaths = _entities.OfType<Death>().Where(e => e.GameVersion == GameVersions.V31).ToList();
		private static readonly List<Enemy> _v31Enemies = _entities.OfType<Enemy>().Where(e => e.GameVersion == GameVersions.V31).ToList();
		private static readonly List<Upgrade> _v31Upgrades = _entities.OfType<Upgrade>().Where(e => e.GameVersion == GameVersions.V31).ToList();

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
			GameVersions.V1 => new(2016, 2, 18),
			GameVersions.V2 => new(2016, 7, 5),
			GameVersions.V3 => new(2016, 9, 19),
			GameVersions.V31 => new(2021, 2, 19),
			_ => null,
		};

		public static string[] GetEnemyInfo(Enemy enemy)
		{
			foreach (KeyValuePair<Enemy, string[]> kvp in _enemyInfo)
			{
				if (kvp.Key == enemy)
					return kvp.Value;
			}

			throw new($"Could not find enemy info for {nameof(Enemy)} with name '{enemy.Name}' and version '{enemy.GameVersion}'.");
		}

		public static Dagger GetDaggerFromTenthsOfMilliseconds(GameVersions gameVersion, int timeInTenthsOfMilliseconds)
			=> GetDaggerFromSeconds(gameVersion, timeInTenthsOfMilliseconds / 10000.0);

		public static Dagger GetDaggerFromSeconds(GameVersions gameVersion, double timeInSeconds)
		{
			List<Dagger> daggers = GetDaggers(gameVersion);
			for (int i = daggers.Count - 1; i >= 0; i--)
			{
				if (timeInSeconds >= daggers[i].UnlockSecond)
					return daggers[i];
			}

			throw new ArgumentOutOfRangeException(nameof(timeInSeconds), $"Could not determine dagger based on time '{timeInSeconds:0.0000}'.");
		}

		public static IEnumerable<GameVersions> GetAppearances(string entityName)
			=> _entities.Where(e => e.Name == entityName && e.GameVersion != GameVersions.V31).Select(e => e.GameVersion);
	}
}
