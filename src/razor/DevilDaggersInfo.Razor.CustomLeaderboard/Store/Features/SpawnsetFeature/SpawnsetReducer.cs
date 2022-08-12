using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.SpawnsetFeature;

public static class SpawnsetReducer
{
	[ReducerMethod]
	public static SpawnsetState ReduceDownloadSpawnsetAction(SpawnsetState state, DownloadSpawnsetAction action)
	{
		return new(true, null, state.Spawnset);
	}

	[ReducerMethod]
	public static SpawnsetState ReduceDownloadSpawnsetSuccessAction(SpawnsetState state, DownloadSpawnsetSuccessAction action)
	{
		return new(false, null, action.Spawnset);
	}

	[ReducerMethod]
	public static SpawnsetState ReduceDownloadSpawnsetFailureAction(SpawnsetState state, DownloadSpawnsetFailureAction action)
	{
		return new(false, action.Error, null);
	}
}
