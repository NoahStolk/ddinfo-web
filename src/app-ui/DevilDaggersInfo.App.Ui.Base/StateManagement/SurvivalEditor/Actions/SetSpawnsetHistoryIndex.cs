namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

/// <summary>
/// Fires when changing history index (undo, redo, clicking an entry).
/// This also reloads the spawnset.
/// </summary>
public record SetSpawnsetHistoryIndex(int Index) : IAction<SetSpawnsetHistoryIndex>
{
	public void Reduce()
	{
		StateManager.SpawnsetHistoryState = StateManager.SpawnsetHistoryState with
		{
			CurrentIndex = Math.Clamp(Index, 0, StateManager.SpawnsetHistoryState.History.Count - 1),
		};

		StateManager.SpawnsetState = StateManager.SpawnsetState with
		{
			Spawnset = StateManager.SpawnsetHistoryState.History[StateManager.SpawnsetHistoryState.CurrentIndex].Spawnset.DeepCopy(),
		};
	}
}
