using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.States;

public record ArenaEditorState(float SelectedHeight, ArenaTool ArenaTool, float BucketTolerance, float BucketVoidHeight)
{
	public static ArenaEditorState GetDefault()
	{
		return new(0, ArenaTool.Pencil, 0.1f, -2);
	}
}
