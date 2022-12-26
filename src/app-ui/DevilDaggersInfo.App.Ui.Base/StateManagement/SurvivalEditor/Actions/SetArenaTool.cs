using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetArenaTool(ArenaTool ArenaTool) : IAction<SetArenaTool>
{
	public void Reduce()
	{
		StateManager.ArenaEditorState = StateManager.ArenaEditorState with
		{
			ArenaTool = ArenaTool,
		};
	}
}
