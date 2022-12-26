namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetActiveSpawnset(string? Name) : IAction<SetActiveSpawnset>
{
	public void Reduce()
	{
		StateManager.ActiveSpawnsetState = new(Name);
	}
}
