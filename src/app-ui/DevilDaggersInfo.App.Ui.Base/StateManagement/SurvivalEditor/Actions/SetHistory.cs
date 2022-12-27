using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record SetHistory(List<SpawnsetHistoryEntry> History) : IAction<SetHistory>
{
	public void Reduce()
	{
		StateManager.SpawnsetHistoryState = StateManager.SpawnsetHistoryState with
		{
			History = History,
		};
	}
}
