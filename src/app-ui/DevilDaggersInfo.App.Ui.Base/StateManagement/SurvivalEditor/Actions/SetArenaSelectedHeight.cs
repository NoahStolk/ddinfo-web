namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaSelectedHeight(float SelectedHeight) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.ArenaEditorState = StateManager.ArenaEditorState with
		{
			SelectedHeight = SelectedHeight,
		};
	}
}
