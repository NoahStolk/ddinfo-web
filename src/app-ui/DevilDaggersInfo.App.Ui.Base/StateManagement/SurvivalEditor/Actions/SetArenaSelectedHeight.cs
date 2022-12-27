namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaSelectedHeight(float SelectedHeight) : IAction
{
	public void Reduce()
	{
		StateManager.ArenaEditorState = StateManager.ArenaEditorState with
		{
			SelectedHeight = SelectedHeight,
		};
	}
}
