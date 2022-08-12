using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecordingFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecordingFeature;

public static class RecordingReducer
{
	[ReducerMethod]
	public static RecordingState ReduceToggleShowEnemyStatsAction(RecordingState state, ToggleShowEnemyStatsAction action)
	{
		return new(!state.ShowEnemyStats);
	}
}
