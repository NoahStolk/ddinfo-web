namespace DevilDaggersInfo.App.Ui.SurvivalEditor.States;

public record SpawnEditorState(List<int> SelectedIndices)
{
	public static SpawnEditorState GetDefault()
	{
		return new(new List<int>());
	}
}
