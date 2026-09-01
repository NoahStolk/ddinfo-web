using DevilDaggersInfo.Web.Server.Domain.Utils;

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

	/// <summary>
	/// A player with enough settings filled in that <c>HasSettings()</c> is true, which is what makes them visible on
	/// the settings page.
	/// </summary>
	internal static PlayerEntity PlayerWithSettings(int id, string name)
	{
		PlayerEntity player = Player(id, name);
		player.Dpi = 800;
		player.InGameSens = 1.5f;
		player.Fov = 105;
		player.IsRightHanded = true;
		player.VerticalSync = VerticalSync.Off;
		return player;
	}

	internal static UserEntity User(string name, int? playerId = null)
	{
		PasswordValidator.CreatePasswordHash("Integration-test-1", out byte[] hash, out byte[] salt);
		return new UserEntity
		{
			Name = name,
			PasswordHash = hash,
			PasswordSalt = salt,
			DateRegistered = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			PlayerId = playerId,
		};
	}

	internal static DonationEntity Donation(int playerId, int amountInEurCents, bool isRefunded = false)
	{
		return new DonationEntity
		{
			PlayerId = playerId,
			Amount = amountInEurCents,
			ConvertedEuroCentsReceived = amountInEurCents,
			Currency = Currency.Eur,
			DateReceived = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			IsRefunded = isRefunded,
		};
	}

	internal static CustomLeaderboardEntity CustomLeaderboard(int spawnsetId, CustomLeaderboardRankSorting rankSorting = CustomLeaderboardRankSorting.TimeDesc)
	{
		return new CustomLeaderboardEntity
		{
			SpawnsetId = spawnsetId,
			RankSorting = rankSorting,
			Bronze = 600000,
			Silver = 1200000,
			Golden = 2500000,
			Devil = 5000000,
			Leviathan = 10000000,
			DateCreated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			DateLastPlayed = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			TotalRunsSubmitted = 0,
		};
	}

	internal static CustomEntryEntity CustomEntry(int customLeaderboardId, int playerId, int time)
	{
		return new CustomEntryEntity
		{
			CustomLeaderboardId = customLeaderboardId,
			PlayerId = playerId,
			Time = time,
			ClientVersion = TestConstants.DdclVersion,
			SubmitDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
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
