namespace DevilDaggersInfo.Web.Server.IntegrationTest.Data;

/// <summary>
/// Minimal valid entities. Every factory fills in only what the schema requires, so a test can state the one field it
/// actually cares about and leave the rest alone.
/// </summary>
internal static class EntityFixtures
{
	internal static PlayerEntity Player(int id, string name)
	{
		return new PlayerEntity
		{
			Id = id,
			PlayerName = name,
			BanType = BanType.NotBanned,
		};
	}

	internal static ModEntity Mod(string name)
	{
		return new ModEntity
		{
			Name = name,
			Url = $"https://example.invalid/{name}",
			ModTypes = ModTypes.Audio,
			LastUpdated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
		};
	}

	internal static SpawnsetEntity Spawnset(string name, int playerId)
	{
		byte[] file = [1, 2, 3, 4];
		return new SpawnsetEntity
		{
			Name = name,
			PlayerId = playerId,
			File = file,
			Md5Hash = MD5.HashData(file),
			GameMode = SpawnsetGameMode.Survival,
			SpawnVersion = 6,
			WorldVersion = 9,
			PreLoopSpawnCount = 1,
			PreLoopLength = 10,
			LoopSpawnCount = 1,
			LoopLength = 10,
			HandLevel = SpawnsetHandLevel.Level1,
			AdditionalGems = 0,
			TimerStart = 0,
			EffectiveHandLevel = SpawnsetHandLevel.Level1,
			EffectiveGemsOrHoming = 0,
			EffectiveHandMesh = SpawnsetHandLevel.Level1,
			LastUpdated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
		};
	}
}
