using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

/// <summary>
/// Fires when the list of history entries is modified.
/// This does NOT reload a history entry.
/// </summary>
public record SetHistory(List<SpawnsetHistoryEntry> History, int CurrentIndex) : IAction
{
	public void Reduce()
	{
		StateManager.SpawnsetHistoryState = new(History, CurrentIndex);
	}
}
