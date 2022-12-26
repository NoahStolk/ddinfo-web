namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetPageIndex(int PageIndex) : IAction<SetPageIndex>
{
	public void Reduce()
	{
		StateManager.LeaderboardListState = StateManager.LeaderboardListState with
		{
			PageIndex = PageIndex,
		};
	}
}
