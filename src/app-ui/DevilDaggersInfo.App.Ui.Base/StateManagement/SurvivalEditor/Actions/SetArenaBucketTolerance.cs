namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaBucketTolerance(float BucketTolerance) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.ArenaEditorState = StateManager.ArenaEditorState with
		{
			BucketTolerance = BucketTolerance,
		};
	}
}
