namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetCurrentPlayerId(int CurrentPlayerId) : IAction
{
	public void Reduce()
	{
		StateManager.RecordingState = StateManager.RecordingState with
		{
			CurrentPlayerId = CurrentPlayerId,
		};
	}
}
