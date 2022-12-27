namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetLastSubmission(DateTime LastSubmission) : IAction
{
	public void Reduce()
	{
		StateManager.RecordingState = StateManager.RecordingState with
		{
			LastSubmission = LastSubmission,
		};
	}
}
