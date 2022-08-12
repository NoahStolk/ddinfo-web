using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature.Actions;
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
}
