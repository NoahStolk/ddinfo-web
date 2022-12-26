namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.States;

public record SpawnsetHistoryState(int CurrentIndex, int Count)
{
	public static SpawnsetHistoryState GetDefault()
	{
		return new(0, 0);
	}
}
