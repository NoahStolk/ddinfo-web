using DevilDaggersInfo.Core.Encryption;
using DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Commands.CustomEntries;
using System.Web;

namespace DevilDaggersInfo.Web.Server.IntegrationTest.Data;

/// <summary>
/// Builds the submission payload ddinfo-tools posts to <c>api/app/custom-entries/submit</c>, including the encrypted
/// validation signature the server checks before accepting a run.
/// </summary>
internal static class UploadRequestFixtures
{
	/// <summary>
	/// Must match the CustomLeaderboards options the test host is configured with, or the server cannot decrypt the
	/// validation and rejects the submission.
	/// </summary>
	internal const string Secret = "0123456789abcdef";

	internal static byte[] MockReplay(byte[] spawnsetFileContents, string playerName = "user")
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);
		bw.Write("ddrpl."u8);
		bw.Seek(44, SeekOrigin.Current);
		bw.Write(playerName.Length);
		foreach (char c in playerName)
			bw.Write((byte)c);

		bw.Seek(10, SeekOrigin.Current);
		bw.Write(MD5.HashData(spawnsetFileContents));
		bw.Write(spawnsetFileContents.Length);
		bw.Write(spawnsetFileContents);

		return ms.ToArray();
	}

	internal static AddUploadRequest Create(
		byte[] survivalHashMd5,
		byte[] replayData,
		float timeInSeconds,
		int playerId,
		int status = 4,
		string? clientVersion = null,
		string? validation = null)
	{
		byte[] timeAsBytes = BitConverter.GetBytes(timeInSeconds);
		byte[] zeroFloat = BitConverter.GetBytes(0f);

		const int gemsCollected = 0;
		const int gemsDespawned = 0;
		const int gemsEaten = 0;
		const int gemsTotal = 0;
		const int enemiesAlive = 0;
		const int enemiesKilled = 0;
		const byte deathType = 0;
		const int daggersHit = 0;
		const int daggersFired = 0;
		const int homingStored = 0;
		const int homingEaten = 0;
		const bool isReplay = false;
		const byte gameMode = 0;
		const bool timeAttackOrRaceFinished = false;
		const bool prohibitedMods = false;

		string calculatedValidation = UploadRequest.CreateValidationV2(
			playerId: playerId,
			timeAsBytes: timeAsBytes,
			gemsCollected: gemsCollected,
			gemsDespawned: gemsDespawned,
			gemsEaten: gemsEaten,
			gemsTotal: gemsTotal,
			enemiesAlive: enemiesAlive,
			enemiesKilled: enemiesKilled,
			deathType: deathType,
			daggersHit: daggersHit,
			daggersFired: daggersFired,
			homingStored: homingStored,
			homingEaten: homingEaten,
			isReplay: isReplay,
			status: status,
			survivalHashMd5: survivalHashMd5,
			levelUpTime2AsBytes: zeroFloat,
			levelUpTime3AsBytes: zeroFloat,
			levelUpTime4AsBytes: zeroFloat,
			gameMode: gameMode,
			timeAttackOrRaceFinished: timeAttackOrRaceFinished,
			prohibitedMods: prohibitedMods);

		AesBase32Wrapper encryptionWrapper = new(Secret, Secret, Secret);

		return new AddUploadRequest
		{
			SurvivalHashMd5 = survivalHashMd5,
			PlayerId = playerId,
			PlayerName = $"TestPlayer{playerId}",
			ReplayPlayerId = 0,
			TimeInSeconds = timeInSeconds,
			TimeAsBytes = timeAsBytes,
			GemsCollected = gemsCollected,
			EnemiesKilled = enemiesKilled,
			DaggersFired = daggersFired,
			DaggersHit = daggersHit,
			EnemiesAlive = enemiesAlive,
			HomingStored = homingStored,
			HomingEaten = homingEaten,
			GemsDespawned = gemsDespawned,
			GemsEaten = gemsEaten,
			GemsTotal = gemsTotal,
			DeathType = deathType,
			LevelUpTime2InSeconds = 0,
			LevelUpTime3InSeconds = 0,
			LevelUpTime4InSeconds = 0,
			LevelUpTime2AsBytes = zeroFloat,
			LevelUpTime3AsBytes = zeroFloat,
			LevelUpTime4AsBytes = zeroFloat,
			ClientVersion = clientVersion ?? TestConstants.DdclVersion,
			Client = "ddinfo-tools",
			OperatingSystem = "Windows",
			BuildMode = "Release",
			Validation = validation ?? HttpUtility.HtmlEncode(encryptionWrapper.EncryptAndEncode(calculatedValidation)),
			ValidationVersion = 2,
			IsReplay = isReplay,
			ProhibitedMods = prohibitedMods,
			GameMode = gameMode,
			TimeAttackOrRaceFinished = timeAttackOrRaceFinished,
			GameData = GameDataFixtures.Empty(),
			ReplayData = replayData,
			Status = status,
			Timestamps =
			[
				new AddUploadRequestTimestamp { Timestamp = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks, TimeInSeconds = 0 },
				new AddUploadRequestTimestamp { Timestamp = new DateTime(2024, 1, 1, 0, 1, 0, DateTimeKind.Utc).Ticks, TimeInSeconds = 60 },
			],
		};
	}
}
