using DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsetBinaryFeature.Actions;
using DevilDaggersInfo.Razor.SurvivalEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsetBinaryFeature;

public static class SpawnsetBinaryReducer
{
	[ReducerMethod]
	public static SpawnsetBinaryState ReduceOpenSpawnsetAction(SpawnsetBinaryState state, OpenSpawnsetAction action)
	{
		return new(action.SpawnsetBinary, action.Name);
	}
}
