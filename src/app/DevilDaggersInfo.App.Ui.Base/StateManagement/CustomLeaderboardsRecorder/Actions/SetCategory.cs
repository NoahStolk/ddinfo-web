using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetCategory(CustomLeaderboardCategory Category) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		int totalResults = stateReducer.LeaderboardListState.CustomLeaderboards.Count(cl => cl.Category == Category);
		int newMaxPageIndex = (int)Math.Ceiling((totalResults + 1) / (float)Constants.CustomLeaderboardsPageSize) - 1;

		stateReducer.LeaderboardListState = stateReducer.LeaderboardListState with
		{
			Category = Category,
			PageIndex = Math.Clamp(stateReducer.LeaderboardListState.PageIndex, 0, newMaxPageIndex),
		};
	}
}
