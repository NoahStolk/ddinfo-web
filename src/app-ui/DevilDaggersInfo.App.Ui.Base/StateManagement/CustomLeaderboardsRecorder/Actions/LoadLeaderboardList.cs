namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record LoadLeaderboardList : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.LeaderboardListState = StateManager.LeaderboardListState with
		{
			IsLoading = true,
		};
	}
}
