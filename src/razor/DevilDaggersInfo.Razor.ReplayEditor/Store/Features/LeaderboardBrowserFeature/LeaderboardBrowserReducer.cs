using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.LeaderboardBrowserFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.LeaderboardBrowserFeature;

public static class LeaderboardBrowserReducer
{
	[ReducerMethod]
	public static LeaderboardBrowserState ReduceDownloadLeaderboardReplayAction(LeaderboardBrowserState state, DownloadLeaderboardReplayAction action)
	{
		return new(true, action.PlayerId);
	}

	[ReducerMethod]
	public static LeaderboardBrowserState ReduceDownloadLeaderboardReplaySuccessAction(LeaderboardBrowserState state, DownloadLeaderboardReplaySuccessAction action)
	{
		return state with { IsDownloading = false };
	}

	[ReducerMethod]
	public static LeaderboardBrowserState ReduceDownloadLeaderboardReplayFailureAction(LeaderboardBrowserState state, DownloadLeaderboardReplayFailureAction action)
	{
		return new(false, 0);
	}
}
