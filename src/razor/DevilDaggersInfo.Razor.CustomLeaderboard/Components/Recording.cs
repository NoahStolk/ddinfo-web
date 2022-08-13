using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Enums;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Components;

public partial class Recording
{
	private Timer? _timer;

	[Inject]
	public ILogger<Recording> Logger { get; set; } = null!;

	[Inject]
	public GameMemoryReaderService ReaderService { get; set; } = null!;

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
		RecorderState state = RecorderState.Value;
		ReaderService.Scan();
		StateFacade.SetRecording(ReaderService.MainBlock, ReaderService.MainBlockPrevious);

		if (LeaderboardListState.Value.SelectedPlayerId != ReaderService.MainBlock.PlayerId)
		{
			StateFacade.SetSelectedPlayerId(ReaderService.MainBlock.PlayerId);
		}

		// TODO: Load spawnset and leaderboard if empty (on startup).
		//if (SpawnsetState.Value.Spawnset == null)
		//{
		//	StateFacade.SetSpawnset()
		//}

		if (state.State != RecorderStateType.Recording)
		{
			if (ReaderService.MainBlock.Time == ReaderService.MainBlockPrevious.Time)
			{
				StateFacade.SetState(RecorderStateType.WaitingForRestart);
				return;
			}

			StateFacade.SetState(RecorderStateType.Recording);
		}

		GameStatus status = (GameStatus)ReaderService.MainBlock.Status;
		if (status == GameStatus.LocalReplay)
		{
			StateFacade.SetState(RecorderStateType.WaitingForLocalReplay);
			return;
		}

		if (status == GameStatus.OwnReplayFromLeaderboard)
		{
			StateFacade.SetState(RecorderStateType.WaitingForLeaderboardReplay);
			return;
		}

		bool justDied = !ReaderService.MainBlock.IsPlayerAlive && ReaderService.MainBlockPrevious.IsPlayerAlive;
		bool uploadRun = justDied && (ReaderService.MainBlock.GameMode == 0 || ReaderService.MainBlock.TimeAttackOrRaceFinished);
		if (!uploadRun)
			return;

		StateFacade.SetState(RecorderStateType.WaitingForStats);
		while (!ReaderService.MainBlock.StatsLoaded)
			await Task.Delay(TimeSpan.FromSeconds(0.5));

		StateFacade.SetState(RecorderStateType.WaitingForReplay);
		while (!ReaderService.IsReplayValid())
			await Task.Delay(TimeSpan.FromSeconds(0.5));

		StateFacade.UploadRun();
	}
}
