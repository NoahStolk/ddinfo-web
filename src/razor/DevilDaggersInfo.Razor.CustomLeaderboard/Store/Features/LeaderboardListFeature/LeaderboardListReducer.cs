using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using DevilDaggersInfo.Razor.CustomLeaderboard.Utils;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.LeaderboardListFeature;

public static class LeaderboardListReducer
{
	[ReducerMethod]
	public static LeaderboardListState ReduceSetPageIndexAction(LeaderboardListState state, SetPageIndexAction action)
	{
		int totalPages = state.Leaderboards == null ? 0 : PageUtils.GetTotalPages(state.PageSize, state.Leaderboards.TotalResults);
		return new(false, null, state.Leaderboards, state.SelectedPlayerId, state.Category, Math.Clamp(action.PageIndex, 0, totalPages - 1), state.PageSize);
	}

	[ReducerMethod]
	public static LeaderboardListState ReduceSetCategoryAction(LeaderboardListState state, SetCategoryAction action)
	{
		return new(false, null, state.Leaderboards, state.SelectedPlayerId, action.Category, 0, state.PageSize);
	}

	[ReducerMethod]
	public static LeaderboardListState ReduceSetSelectedPlayerIdAction(LeaderboardListState state, SetSelectedPlayerIdAction action)
	{
		return new(false, null, state.Leaderboards, action.SelectedPlayerId, state.Category, state.PageIndex, state.PageSize);
	}

	[ReducerMethod]
	public static LeaderboardListState ReduceFetchLeaderboardsAction(LeaderboardListState state, FetchLeaderboardsAction action)
	{
		return new(true, null, state.Leaderboards, state.SelectedPlayerId, state.Category, state.PageIndex, state.PageSize);
	}

	[ReducerMethod]
	public static LeaderboardListState ReduceFetchLeaderboardsSuccessAction(LeaderboardListState state, FetchLeaderboardsSuccessAction action)
	{
		return new(false, null, action.Leaderboards, state.SelectedPlayerId, state.Category, state.PageIndex, state.PageSize);
	}

	[ReducerMethod]
	public static LeaderboardListState ReduceFetchLeaderboardsFailureAction(LeaderboardListState state, FetchLeaderboardsFailureAction action)
	{
		return new(false, action.Error, null, state.SelectedPlayerId, state.Category, state.PageIndex, state.PageSize);
	}
}
