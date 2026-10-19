using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Web.Server.Domain.Converters.CoreToDomain;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Data;

/// <summary>
/// The spawnset, leaderboard and entry that the custom entry submission tests are written against. These are seeded as
/// real rows rather than mocked, so the identifiers below are the ones the database will actually assign.
/// </summary>
internal static class CustomLeaderboardFixtures
{
	internal static byte[] V3SpawnsetFile { get; } = File.ReadAllBytes(Path.Combine("Resources", "Spawnsets", "V3"));

	/// <summary>
	/// The V3 spawnset row, with its derived columns computed from the file exactly as SpawnsetService does when a
	/// spawnset is uploaded, so that APIs serving them (in seconds, from game units) see realistic values.
	/// </summary>
	internal static SpawnsetEntity Spawnset(int playerId)
	{
		SpawnsetBinary binary = SpawnsetBinary.Parse(V3SpawnsetFile);
		(SpawnSectionInfo preLoopSection, SpawnSectionInfo loopSection) = binary.CalculateSections();
		EffectivePlayerSettings effectivePlayerSettings = binary.GetEffectivePlayerSettings();

		return new SpawnsetEntity
		{
			Id = 1,
			LastUpdated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			Name = "V3",
			PlayerId = playerId,
			HtmlDescription = string.Empty,
			MaxDisplayWaves = 5,
			File = V3SpawnsetFile,
			Md5Hash = MD5.HashData(V3SpawnsetFile),
			GameMode = binary.GameMode.ToDomain(),
			AdditionalGems = binary.AdditionalGems,
			HandLevel = binary.HandLevel.ToDomain(),
			PreLoopLength = preLoopSection.Length.HasValue ? (int)GameTime.FromSeconds(preLoopSection.Length.Value).GameUnits : null,
			PreLoopSpawnCount = preLoopSection.SpawnCount,
			LoopLength = loopSection.Length.HasValue ? (int)GameTime.FromSeconds(loopSection.Length.Value).GameUnits : null,
			LoopSpawnCount = loopSection.SpawnCount,
			SpawnVersion = binary.SpawnVersion,
			WorldVersion = binary.WorldVersion,
			TimerStart = (int)GameTime.FromSeconds(binary.TimerStart).GameUnits,
			EffectiveHandLevel = effectivePlayerSettings.HandLevel.ToDomain(),
			EffectiveHandMesh = effectivePlayerSettings.HandMesh.ToDomain(),
			EffectiveGemsOrHoming = effectivePlayerSettings.GemsOrHoming,
		};
	}

	internal static CustomLeaderboardEntity CustomLeaderboard(int spawnsetId)
	{
		return new CustomLeaderboardEntity
		{
			Id = 1,
			Bronze = 600000,
			Silver = 1200000,
			Golden = 2500000,
			Devil = 5000000,
			Leviathan = 10000000,
			RankSorting = CustomLeaderboardRankSorting.TimeDesc,
			DateCreated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			DateLastPlayed = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			SpawnsetId = spawnsetId,
			TotalRunsSubmitted = 666,
		};
	}

	internal static CustomEntryEntity CustomEntry(int customLeaderboardId, int playerId, int id = 1, int time = 166666)
	{
		return new CustomEntryEntity
		{
			Id = id,
			ClientVersion = TestConstants.DdclVersion,
			CustomLeaderboardId = customLeaderboardId,
			DaggersFired = 15,
			DaggersHit = 6,
			DeathType = 1,
			EnemiesAlive = 6,
			GemsCollected = 3,
			HomingStored = 0,
			EnemiesKilled = 2,
			PlayerId = playerId,
			Time = time,
			LevelUpTime2 = 0,
			LevelUpTime3 = 0,
			LevelUpTime4 = 0,
			SubmitDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
		};
	}
}
