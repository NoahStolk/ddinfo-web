namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record LoadSpawnsetFromHistory : IAction<LoadSpawnsetFromHistory>
{
	public void Reduce()
	{
		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetHistoryState.History[StateManager.SpawnsetHistoryState.CurrentIndex].Spawnset.DeepCopy(),
		};
	}
}
