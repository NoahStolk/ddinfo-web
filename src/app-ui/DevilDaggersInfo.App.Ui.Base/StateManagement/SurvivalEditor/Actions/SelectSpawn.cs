namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SelectSpawn(int Index) : IAction<SelectSpawn>
{
	public void Reduce()
	{
		// TODO: Mutable list OK?
		if (!StateManager.SpawnEditorState.SelectedIndices.Contains(Index))
			StateManager.SpawnEditorState.SelectedIndices.Add(Index);
	}
}
