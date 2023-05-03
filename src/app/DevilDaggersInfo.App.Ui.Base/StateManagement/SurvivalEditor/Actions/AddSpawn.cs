// using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
// using DevilDaggersInfo.Core.Spawnset;
// using System.Collections.Immutable;
//
// namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
//
// public record AddSpawn(ImmutableArray<Spawn> NewSpawns) : IAction
// {
// 	public void Reduce(StateReducer stateReducer)
// 	{
// 		stateReducer.SpawnsetState = stateReducer.SpawnsetState with
// 		{
// 			Spawnset = stateReducer.SpawnsetState.Spawnset with
// 			{
// 				Spawns = NewSpawns,
// 			},
// 		};
//
// 		SpawnsetHistoryUtils.Save(stateReducer, SpawnsetEditType.SpawnAdd);
// 	}
// }
