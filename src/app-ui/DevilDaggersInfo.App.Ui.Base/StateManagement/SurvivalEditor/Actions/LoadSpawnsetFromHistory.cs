using DevilDaggersInfo.Core.Spawnset;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record LoadSpawnsetFromHistory(SpawnsetBinary SpawnsetBinary) : IAction<LoadSpawnsetFromHistory>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = SpawnsetBinary,
		};
	}
}
