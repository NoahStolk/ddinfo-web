namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetSpawnsetHistoryIndex(int Index, int Count) : IAction<SetSpawnsetHistoryIndex>
{
	public void Reduce()
	{
		StateManager.SpawnsetHistoryState = new(Math.Clamp(Index, 0, Count - 1), Count);
	}
}
