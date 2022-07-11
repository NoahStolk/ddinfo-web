using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature;

public static class ReplayBinaryReducer
{
	[ReducerMethod]
	public static ReplayBinaryState ReduceOpenBinaryAction(ReplayBinaryState state, OpenReplayAction action)
	{
		return new(action.ReplayBinary, action.Name);
	}
}
