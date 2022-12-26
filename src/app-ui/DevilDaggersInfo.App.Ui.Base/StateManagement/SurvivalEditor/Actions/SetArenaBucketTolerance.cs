namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaBucketTolerance(float BucketTolerance) : IAction<SetArenaBucketTolerance>
{
	public void Reduce()
	{
		StateManager.ArenaEditorState = StateManager.ArenaEditorState with
		{
			BucketTolerance = BucketTolerance,
		};
	}
}
