using DevilDaggersInfo.Core.CustomLeaderboard.Enums;
using DevilDaggersInfo.Core.CustomLeaderboard.Memory;
using DevilDaggersInfo.Core.CustomLeaderboard.Models;
using DevilDaggersInfo.Core.CustomLeaderboard.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.CustomLeaderboard.Pages;

public partial class Recorder : IDisposable
{
	private Timer? _timer;

	private long? _marker;
	private State _state;
	private SubmissionResponseWrapper? _submissionResponseWrapper;
	private MainBlock? _finalRecordedMainBlock;
	private string? _localError;
	private bool? _leaderboardExists;

	private enum State
	{
		Recording,
		WaitingForRestart,
		WaitingForLocalReplay,
		WaitingForLeaderboardReplay,
		WaitingForStats,
		WaitingForReplay,
		Uploading,
		CompletedUpload,
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
		// Don't start more threads and alter the main block when we are uploading.
		if (_state is State.WaitingForStats or State.WaitingForReplay or State.Uploading)
			return;

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

			GameStatus status = (GameStatus)ReaderService.MainBlock.Status;
			if (status is GameStatus.LocalReplay or GameStatus.OwnReplayFromLeaderboard)
			{
				_state = status == GameStatus.LocalReplay ? State.WaitingForLocalReplay : State.WaitingForLeaderboardReplay;
				return;
			}

			_state = State.Recording;
			ClearUploadState();
		}

		bool justDied = !ReaderService.MainBlock.IsPlayerAlive && ReaderService.MainBlockPrevious.IsPlayerAlive;
		bool uploadRun = justDied && (ReaderService.MainBlock.GameMode == 0 || ReaderService.MainBlock.TimeAttackOrRaceFinished);
		if (!uploadRun)
			return;

		_state = State.WaitingForStats;
		while (!ReaderService.MainBlock.StatsLoaded)
			await Task.Delay(TimeSpan.FromSeconds(0.5));

		_state = State.WaitingForReplay;
		while (!ReaderService.IsReplayValid())
			await Task.Delay(TimeSpan.FromSeconds(0.5));

		_state = State.Uploading;

		_localError = ReaderService.ValidateRunLocally();
		if (_localError == null)
		{
			_leaderboardExists = await NetworkService.CheckIfLeaderboardExists(ReaderService.MainBlock.SurvivalHashMd5);
			if (!_leaderboardExists.Value)
				return;

			_submissionResponseWrapper = await UploadService.UploadRun();
			_finalRecordedMainBlock = ReaderService.MainBlock;
		}

		_state = State.CompletedUpload;
	}

	private void ClearUploadState()
	{
		_submissionResponseWrapper = null;
		_finalRecordedMainBlock = null;
		_localError = null;
		_leaderboardExists = null;
	}
}
