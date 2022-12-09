using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Api.App.ProcessMemory;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Encryption;
using System.IO.Compression;
using System.Text;
using System.Web;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder;

public static class RecordingLogic
{
	private static readonly AesBase32Wrapper _aesBase32Wrapper;

	static RecordingLogic()
	{
		using MemoryStream msIn = new(File.ReadAllBytes("ddinfo-value").Select((b, i) => i < 4 ? (byte)(b << 4 | b >> 4) : (byte)~b).ToArray());
		using MemoryStream msOut = new();
		using DeflateStream ds = new(msIn, CompressionMode.Decompress);
		ds.CopyTo(msOut);
		using BinaryReader br = new(msOut);
		br.BaseStream.Position = 0;
		string[] values = Enumerable.Range(0, br.ReadByte()).Select(_ => Encoding.UTF8.GetString(br.ReadBytes(br.ReadByte()).Select((b, j) => j % 4 == 0 ? b : (byte)~b).ToArray())).ToArray();
		_aesBase32Wrapper = new(values[0], values[1], values[2]);
	}

	/// <summary>
	/// Scans game memory. If the marker is not known, fires the call to retrieve it, then returns false because memory can't be scanned until the HTTP call has returned successfully.
	/// </summary>
	/// <returns>Whether the marker is known.</returns>
	public static bool Scan()
	{
		if (!StateManager.MarkerState.Marker.HasValue)
		{
			InitializeMarker();
			return false;
		}

		// Always initialize the process so we detach properly when the game exits.
		Root.Game.GameMemoryService.Initialize(StateManager.MarkerState.Marker.Value);
		Root.Game.GameMemoryService.Scan();

		return true;
	}

	public static void Handle()
	{
		if (!Root.Game.GameMemoryService.IsInitialized)
		{
			StateManager.SetRecordingState(RecordingStateType.WaitingForGame);
			return;
		}

		// TODO: Show current leaderboard
		MainBlock mainBlock = Root.Game.GameMemoryService.MainBlock;
		MainBlock mainBlockPrevious = Root.Game.GameMemoryService.MainBlockPrevious;
		if (StateManager.RecordingState.RecordingStateType != RecordingStateType.Recording)
		{
			if (Math.Abs(mainBlock.Time - mainBlockPrevious.Time) < 0.0001f)
			{
				StateManager.SetRecordingState(RecordingStateType.WaitingForRestart);
				return;
			}

			StateManager.SetRecordingState(RecordingStateType.Recording);
		}

		GameStatus status = (GameStatus)mainBlock.Status;
		if (status == GameStatus.LocalReplay)
		{
			StateManager.SetRecordingState(RecordingStateType.WaitingForLocalReplay);
			return;
		}

		if (status == GameStatus.OwnReplayFromLeaderboard)
		{
			StateManager.SetRecordingState(RecordingStateType.WaitingForLeaderboardReplay);
			return;
		}

		bool justDied = !mainBlock.IsPlayerAlive && mainBlockPrevious.IsPlayerAlive;
		bool uploadRun = justDied && (mainBlock.GameMode == 0 || mainBlock.TimeAttackOrRaceFinished);
		if (!uploadRun)
			return;

		StateManager.SetRecordingState(RecordingStateType.WaitingForStats);
		if (!mainBlock.StatsLoaded)
			return;

		StateManager.SetRecordingState(RecordingStateType.WaitingForReplay);
		if (!Root.Game.GameMemoryService.IsReplayValid())
			return;

		UploadRunIfExists(mainBlock.SurvivalHashMd5);
	}

	private static void InitializeMarker()
	{
		AsyncHandler.Run(SetMarker, () => FetchMarker.HandleAsync(Root.Game.SupportedOperatingSystem));

		void SetMarker(GetMarker? getMarker)
		{
			if (getMarker == null)
			{
				// TODO: Show error.
			}
			else
			{
				StateManager.SetMarker(getMarker.Value);
			}
		}
	}

	private static void UploadRunIfExists(byte[] survivalHash)
	{
		AsyncHandler.Run(UploadRun, () => CheckIfLeaderboardExists.HandleAsync(survivalHash));
	}

	private static void UploadRun(bool? leaderboardExists)
	{
		if (!leaderboardExists.HasValue || !leaderboardExists.Value)
			return;

		GameMemoryService memoryService = Root.Game.GameMemoryService;
		MainBlock block = memoryService.MainBlock;

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
		string validation = _aesBase32Wrapper.EncryptAndEncode(toEncrypt);

		byte[] statsBuffer = memoryService.GetStatsBuffer();

		AddUploadRequest uploadRequest = new()
		{
			DaggersFired = block.DaggersFired,
			DaggersHit = block.DaggersHit,
			ClientVersion = Root.Game.AppVersion.ToString(),
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
			GameData = GetGameDataForUpload(block, statsBuffer),
#if DEBUG
			BuildMode = "DEBUG",
#else
			BuildMode = "RELEASE",
#endif
			OperatingSystem = Root.Game.SupportedOperatingSystem.ToString(),
			ProhibitedMods = block.ProhibitedMods,
			Client = "ddinfo-tools",
			ReplayData = memoryService.ReadReplayFromMemory(),
			Status = block.Status,
			ReplayPlayerId = block.ReplayPlayerId,
			GameMode = block.GameMode,
			TimeAttackOrRaceFinished = block.TimeAttackOrRaceFinished,
		};

		AsyncHandler.Run(OnSubmit, () => UploadSubmission.HandleAsync(uploadRequest));
	}

	private static void OnSubmit(bool? successStatusCode) // TODO
	{

	}

	private static AddGameData GetGameDataForUpload(MainBlock block, byte[] statsBuffer)
	{
		AddGameData gameData = new();

		using MemoryStream ms = new(statsBuffer);
		using BinaryReader br = new(ms);
		for (int i = 0; i < block.StatsCount; i++)
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
