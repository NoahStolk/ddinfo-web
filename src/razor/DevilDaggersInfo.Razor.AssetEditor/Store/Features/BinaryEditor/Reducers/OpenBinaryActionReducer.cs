using DevilDaggersInfo.Razor.AssetEditor.Store.Features.BinaryEditor.Actions;
using DevilDaggersInfo.Razor.AssetEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.AssetEditor.Store.Features.BinaryEditor.Reducers;

public static class OpenBinaryActionReducer
{
	[ReducerMethod]
	public static BinaryEditorState ReduceSetBinaryAction(BinaryEditorState state, OpenBinaryAction action)
		=> new(action.ModBinary, action.Name);
}
