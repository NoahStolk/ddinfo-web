using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.States;

public record SpawnsetHistoryState(IReadOnlyList<SpawnsetHistoryEntry> History, int CurrentIndex)
{
	public static SpawnsetHistoryState GetDefault()
	{
		return new(new List<SpawnsetHistoryEntry>(), 0);
	}
}
