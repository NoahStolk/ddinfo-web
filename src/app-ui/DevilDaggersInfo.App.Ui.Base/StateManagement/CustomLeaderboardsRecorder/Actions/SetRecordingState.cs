using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

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
