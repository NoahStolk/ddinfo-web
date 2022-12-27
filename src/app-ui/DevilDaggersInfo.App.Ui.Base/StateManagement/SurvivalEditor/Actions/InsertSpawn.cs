using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.Core.Spawnset;
using System.Collections.Immutable;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record InsertSpawn(ImmutableArray<Spawn> NewSpawns) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				Spawns = NewSpawns,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.SpawnInsert);
	}
}
