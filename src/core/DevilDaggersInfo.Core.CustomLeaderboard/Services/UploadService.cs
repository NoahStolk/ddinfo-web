using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
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

	public async Task<SubmissionResponseWrapper> UploadRun()
	{
		byte[] timeAsBytes = BitConverter.GetBytes(_readerService.MainBlock.Time);
		byte[] levelUpTime2AsBytes = BitConverter.GetBytes(_readerService.MainBlock.LevelUpTime2);
		byte[] levelUpTime3AsBytes = BitConverter.GetBytes(_readerService.MainBlock.LevelUpTime3);
		byte[] levelUpTime4AsBytes = BitConverter.GetBytes(_readerService.MainBlock.LevelUpTime4);

		string toEncrypt = string.Join(
			";",
			_readerService.MainBlock.PlayerId,
			timeAsBytes.ByteArrayToHexString(),
			_readerService.MainBlock.GemsCollected,
			_readerService.MainBlock.GemsDespawned,
			_readerService.MainBlock.GemsEaten,
			_readerService.MainBlock.GemsTotal,
			_readerService.MainBlock.EnemiesAlive,
			_readerService.MainBlock.EnemiesKilled,
			_readerService.MainBlock.DeathType,
			_readerService.MainBlock.DaggersHit,
			_readerService.MainBlock.DaggersFired,
			_readerService.MainBlock.HomingStored,
			_readerService.MainBlock.HomingEaten,
			_readerService.MainBlock.IsReplay,
			_readerService.MainBlock.Status,
			_readerService.MainBlock.SurvivalHashMd5.ByteArrayToHexString(),
			levelUpTime2AsBytes.ByteArrayToHexString(),
			levelUpTime3AsBytes.ByteArrayToHexString(),
			levelUpTime4AsBytes.ByteArrayToHexString(),
			_readerService.MainBlock.GameMode,
			_readerService.MainBlock.TimeAttackOrRaceFinished,
			_readerService.MainBlock.ProhibitedMods);
		string validation = _encryptionService.EncryptAndEncode(toEncrypt);

		AddUploadRequest uploadRequest = new()
		{
			DaggersFired = _readerService.MainBlock.DaggersFired,
			DaggersHit = _readerService.MainBlock.DaggersHit,
			ClientVersion = _clientConfiguration.GetApplicationVersion(),
			DeathType = _readerService.MainBlock.DeathType,
			EnemiesAlive = _readerService.MainBlock.EnemiesAlive,
			GemsCollected = _readerService.MainBlock.GemsCollected,
			GemsDespawned = _readerService.MainBlock.GemsDespawned,
			GemsEaten = _readerService.MainBlock.GemsEaten,
			GemsTotal = _readerService.MainBlock.GemsTotal,
			HomingStored = _readerService.MainBlock.HomingStored,
			HomingEaten = _readerService.MainBlock.HomingEaten,
			EnemiesKilled = _readerService.MainBlock.EnemiesKilled,
			LevelUpTime2InSeconds = _readerService.MainBlock.LevelUpTime2,
			LevelUpTime3InSeconds = _readerService.MainBlock.LevelUpTime3,
			LevelUpTime4InSeconds = _readerService.MainBlock.LevelUpTime4,
			LevelUpTime2AsBytes = levelUpTime2AsBytes,
			LevelUpTime3AsBytes = levelUpTime3AsBytes,
			LevelUpTime4AsBytes = levelUpTime4AsBytes,
			PlayerId = _readerService.MainBlock.PlayerId,
			SurvivalHashMd5 = _readerService.MainBlock.SurvivalHashMd5,
			TimeInSeconds = _readerService.MainBlock.Time,
			TimeAsBytes = timeAsBytes,
			PlayerName = _readerService.MainBlock.PlayerName,
			IsReplay = _readerService.MainBlock.IsReplay,
			Validation = HttpUtility.HtmlEncode(validation),
			ValidationVersion = 2,
			GameData = _readerService.GetGameDataForUpload(),
			BuildMode = _clientConfiguration.GetBuildMode(),
			OperatingSystem = _clientConfiguration.GetOperatingSystem().ToString(),
			ProhibitedMods = _readerService.MainBlock.ProhibitedMods,
			Client = _clientConfiguration.GetApplicationName(),
			ReplayData = _readerService.GetReplayForUpload(),
			Status = _readerService.MainBlock.Status,
			ReplayPlayerId = _readerService.MainBlock.ReplayPlayerId,
			GameMode = _readerService.MainBlock.GameMode,
			TimeAttackOrRaceFinished = _readerService.MainBlock.TimeAttackOrRaceFinished,
		};

		return await _networkService.SubmitScore(uploadRequest);
	}
}
