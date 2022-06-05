using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.CustomLeaderboards.Data;
using DevilDaggersInfo.Core.CustomLeaderboards.Enums;
using DevilDaggersInfo.Core.CustomLeaderboards.Memory;
using DevilDaggersInfo.Core.CustomLeaderboards.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevilDaggersInfo.Core.CustomLeaderboards.Services;

public class RecorderService
{
	private const int _mainLoopSleepMilliseconds = 50;

	private readonly NetworkService _networkService;
	private readonly ReaderService _readerService;
	private readonly UploadService _uploadService;
	private readonly ILogger<RecorderService> _logger;
	private readonly IOptions<ClientInfo> _clientInfo;

	private bool _isRecording = true;
	private long _marker;
	private int _selectedIndex;
	private int _pageIndex;
	private GetUploadSuccess? _uploadSuccess;
	private MainBlock _finalRecordedMainBlock;

	public RecorderService(NetworkService networkService, ReaderService readerService, UploadService uploadService, ILogger<RecorderService> logger, IOptions<ClientInfo> clientInfo)
	{
		_networkService = networkService;
		_readerService = readerService;
		_uploadService = uploadService;
		_logger = logger;
		_clientInfo = clientInfo;
	}

	public async Task Record()
	{
		_marker = await _networkService.GetMarker();

		Console.Clear();
		while (true)
		{
			await HandleInput();
			await ExecuteMainLoop();
		}
	}

	private async Task ExecuteMainLoop()
	{
		_readerService.FindWindow();
		if (_readerService.Process == null)
		{
			_readerService.IsInitialized = false;
			Cmd.WriteLine("Devil Daggers not found. Make sure the game is running. Retrying in a second...");
			await Task.Delay(TimeSpan.FromSeconds(1));
			Console.Clear();
			return;
		}

		_readerService.Initialize(_marker);
		if (!_readerService.IsInitialized)
		{
			Cmd.WriteLine("Could not find memory block starting address. Retrying in a second...");
			await Task.Delay(TimeSpan.FromSeconds(1));
			Console.Clear();
			return;
		}

		_readerService.Scan();

		if (!_isRecording)
		{
			if (_readerService.MainBlock.Time == _readerService.MainBlockPrevious.Time || _readerService.MainBlock.Status == (int)GameStatus.LocalReplay)
				return;

			Console.Clear();
			_isRecording = true;
			_uploadSuccess = null;
		}

		GuiUtils.WriteRecording(_readerService.Process, _readerService.MainBlock, _readerService.MainBlockPrevious);

		await Task.Delay(TimeSpan.FromMilliseconds(_mainLoopSleepMilliseconds));
		Console.SetCursorPosition(0, 0);

		bool justDied = !_readerService.MainBlock.IsPlayerAlive && _readerService.MainBlockPrevious.IsPlayerAlive;
		bool uploadRun = justDied && (_readerService.MainBlock.GameMode == 0 || _readerService.MainBlock.TimeAttackOrRaceFinished);
		if (!uploadRun)
			return;

		if (!_readerService.MainBlock.StatsLoaded)
		{
			Console.Clear();
			Cmd.WriteLine("Waiting for stats to be loaded...");

			await Task.Delay(TimeSpan.FromSeconds(0.5));
			return;
		}

		if (!_readerService.IsReplayValid())
		{
			Console.Clear();
			Cmd.WriteLine("Waiting for replay to be loaded...");

			await Task.Delay(TimeSpan.FromSeconds(0.5));
			return;
		}

		_isRecording = false;

		Console.Clear();
		Cmd.WriteLine("Validating...");
		Cmd.WriteLine();

		string? errorMessage = ValidateRunLocally();
		if (errorMessage == null)
		{
			GetUploadSuccess? uploadSuccess = await _uploadService.UploadRun(_clientInfo.Value);
			if (uploadSuccess != null)
			{
				_uploadSuccess = uploadSuccess;
				_finalRecordedMainBlock = _readerService.MainBlock;
				_selectedIndex = 0;
				_pageIndex = 0;
				RenderSuccessfulSubmit();
			}
			else
			{
				await Task.Delay(TimeSpan.FromSeconds(0.5));
			}
		}
		else
		{
			Cmd.WriteLine("Validation failed", ColorUtils.Error);
			Cmd.WriteLine(errorMessage);
			_logger.LogWarning("Validation failed - {errorMessage}", errorMessage);

			await Task.Delay(TimeSpan.FromSeconds(0.5));
		}

		Console.SetCursorPosition(0, 0);
		Cmd.WriteLine("Ready to restart", string.Empty);
		Cmd.WriteLine();
	}

	private void RenderSuccessfulSubmit()
	{
		if (_uploadSuccess == null)
			return;

		Console.SetCursorPosition(0, 2);

		_uploadSuccess.WriteLeaderboard(_readerService.MainBlock.PlayerId, _selectedIndex, _pageIndex);

		Cmd.WriteLine();

		if (_uploadSuccess.IsHighscore)
			_uploadSuccess.WriteHighscoreStats(_finalRecordedMainBlock);
		else
			_uploadSuccess.WriteStats(_finalRecordedMainBlock);

		Cmd.WriteLine();
	}

	private string? ValidateRunLocally()
	{
		const float minimalTime = 0.01f;

		if (_readerService.MainBlock.PlayerId <= 0)
		{
			_logger.LogWarning("Invalid player ID: {playerId}", _readerService.MainBlock.PlayerId);
			return "Invalid player ID.";
		}

		if (_readerService.MainBlock.Time < minimalTime)
			return $"Timer is under {minimalTime:0.0000}. Unable to validate.";

		if (_readerService.MainBlock.Status == (int)GameStatus.LocalReplay)
			return "Local replays are not uploaded.";

		return null;
	}

	private async Task HandleInput()
	{
		if (!Console.KeyAvailable || _uploadSuccess == null)
			return;

		List<int> customEntryIds = _uploadSuccess.Entries.ConvertAll(e => e.Id);
		switch (Console.ReadKey(true).Key)
		{
			case ConsoleKey.Enter:
				byte[]? replay = await _networkService.GetReplay(customEntryIds[Math.Clamp(_selectedIndex, 0, customEntryIds.Count - 1)]);
				if (replay != null)
					_readerService.WriteReplayToMemory(replay);
				break;
			case ConsoleKey.UpArrow:
				ChangeSelection(_selectedIndex - 1);
				break;
			case ConsoleKey.DownArrow:
				ChangeSelection(_selectedIndex + 1);
				break;
			case ConsoleKey.LeftArrow:
				ChangeSelection(_selectedIndex - GuiUtils.PageSize);
				break;
			case ConsoleKey.RightArrow:
				ChangeSelection(_selectedIndex + GuiUtils.PageSize);
				RenderSuccessfulSubmit();
				break;
		}

		void ChangeSelection(int newIndex)
		{
			_selectedIndex = Math.Clamp(newIndex, 0, customEntryIds.Count - 1);
			_pageIndex = _selectedIndex / GuiUtils.PageSize;
			RenderSuccessfulSubmit();
		}
	}
}
