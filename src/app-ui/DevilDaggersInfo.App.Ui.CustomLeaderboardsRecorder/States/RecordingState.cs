namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public record RecordingState(RecordingStateType RecordingStateType, DateTime? LastSubmission, int CurrentPlayerId)
{
	public static RecordingState GetDefault()
	{
		return new(RecordingStateType.WaitingForGame, null, 0);
	}
}
