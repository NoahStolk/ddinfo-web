using DevilDaggersCustomLeaderboards.Clients;
using DevilDaggersInfo.Core.CustomLeaderboards.Data;
using System.Web;

namespace DevilDaggersInfo.Core.CustomLeaderboards.Services;

public class UploadService
{
	private readonly NetworkService _networkService;
	private readonly ReaderService _memoryService;
	private readonly IEncryptionService _encryptionService;

	public UploadService(NetworkService networkService, ReaderService memoryService, IEncryptionService encryptionService)
	{
		_networkService = networkService;
		_memoryService = memoryService;
		_encryptionService = encryptionService;
	}

	public async Task<GetUploadSuccess?> UploadRun(ClientInfo clientInfo)
	{
		Console.Clear();
		Cmd.WriteLine("Checking if this spawnset has a leaderboard...");
		Cmd.WriteLine();

		if (!await _networkService.CheckIfLeaderboardExists(_memoryService.MainBlock.SurvivalHashMd5))
			return null;

		Console.Clear();
		Cmd.WriteLine("Uploading...");
		Cmd.WriteLine();

		byte[] timeAsBytes = BitConverter.GetBytes(_memoryService.MainBlock.Time);
		byte[] levelUpTime2AsBytes = BitConverter.GetBytes(_memoryService.MainBlock.LevelUpTime2);
		byte[] levelUpTime3AsBytes = BitConverter.GetBytes(_memoryService.MainBlock.LevelUpTime3);
		byte[] levelUpTime4AsBytes = BitConverter.GetBytes(_memoryService.MainBlock.LevelUpTime4);

		string toEncrypt = string.Join(
			";",
			_memoryService.MainBlock.PlayerId,
			timeAsBytes.ByteArrayToHexString(),
			_memoryService.MainBlock.GemsCollected,
			_memoryService.MainBlock.GemsDespawned,
			_memoryService.MainBlock.GemsEaten,
			_memoryService.MainBlock.GemsTotal,
			_memoryService.MainBlock.EnemiesAlive,
			_memoryService.MainBlock.EnemiesKilled,
			_memoryService.MainBlock.DeathType,
			_memoryService.MainBlock.DaggersHit,
			_memoryService.MainBlock.DaggersFired,
			_memoryService.MainBlock.HomingStored,
			_memoryService.MainBlock.HomingEaten,
			_memoryService.MainBlock.IsReplay,
			_memoryService.MainBlock.Status,
			_memoryService.MainBlock.SurvivalHashMd5.ByteArrayToHexString(),
			levelUpTime2AsBytes.ByteArrayToHexString(),
			levelUpTime3AsBytes.ByteArrayToHexString(),
			levelUpTime4AsBytes.ByteArrayToHexString(),
			_memoryService.MainBlock.GameMode,
			_memoryService.MainBlock.TimeAttackOrRaceFinished,
			_memoryService.MainBlock.ProhibitedMods);
		string validation = _encryptionService.EncryptAndEncode(toEncrypt);

		AddUploadRequest uploadRequest = new()
		{
			DaggersFired = _memoryService.MainBlock.DaggersFired,
			DaggersHit = _memoryService.MainBlock.DaggersHit,
			ClientVersion = clientInfo.ApplicationVersion,
			DeathType = _memoryService.MainBlock.DeathType,
			EnemiesAlive = _memoryService.MainBlock.EnemiesAlive,
			GemsCollected = _memoryService.MainBlock.GemsCollected,
			GemsDespawned = _memoryService.MainBlock.GemsDespawned,
			GemsEaten = _memoryService.MainBlock.GemsEaten,
			GemsTotal = _memoryService.MainBlock.GemsTotal,
			HomingStored = _memoryService.MainBlock.HomingStored,
			HomingEaten = _memoryService.MainBlock.HomingEaten,
			EnemiesKilled = _memoryService.MainBlock.EnemiesKilled,
			LevelUpTime2InSeconds = _memoryService.MainBlock.LevelUpTime2,
			LevelUpTime3InSeconds = _memoryService.MainBlock.LevelUpTime3,
			LevelUpTime4InSeconds = _memoryService.MainBlock.LevelUpTime4,
			LevelUpTime2AsBytes = levelUpTime2AsBytes,
			LevelUpTime3AsBytes = levelUpTime3AsBytes,
			LevelUpTime4AsBytes = levelUpTime4AsBytes,
			PlayerId = _memoryService.MainBlock.PlayerId,
			SurvivalHashMd5 = _memoryService.MainBlock.SurvivalHashMd5,
			TimeInSeconds = _memoryService.MainBlock.Time,
			TimeAsBytes = timeAsBytes,
			PlayerName = _memoryService.MainBlock.PlayerName,
			IsReplay = _memoryService.MainBlock.IsReplay,
			Validation = HttpUtility.HtmlEncode(validation),
			ValidationVersion = 2,
			GameData = _memoryService.GetGameDataForUpload(),
			BuildMode = clientInfo.ApplicationBuildMode,
			OperatingSystem = clientInfo.OperatingSystem,
			ProhibitedMods = _memoryService.MainBlock.ProhibitedMods,
			Client = clientInfo.ApplicationName,
			ReplayData = _memoryService.GetReplayForUpload(),
			Status = _memoryService.MainBlock.Status,
			ReplayPlayerId = _memoryService.MainBlock.ReplayPlayerId,
			GameMode = _memoryService.MainBlock.GameMode,
			TimeAttackOrRaceFinished = _memoryService.MainBlock.TimeAttackOrRaceFinished,
		};

		return await _networkService.SubmitScore(uploadRequest);
	}
}
