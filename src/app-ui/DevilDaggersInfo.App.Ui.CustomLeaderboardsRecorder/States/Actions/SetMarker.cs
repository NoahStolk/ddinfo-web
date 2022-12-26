namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetMarker(long Value) : IAction<SetMarker>
{
	public void Reduce()
	{
		StateManager.MarkerState = new(Value);
	}
}
