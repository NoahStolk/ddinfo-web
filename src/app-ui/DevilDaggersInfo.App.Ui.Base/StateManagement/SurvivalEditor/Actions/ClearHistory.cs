using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

public record ClearHistory(SpawnsetHistoryEntry InitialHistoryEntry) : IAction
{
	public void Reduce()
	{
		StateManager.SpawnsetHistoryState = new(new List<SpawnsetHistoryEntry> { InitialHistoryEntry }, 0);
	}
}
