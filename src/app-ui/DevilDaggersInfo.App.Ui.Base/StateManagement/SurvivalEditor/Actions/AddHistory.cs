using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;

/// <summary>
/// Fires when a history entry is added.
/// This does NOT reload a history entry.
/// </summary>
public record AddHistory(SpawnsetHistoryEntry HistoryEntry) : IAction
{
	private const int _maxHistoryEntries = 100;

	public void Reduce()
	{
		// Clear any newer history.
		List<SpawnsetHistoryEntry> newHistory = StateManager.SpawnsetHistoryState.History.ToList();
		newHistory = newHistory.Take(StateManager.SpawnsetHistoryState.CurrentIndex + 1).Append(HistoryEntry).ToList();

		int newCurrentIndex = StateManager.SpawnsetHistoryState.CurrentIndex + 1;
		if (newHistory.Count > _maxHistoryEntries)
		{
			newHistory.RemoveAt(0);
			newCurrentIndex--;
		}

		StateManager.SpawnsetHistoryState = new(newHistory, newCurrentIndex);
	}
}
