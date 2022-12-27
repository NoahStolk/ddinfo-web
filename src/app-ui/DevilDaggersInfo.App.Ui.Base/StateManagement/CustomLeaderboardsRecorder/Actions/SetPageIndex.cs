namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetPageIndex(int PageIndex) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.LeaderboardListState = StateManager.LeaderboardListState with
		{
			PageIndex = PageIndex,
		};
	}
}
