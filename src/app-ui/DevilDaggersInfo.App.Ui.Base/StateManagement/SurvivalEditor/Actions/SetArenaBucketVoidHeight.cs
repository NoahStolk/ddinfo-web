namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaBucketVoidHeight(float BucketVoidHeight) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.ArenaEditorState = stateReducer.ArenaEditorState with
		{
			BucketVoidHeight = BucketVoidHeight,
		};
	}
}
