namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaBucketVoidHeight(float BucketVoidHeight) : IAction
{
	public void Reduce()
	{
		StateManager.ArenaEditorState = StateManager.ArenaEditorState with
		{
			BucketVoidHeight = BucketVoidHeight,
		};
	}
}
