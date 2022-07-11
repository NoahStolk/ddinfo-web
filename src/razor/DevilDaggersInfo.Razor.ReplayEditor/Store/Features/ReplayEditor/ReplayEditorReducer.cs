using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditor.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditor;

public static class ReplayEditorReducer
{
	[ReducerMethod]
	public static ReplayEditorState ReduceOpenBinaryAction(ReplayEditorState state, OpenReplayAction action)
		=> new(action.ReplayBinary, action.Name, 0, 60);

	[ReducerMethod]
	public static ReplayEditorState ReduceSelectTickRangeAction(ReplayEditorState state, SelectTickRangeAction action)
		=> new(state.ReplayBinary, state.Name, action.StartTick, action.EndTick);
}
