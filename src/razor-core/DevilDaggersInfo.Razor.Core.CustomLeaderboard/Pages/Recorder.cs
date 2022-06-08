using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.CustomLeaderboard.Enums;
using DevilDaggersInfo.Core.CustomLeaderboard.Memory;
using DevilDaggersInfo.Core.CustomLeaderboard.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.CustomLeaderboard.Pages;

public partial class Recorder : IDisposable
{
	private Timer? _timer;

	private long? _marker;
	private State _state;
	private MainBlock _finalRecordedMainBlock;
	private GetUploadSuccess? _uploadSuccess;
	private string? _localError;
	private bool? _leaderboardExists;

	private enum State
	{
		Recording = 0,
		WaitingForRestart = 1,
		WaitingForLocalReplay = 2,
		Uploading = 3,
	}

	[Inject]
	public IClientConfiguration ClientConfiguration { get; set; } = null!;

	[Inject]
	public NetworkService NetworkService { get; set; } = null!;

	[Inject]
	public ReaderService ReaderService { get; set; } = null!;

	[Inject]
	public UploadService UploadService { get; set; } = null!;

	public void Dispose()
	{
		_timer?.Dispose();
	}

	protected override void OnInitialized()
	{
		_timer = new(
			callback: async _ =>
			{
				await Record();
				await InvokeAsync(StateHasChanged);
			},
			state: null,
			dueTime: 0,
			period: 50);
	}

	private async Task Record()
	{
		if (!_marker.HasValue)
		{
			_marker = await NetworkService.GetMarker(ClientConfiguration.GetOperatingSystem());
			if (!_marker.HasValue)
				return;
		}

		if (!ReaderService.FindWindow())
			return;

		if (!ReaderService.Initialize(_marker.Value))
			return;

		ReaderService.Scan();

		if (_state != State.Recording)
		{
			if (ReaderService.MainBlock.Time == ReaderService.MainBlockPrevious.Time)
			{
				_state = State.WaitingForRestart;
				return;
			}

			if (ReaderService.MainBlock.Status == (int)GameStatus.LocalReplay)
			{
				_state = State.WaitingForLocalReplay;
				return;
			}

			_state = State.Recording;
			ClearUploadState();
		}

		bool justDied = !ReaderService.MainBlock.IsPlayerAlive && ReaderService.MainBlockPrevious.IsPlayerAlive;
		bool uploadRun = justDied && (ReaderService.MainBlock.GameMode == 0 || ReaderService.MainBlock.TimeAttackOrRaceFinished);
		if (!uploadRun)
			return;

		// TODO: We need to pause the timer here so it doesn't re-read memory while we're uploading the run...
		// When the validation is built, then the memory is re-read, and then we create the request, it will be invalid.
		if (!ReaderService.MainBlock.StatsLoaded)
		{
			await Task.Delay(TimeSpan.FromSeconds(0.5));
			return;
		}

		if (!ReaderService.IsReplayValid())
		{
			await Task.Delay(TimeSpan.FromSeconds(0.5));
			return;
		}

		_state = State.Uploading;

		_localError = ReaderService.ValidateRunLocally();
		if (_localError == null)
		{
			_leaderboardExists = await NetworkService.CheckIfLeaderboardExists(ReaderService.MainBlock.SurvivalHashMd5);
			if (!_leaderboardExists.Value)
				return;

			GetUploadSuccess? uploadSuccess = await UploadService.UploadRun();
			if (uploadSuccess != null)
			{
				_uploadSuccess = uploadSuccess;
				_finalRecordedMainBlock = ReaderService.MainBlock;
			}
		}
	}

	private void ClearUploadState()
	{
		_uploadSuccess = null;
		_localError = null;
		_leaderboardExists = null;
	}
}
