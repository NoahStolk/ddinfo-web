namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record ToggleSpawnSelection(int Index) : IAction<ToggleSpawnSelection>
{
	public void Reduce()
	{
		if (StateManager.SpawnEditorState.SelectedIndices.Contains(Index))
			StateManager.SpawnEditorState.SelectedIndices.Remove(Index);
		else
			StateManager.SpawnEditorState.SelectedIndices.Add(Index);
	}
}
