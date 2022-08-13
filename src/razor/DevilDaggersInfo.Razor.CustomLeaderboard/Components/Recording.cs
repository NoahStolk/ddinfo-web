using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Enums;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Components;

public partial class Recording
{
	[Parameter]
	[EditorRequired]
	public MainBlock Block { get; set; }

	[Parameter]
	[EditorRequired]
	public MainBlock BlockPrevious { get; set; }

	[Inject]
	public ILogger<Recording> Logger { get; set; } = null!;

	[Inject]
	public GameMemoryReaderService ReaderService { get; set; } = null!;

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
		RecorderState state = RecorderState.Value;
		if (!state.Marker.HasValue)
		{
			StateFacade.FetchMarker();
			return;
		}

		ReaderService.Scan();

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
