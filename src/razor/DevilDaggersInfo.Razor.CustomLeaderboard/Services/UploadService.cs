using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Common.Extensions;
using System.Web;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Services;

public class UploadService
{
	private readonly NetworkService _networkService;
	private readonly GameMemoryService _gameMemoryService;
	private readonly IEncryptionService _encryptionService;
	private readonly IClientConfiguration _clientConfiguration;

	public UploadService(NetworkService networkService, GameMemoryService gameMemoryService, IEncryptionService encryptionService, IClientConfiguration clientConfiguration)
	{
		_networkService = networkService;
		_gameMemoryService = gameMemoryService;
		_encryptionService = encryptionService;
		_clientConfiguration = clientConfiguration;
	}

	public async Task<GetUploadSuccess> UploadRun(MainBlock block)
	{
		bool leaderboardExists = await _networkService.CheckIfLeaderboardExists(block.SurvivalHashMd5);
		if (!leaderboardExists)
			throw new("This leaderboard does not exist.");

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

		byte[] statsBuffer = _gameMemoryService.GetStatsBuffer();

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
			GameData = GetGameDataForUpload(statsBuffer),
			BuildMode = _clientConfiguration.GetBuildMode(),
			OperatingSystem = _clientConfiguration.GetOperatingSystem().ToString(),
			ProhibitedMods = block.ProhibitedMods,
			Client = _clientConfiguration.GetApplicationName(),
			ReplayData = _gameMemoryService.ReadReplayFromMemory(),
			Status = block.Status,
			ReplayPlayerId = block.ReplayPlayerId,
			GameMode = block.GameMode,
			TimeAttackOrRaceFinished = block.TimeAttackOrRaceFinished,
		};

		return await _networkService.SubmitScore(uploadRequest);
	}

	private AddGameData GetGameDataForUpload(byte[] statsBuffer)
	{
		AddGameData gameData = new();

		using MemoryStream ms = new(statsBuffer);
		using BinaryReader br = new(ms);
		for (int i = 0; i < _gameMemoryService.MainBlock.StatsCount; i++)
		{
			gameData.GemsCollected.Add(br.ReadInt32());
			gameData.EnemiesKilled.Add(br.ReadInt32());
			gameData.DaggersFired.Add(br.ReadInt32());
			gameData.DaggersHit.Add(br.ReadInt32());
			gameData.EnemiesAlive.Add(br.ReadInt32());
			_ = br.ReadInt32(); // Skip level gems.
			gameData.HomingStored.Add(br.ReadInt32());
			gameData.GemsDespawned.Add(br.ReadInt32());
			gameData.GemsEaten.Add(br.ReadInt32());
			gameData.GemsTotal.Add(br.ReadInt32());
			gameData.HomingEaten.Add(br.ReadInt32());

			gameData.Skull1sAlive.Add(br.ReadUInt16());
			gameData.Skull2sAlive.Add(br.ReadUInt16());
			gameData.Skull3sAlive.Add(br.ReadUInt16());
			gameData.SpiderlingsAlive.Add(br.ReadUInt16());
			gameData.Skull4sAlive.Add(br.ReadUInt16());
			gameData.Squid1sAlive.Add(br.ReadUInt16());
			gameData.Squid2sAlive.Add(br.ReadUInt16());
			gameData.Squid3sAlive.Add(br.ReadUInt16());
			gameData.CentipedesAlive.Add(br.ReadUInt16());
			gameData.GigapedesAlive.Add(br.ReadUInt16());
			gameData.Spider1sAlive.Add(br.ReadUInt16());
			gameData.Spider2sAlive.Add(br.ReadUInt16());
			gameData.LeviathansAlive.Add(br.ReadUInt16());
			gameData.OrbsAlive.Add(br.ReadUInt16());
			gameData.ThornsAlive.Add(br.ReadUInt16());
			gameData.GhostpedesAlive.Add(br.ReadUInt16());
			gameData.SpiderEggsAlive.Add(br.ReadUInt16());

			gameData.Skull1sKilled.Add(br.ReadUInt16());
			gameData.Skull2sKilled.Add(br.ReadUInt16());
			gameData.Skull3sKilled.Add(br.ReadUInt16());
			gameData.SpiderlingsKilled.Add(br.ReadUInt16());
			gameData.Skull4sKilled.Add(br.ReadUInt16());
			gameData.Squid1sKilled.Add(br.ReadUInt16());
			gameData.Squid2sKilled.Add(br.ReadUInt16());
			gameData.Squid3sKilled.Add(br.ReadUInt16());
			gameData.CentipedesKilled.Add(br.ReadUInt16());
			gameData.GigapedesKilled.Add(br.ReadUInt16());
			gameData.Spider1sKilled.Add(br.ReadUInt16());
			gameData.Spider2sKilled.Add(br.ReadUInt16());
			gameData.LeviathansKilled.Add(br.ReadUInt16());
			gameData.OrbsKilled.Add(br.ReadUInt16());
			gameData.ThornsKilled.Add(br.ReadUInt16());
			gameData.GhostpedesKilled.Add(br.ReadUInt16());
			gameData.SpiderEggsKilled.Add(br.ReadUInt16());
		}

		return gameData;
	}
}
