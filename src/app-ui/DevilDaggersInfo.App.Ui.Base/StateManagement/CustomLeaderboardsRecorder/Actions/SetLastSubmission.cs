namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetLastSubmission(DateTime LastSubmission) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.RecordingState = stateReducer.RecordingState with
		{
			LastSubmission = LastSubmission,
		};
	}
}
