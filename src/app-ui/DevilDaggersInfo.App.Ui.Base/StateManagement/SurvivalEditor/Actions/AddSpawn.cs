using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.Core.Spawnset;
using System.Collections.Immutable;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record AddSpawn(ImmutableArray<Spawn> NewSpawns) : IAction<AddSpawn>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetState.Spawnset with
			{
				Spawns = NewSpawns,
			},
		};

		SpawnsetHistoryUtils.Save(SpawnsetEditType.SpawnAdd);
	}
}
