namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetTotalResults(int TotalResults) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		int newMaxPageIndex = (int)Math.Ceiling((TotalResults + 1) / (float)stateReducer.LeaderboardListState.PageSize) - 1;
		stateReducer.LeaderboardListState = stateReducer.LeaderboardListState with
		{
			MaxPageIndex = newMaxPageIndex,
			TotalResults = TotalResults,
			PageIndex = Math.Clamp(stateReducer.LeaderboardListState.PageIndex, 0, newMaxPageIndex),
		};
	}
}
