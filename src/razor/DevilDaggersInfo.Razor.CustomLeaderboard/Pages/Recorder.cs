using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Enums;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Pages;

public partial class Recorder
{
	[Inject]
	public IState<RecorderState> State { get; set; } = null!;

	[Inject]
	public ILogger<Recorder> Logger { get; set; } = null!;

	[Inject]
	public GameMemoryReaderService ReaderService { get; set; } = null!;

	[Inject]
	public StateFacade StateFacade { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(100));
		while (await timer.WaitForNextTickAsync())
		{
			await Record();
			await InvokeAsync(StateHasChanged);
		}
	}

	private async Task Record()
	{
		RecorderState state = State.Value;
		if (!state.Marker.HasValue)
		{
			StateFacade.FetchMarker();
			return;
		}

		ReaderService.Scan();

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
