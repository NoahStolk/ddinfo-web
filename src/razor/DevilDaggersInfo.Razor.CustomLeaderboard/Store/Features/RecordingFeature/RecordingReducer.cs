using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecordingFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecordingFeature;

public static class RecordingReducer
{
	[ReducerMethod]
	public static RecordingState ReduceToggleShowEnemyStatsAction(RecordingState state, ToggleShowEnemyStatsAction action)
	{
		return state with { ShowEnemyStats = !state.ShowEnemyStats };
	}

	[ReducerMethod]
	public static RecordingState ReduceSetRecordingAction(RecordingState state, SetRecordingAction action)
	{
		return new(state.ShowEnemyStats, action.Block, action.BlockPrevious);
	}
}
