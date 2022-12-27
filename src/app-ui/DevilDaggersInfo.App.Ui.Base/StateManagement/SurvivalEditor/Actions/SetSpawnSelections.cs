namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetSpawnSelections(List<int> SelectedIndices) : IAction
{
	public void Reduce()
	{
		StateManager.SpawnEditorState = new(SelectedIndices);
	}
}
