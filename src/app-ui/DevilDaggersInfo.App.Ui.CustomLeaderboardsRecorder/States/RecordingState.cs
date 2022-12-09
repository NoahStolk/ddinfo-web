namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public record RecordingState(RecordingStateType RecordingStateType, DateTime? LastSubmission)
{
	public static RecordingState GetDefault()
	{
		return new(RecordingStateType.WaitingForGame, null);
	}
}
