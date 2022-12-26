using DevilDaggersInfo.App.Ui.Base.States.Actions;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetLastSubmission(DateTime LastSubmission) : IAction<SetLastSubmission>
{
	public void Reduce()
	{
		StateManager.RecordingState = StateManager.RecordingState with
		{
			LastSubmission = LastSubmission,
		};
	}
}
