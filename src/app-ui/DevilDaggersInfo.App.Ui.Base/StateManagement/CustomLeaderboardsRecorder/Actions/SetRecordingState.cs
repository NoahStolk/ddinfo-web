using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Data;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetRecordingState(RecordingStateType RecordingStateType) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.RecordingState = StateManager.RecordingState with
		{
			RecordingStateType = RecordingStateType,
		};
	}
}
