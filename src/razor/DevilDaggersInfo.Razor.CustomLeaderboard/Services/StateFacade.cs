using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Enums;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecordingFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Actions;
using DevilDaggersInfo.Types.Web;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Services;

public class StateFacade
{
	private readonly IDispatcher _dispatcher;

	public StateFacade(IDispatcher dispatcher)
	{
		_dispatcher = dispatcher;
	}

	public void LoadLeaderboards()
	{
		_dispatcher.Dispatch(new FetchLeaderboardsAction());
	}

	public void SetPageIndex(int pageIndex)
	{
		_dispatcher.Dispatch(new SetPageIndexAction(pageIndex));
		_dispatcher.Dispatch(new FetchLeaderboardsAction());
	}

	public void SetCategory(CustomLeaderboardCategory category)
	{
		_dispatcher.Dispatch(new SetCategoryAction(category));
		_dispatcher.Dispatch(new FetchLeaderboardsAction());
	}

	public void SetSelectedPlayerId(int selectedPlayerId)
	{
		_dispatcher.Dispatch(new SetSelectedPlayerIdAction(selectedPlayerId));
		_dispatcher.Dispatch(new FetchLeaderboardsAction());
	}

	public void SetSpawnset(int spawnsetId)
	{
		_dispatcher.Dispatch(new DownloadSpawnsetAction(spawnsetId));
	}

	public void SetReplay(int customEntryId)
	{
		_dispatcher.Dispatch(new SetReplayAction(customEntryId));
	}

	public void ToggleShowEnemyStats()
	{
		_dispatcher.Dispatch(new ToggleShowEnemyStatsAction());
	}

	public void SetState(RecorderStateType state)
	{
		_dispatcher.Dispatch(new SetStateAction(state));
	}

	public void SetRecording(MainBlock block, MainBlock blockPrevious)
	{
		_dispatcher.Dispatch(new SetRecordingAction(block, blockPrevious));
	}

	public void UploadRun()
	{
		_dispatcher.Dispatch(new UploadRunAction());
	}
}
