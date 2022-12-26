using DevilDaggersInfo.App.Ui.Base.States.Actions;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

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
