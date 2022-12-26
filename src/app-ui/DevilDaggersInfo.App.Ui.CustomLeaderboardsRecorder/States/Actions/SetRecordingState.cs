namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetRecordingState(RecordingStateType RecordingStateType) : IAction<SetRecordingState>
{
	public void Reduce()
	{
		StateManager.RecordingState = StateManager.RecordingState with
		{
			RecordingStateType = RecordingStateType,
		};
	}
}
