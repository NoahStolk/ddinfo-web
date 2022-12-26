namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record DeselectSpawn(int Index) : IAction<DeselectSpawn>
{
	public void Reduce()
	{
		// TODO: Mutable list OK?
		if (StateManager.SpawnEditorState.SelectedIndices.Contains(Index))
			StateManager.SpawnEditorState.SelectedIndices.Remove(Index);
	}
}
