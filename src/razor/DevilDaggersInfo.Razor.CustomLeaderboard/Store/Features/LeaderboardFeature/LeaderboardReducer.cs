using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardFeature;

public static class LeaderboardReducer
{
	// TODO: Use different state for spawnset.
	[ReducerMethod]
	public static LeaderboardState ReduceDownloadSpawnsetAction(LeaderboardState state, DownloadSpawnsetAction action)
	{
		return new(true, null, state.Leaderboard, state.Spawnset);
	}

	[ReducerMethod]
	public static LeaderboardState ReduceDownloadSpawnsetSuccessAction(LeaderboardState state, DownloadSpawnsetSuccessAction action)
	{
		return new(false, null, state.Leaderboard, action.Spawnset);
	}

	[ReducerMethod]
	public static LeaderboardState ReduceDownloadSpawnsetFailureAction(LeaderboardState state, DownloadSpawnsetFailureAction action)
	{
		return new(false, action.Error, null, null);
	}

	[ReducerMethod]
	public static LeaderboardState ReduceDownloadLeaderboardAction(LeaderboardState state, DownloadLeaderboardAction action)
	{
		return new(true, null, state.Leaderboard, state.Spawnset);
	}

	[ReducerMethod]
	public static LeaderboardState ReduceDownloadLeaderboardSuccessAction(LeaderboardState state, DownloadLeaderboardSuccessAction action)
	{
		return new(false, null, action.Leaderboard, state.Spawnset);
	}

	[ReducerMethod]
	public static LeaderboardState ReduceDownloadLeaderboardFailureAction(LeaderboardState state, DownloadLeaderboardFailureAction action)
	{
		return new(false, action.Error, null, null);
	}
}
