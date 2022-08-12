using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Common.Utils;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Pages;

public partial class Recorder : IDisposable
{
	private Timer? _timer;

	private long? _marker;
	private State _state;
	private GetUploadSuccess? _submissionResponseWrapper;

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
	public ILogger<Recorder> Logger { get; set; } = null!;

	[Inject]
	public IClientConfiguration ClientConfiguration { get; set; } = null!;

	[Inject]
	public NetworkService NetworkService { get; set; } = null!;

	[Inject]
	public GameMemoryReaderService ReaderService { get; set; } = null!;

	[Inject]
	public UploadService UploadService { get; set; } = null!;

	[Inject]
	public StateFacade StateFacade { get; set; } = null!;

	public void Dispose()
	{
		base.Dispose();

		_timer?.Dispose();
	}

	protected override void OnInitialized()
	{
		base.OnInitialized();

		_timer = new(
			callback: async _ =>
			{
				await Record();
				await InvokeAsync(StateHasChanged);

				_timer?.Change(50, Timeout.Infinite);
			},
			state: null,
			dueTime: 0,
			period: Timeout.Infinite);
	}

	private async Task Record()
	{
		if (!_marker.HasValue)
		{
			_marker = await NetworkService.GetMarker(ClientConfiguration.GetOperatingSystem());
			if (!_marker.HasValue)
				return;
		}

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

			_state = State.Recording;
			_submissionResponseWrapper = null;
		}

		if (ReaderService.MainBlock.SurvivalHashMd5 != null && ReaderService.MainBlockPrevious.SurvivalHashMd5 != null && !ArrayUtils.AreEqual(ReaderService.MainBlock.SurvivalHashMd5, ReaderService.MainBlockPrevious.SurvivalHashMd5))
		{
			Logger.LogInformation("Fetching leaderboard because of hash change.");
#if TODO
			int spawnsetId = await NetworkService.GetSpawnsetIdByHash(ReaderService.MainBlock.SurvivalHashMd5);
			StateFacade.SetSpawnset(lb.SpawnsetId);
#endif
		}

		GameStatus status = (GameStatus)ReaderService.MainBlock.Status;
		bool waitForLocalOrLbReplay = status is GameStatus.LocalReplay or GameStatus.OwnReplayFromLeaderboard;
		if (waitForLocalOrLbReplay)
			_state = status == GameStatus.LocalReplay ? State.WaitingForLocalReplay : State.WaitingForLeaderboardReplay;

		bool justDied = !ReaderService.MainBlock.IsPlayerAlive && ReaderService.MainBlockPrevious.IsPlayerAlive;
		bool uploadRun = !waitForLocalOrLbReplay && justDied && (ReaderService.MainBlock.GameMode == 0 || ReaderService.MainBlock.TimeAttackOrRaceFinished);
		if (!uploadRun)
			return;

		_state = State.WaitingForStats;
		while (!ReaderService.MainBlock.StatsLoaded)
			await Task.Delay(TimeSpan.FromSeconds(0.5));

		_state = State.WaitingForReplay;
		while (!ReaderService.IsReplayValid())
			await Task.Delay(TimeSpan.FromSeconds(0.5));

		_state = State.Uploading;
		_submissionResponseWrapper = await UploadService.UploadRun(ReaderService.MainBlock);
		_state = State.CompletedUpload;

		// Refresh the leaderboard after submit.
#if TODO
		int spawnsetId = await NetworkService.GetSpawnsetIdByHash(ReaderService.MainBlock.SurvivalHashMd5);
		StateFacade.SetSpawnset(lb.SpawnsetId);
#endif
	}
}
