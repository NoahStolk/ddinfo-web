namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetMarker(long Value) : IAction<SetMarker>
{
	public void Reduce()
	{
		StateManager.MarkerState = new(Value);
	}
}
