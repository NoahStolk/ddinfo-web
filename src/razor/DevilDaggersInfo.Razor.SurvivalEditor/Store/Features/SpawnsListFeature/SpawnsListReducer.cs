using DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsListFeature.Actions;
using DevilDaggersInfo.Razor.SurvivalEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.SurvivalEditor.Store.Features.SpawnsListFeature;

public static class SpawnsListReducer
{
	[ReducerMethod]
	public static SpawnsListState ReduceSetSpawnsListAction(SpawnsListState state, SetSpawnsListAction action)
	{
		return new(action.SpawnsView);
	}
}
