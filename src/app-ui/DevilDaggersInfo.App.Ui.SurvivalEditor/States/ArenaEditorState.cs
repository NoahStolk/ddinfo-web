using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public record ArenaEditorState(float SelectedHeight, ArenaTool ArenaTool)
{
	public static ArenaEditorState GetDefault()
	{
		return new(0, ArenaTool.Pencil);
	}
}
