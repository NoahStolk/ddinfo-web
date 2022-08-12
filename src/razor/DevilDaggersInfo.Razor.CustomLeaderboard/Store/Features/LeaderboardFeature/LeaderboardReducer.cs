using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature;

public static class LeaderboardReducer
{
	[ReducerMethod]
	public static LeaderboardState ReduceDownloadLeaderboardSuccessAction(LeaderboardState state, DownloadLeaderboardSuccessAction action)
	{
		return new(null, action.Leaderboard);
	}

	[ReducerMethod]
	public static LeaderboardState ReduceDownloadLeaderboardFailureAction(LeaderboardState state, DownloadLeaderboardFailureAction action)
	{
		return new(action.Error, null);
	}
}
