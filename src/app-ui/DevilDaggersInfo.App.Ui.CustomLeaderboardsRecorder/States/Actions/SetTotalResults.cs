namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetTotalResults(int TotalResults) : IAction
{
	public void Reduce()
	{
		int newMaxPageIndex = (int)Math.Ceiling((TotalResults + 1) / (float)StateManager.LeaderboardListState.PageSize) - 1;
		StateManager.LeaderboardListState = StateManager.LeaderboardListState with
		{
			MaxPageIndex = newMaxPageIndex,
			TotalResults = TotalResults,
			PageIndex = Math.Clamp(StateManager.LeaderboardListState.PageIndex, 0, newMaxPageIndex),
		};
	}
}
