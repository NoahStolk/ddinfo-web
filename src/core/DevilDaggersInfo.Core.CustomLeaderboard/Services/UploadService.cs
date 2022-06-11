using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.CustomLeaderboard.Memory;
using DevilDaggersInfo.Core.CustomLeaderboard.Models;
using System.Web;

namespace DevilDaggersInfo.Core.CustomLeaderboard.Services;

public class UploadService
{
	private readonly NetworkService _networkService;
	private readonly ReaderService _readerService;
	private readonly IEncryptionService _encryptionService;
	private readonly IClientConfiguration _clientConfiguration;

	public UploadService(NetworkService networkService, ReaderService readerService, IEncryptionService encryptionService, IClientConfiguration clientConfiguration)
	{
		_networkService = networkService;
		_readerService = readerService;
		_encryptionService = encryptionService;
		_clientConfiguration = clientConfiguration;
	}

	public async Task<ResponseWrapper<GetUploadSuccess>> UploadRun(MainBlock block)
	{
		bool leaderboardExists = await _networkService.CheckIfLeaderboardExists(block.SurvivalHashMd5);
		if (!leaderboardExists)
			return new("This leaderboard does not exist.");

		byte[] timeAsBytes = BitConverter.GetBytes(block.Time);
		byte[] levelUpTime2AsBytes = BitConverter.GetBytes(block.LevelUpTime2);
		byte[] levelUpTime3AsBytes = BitConverter.GetBytes(block.LevelUpTime3);
		byte[] levelUpTime4AsBytes = BitConverter.GetBytes(block.LevelUpTime4);

		string toEncrypt = string.Join(
			";",
			block.PlayerId,
			timeAsBytes.ByteArrayToHexString(),
			block.GemsCollected,
			block.GemsDespawned,
			block.GemsEaten,
			block.GemsTotal,
			block.EnemiesAlive,
			block.EnemiesKilled,
			block.DeathType,
			block.DaggersHit,
			block.DaggersFired,
			block.HomingStored,
			block.HomingEaten,
			block.IsReplay,
			block.Status,
			block.SurvivalHashMd5.ByteArrayToHexString(),
			levelUpTime2AsBytes.ByteArrayToHexString(),
			levelUpTime3AsBytes.ByteArrayToHexString(),
			levelUpTime4AsBytes.ByteArrayToHexString(),
			block.GameMode,
			block.TimeAttackOrRaceFinished,
			block.ProhibitedMods);
		string validation = _encryptionService.EncryptAndEncode(toEncrypt);

		AddUploadRequest uploadRequest = new()
		{
			DaggersFired = block.DaggersFired,
			DaggersHit = block.DaggersHit,
			ClientVersion = _clientConfiguration.GetApplicationVersion(),
			DeathType = block.DeathType,
			EnemiesAlive = block.EnemiesAlive,
			GemsCollected = block.GemsCollected,
			GemsDespawned = block.GemsDespawned,
			GemsEaten = block.GemsEaten,
			GemsTotal = block.GemsTotal,
			HomingStored = block.HomingStored,
			HomingEaten = block.HomingEaten,
			EnemiesKilled = block.EnemiesKilled,
			LevelUpTime2InSeconds = block.LevelUpTime2,
			LevelUpTime3InSeconds = block.LevelUpTime3,
			LevelUpTime4InSeconds = block.LevelUpTime4,
			LevelUpTime2AsBytes = levelUpTime2AsBytes,
			LevelUpTime3AsBytes = levelUpTime3AsBytes,
			LevelUpTime4AsBytes = levelUpTime4AsBytes,
			PlayerId = block.PlayerId,
			SurvivalHashMd5 = block.SurvivalHashMd5,
			TimeInSeconds = block.Time,
			TimeAsBytes = timeAsBytes,
			PlayerName = block.PlayerName,
			IsReplay = block.IsReplay,
			Validation = HttpUtility.HtmlEncode(validation),
			ValidationVersion = 2,
			GameData = _readerService.GetGameDataForUpload(),
			BuildMode = _clientConfiguration.GetBuildMode(),
			OperatingSystem = _clientConfiguration.GetOperatingSystem().ToString(),
			ProhibitedMods = block.ProhibitedMods,
			Client = _clientConfiguration.GetApplicationName(),
			ReplayData = _readerService.GetReplayForUpload(),
			Status = block.Status,
			ReplayPlayerId = block.ReplayPlayerId,
			GameMode = block.GameMode,
			TimeAttackOrRaceFinished = block.TimeAttackOrRaceFinished,
		};

		return await _networkService.SubmitScore(uploadRequest);
	}
}
