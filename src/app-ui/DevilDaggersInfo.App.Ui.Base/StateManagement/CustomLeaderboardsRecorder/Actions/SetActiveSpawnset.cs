namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetActiveSpawnset(string? Name) : IAction
{
	public void Reduce()
	{
		StateManager.ActiveSpawnsetState = new(Name);
	}
}
