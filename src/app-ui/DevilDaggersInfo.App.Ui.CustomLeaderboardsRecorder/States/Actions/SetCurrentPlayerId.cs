using DevilDaggersInfo.App.Ui.Base.States.Actions;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetCurrentPlayerId(int CurrentPlayerId) : IAction<SetCurrentPlayerId>
{
	public void Reduce()
	{
		StateManager.RecordingState = StateManager.RecordingState with
		{
			CurrentPlayerId = CurrentPlayerId,
		};
	}
}
