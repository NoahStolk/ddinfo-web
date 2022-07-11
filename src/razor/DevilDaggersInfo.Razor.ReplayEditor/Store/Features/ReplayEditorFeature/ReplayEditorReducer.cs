using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature;

public static class ReplayEditorReducer
{
	[ReducerMethod]
	public static ReplayEditorState ReduceSelectTickRangeAction(ReplayEditorState state, SelectTickRangeAction action)
	{
		int startTick = Math.Max(0, action.StartTick);
		int endTick = Math.Max(action.EndTick, startTick + 1);

		return new(startTick, endTick, state.OpenedTicks);
	}

	[ReducerMethod]
	public static ReplayEditorState ReduceToggleTickAction(ReplayEditorState state, ToggleTickAction action)
	{
		List<int> openedTicks = new(state.OpenedTicks);
		if (openedTicks.Contains(action.Tick))
			openedTicks.Remove(action.Tick);
		else
			openedTicks.Add(action.Tick);

		return new(state.StartTick, state.EndTick, openedTicks);
	}
}
