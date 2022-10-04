using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public record ArenaEditorState(float SelectedHeight, ArenaTool ArenaTool, float BucketTolerance, float BucketVoidHeight)
{
	public static ArenaEditorState GetDefault()
	{
		return new(0, ArenaTool.Pencil, 0.1f, -2);
	}
}
