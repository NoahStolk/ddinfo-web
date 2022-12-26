namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaSelectedHeight(float SelectedHeight) : IAction<SetArenaSelectedHeight>
{
	public void Reduce()
	{
		StateManager.ArenaEditorState = StateManager.ArenaEditorState with
		{
			SelectedHeight = SelectedHeight,
		};
	}
}
