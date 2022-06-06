using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.CustomLeaderboard.Configuration;
using DevilDaggersInfo.Core.CustomLeaderboard.Enums;
using DevilDaggersInfo.Core.CustomLeaderboard.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevilDaggersInfo.Core.CustomLeaderboard.Services;

public class RecorderService
{
	private const int _mainLoopSleepMilliseconds = 50;

	private readonly NetworkService _networkService;
	private readonly ReaderService _readerService;
	private readonly UploadService _uploadService;
	private readonly ILogger<RecorderService> _logger;
	private readonly IOptions<ClientOptions> _clientInfo;

	private bool _isRecording = true;
	private long _marker;
	private int _selectedIndex;
	private int _pageIndex;
	private GetUploadSuccess? _uploadSuccess;
	private MainBlock _finalRecordedMainBlock;

	public RecorderService(NetworkService networkService, ReaderService readerService, UploadService uploadService, ILogger<RecorderService> logger, IOptions<ClientOptions> clientInfo)
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
			await ExecuteMainLoop();
		}
	}

	private async Task ExecuteMainLoop()
	{
		_readerService.FindWindow();
		if (_readerService.Process == null)
		{
			_readerService.IsInitialized = false;
			await Task.Delay(TimeSpan.FromSeconds(1));
			return;
		}

		_readerService.Initialize(_marker);
		if (!_readerService.IsInitialized)
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
			return;
		}

		_readerService.Scan();

		if (!_isRecording)
		{
			if (_readerService.MainBlock.Time == _readerService.MainBlockPrevious.Time || _readerService.MainBlock.Status == (int)GameStatus.LocalReplay)
				return;

			_isRecording = true;
			_uploadSuccess = null;
		}

		await Task.Delay(TimeSpan.FromMilliseconds(_mainLoopSleepMilliseconds));

		bool justDied = !_readerService.MainBlock.IsPlayerAlive && _readerService.MainBlockPrevious.IsPlayerAlive;
		bool uploadRun = justDied && (_readerService.MainBlock.GameMode == 0 || _readerService.MainBlock.TimeAttackOrRaceFinished);
		if (!uploadRun)
			return;

		if (!_readerService.MainBlock.StatsLoaded)
		{
			await Task.Delay(TimeSpan.FromSeconds(0.5));
			return;
		}

		if (!_readerService.IsReplayValid())
		{
			await Task.Delay(TimeSpan.FromSeconds(0.5));
			return;
		}

		_isRecording = false;

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
			}
			else
			{
				await Task.Delay(TimeSpan.FromSeconds(0.5));
			}
		}
		else
		{
			_logger.LogWarning("Validation failed - {errorMessage}", errorMessage);

			await Task.Delay(TimeSpan.FromSeconds(0.5));
		}
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
}
