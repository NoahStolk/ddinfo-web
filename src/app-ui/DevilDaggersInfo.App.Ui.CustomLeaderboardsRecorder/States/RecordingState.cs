namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public record RecordingState(RecordingStateType RecordingStateType)
{
	public static RecordingState GetDefault()
	{
		return new(RecordingStateType.WaitingForGame);
	}
}
