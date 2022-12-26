namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record ClearSpawnSelections : IAction<ClearSpawnSelections>
{
	public void Reduce()
	{
		StateManager.SpawnEditorState.SelectedIndices.Clear();
	}
}
