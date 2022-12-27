namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetSpawnsetHistoryIndex(int Index) : IAction<SetSpawnsetHistoryIndex>
{
	public void Reduce()
	{
		StateManager.SpawnsetHistoryState = StateManager.SpawnsetHistoryState with
		{
			CurrentIndex = Math.Clamp(Index, 0, StateManager.SpawnsetHistoryState.History.Count - 1),
		};
	}
}
