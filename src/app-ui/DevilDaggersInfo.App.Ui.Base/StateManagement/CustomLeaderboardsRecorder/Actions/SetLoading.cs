namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetLoading(bool IsLoading) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.LeaderboardListState = stateReducer.LeaderboardListState with
		{
			IsLoading = IsLoading,
		};
	}
}
