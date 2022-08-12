using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Actions;
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

	public void SetSpawnset(int spawnsetId)
	{
		_dispatcher.Dispatch(new DownloadSpawnsetAction(spawnsetId));
	}

	public void SetReplay(int customEntryId)
	{
		//byte[]? replayData = await NetworkService.GetReplay(customEntryId);
		//if (replayData != null)
		//	_gameMemoryReaderService.WriteReplayToMemory(replayData);
	}
}
