namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetActiveSpawnset(string? Name) : IAction<SetActiveSpawnset>
{
	public void Reduce()
	{
		StateManager.ActiveSpawnsetState = new(Name);
	}
}
