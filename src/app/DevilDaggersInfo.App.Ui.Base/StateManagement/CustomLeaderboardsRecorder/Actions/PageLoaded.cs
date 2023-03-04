using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

/// <summary>
/// Fires when the custom leaderboard page has loaded.
/// </summary>
public record PageLoaded(List<GetCustomLeaderboardForOverview>? CustomLeaderboards) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		if (CustomLeaderboards == null)
		{
			stateReducer.LeaderboardListState = stateReducer.LeaderboardListState with
			{
				IsLoading = false,
				CustomLeaderboards = new(),
				PageIndex = 0,
			};
			return;
		}

		int totalResults = CustomLeaderboards.Count(cl => cl.Category == stateReducer.LeaderboardListState.Category);
		int newMaxPageIndex = (int)Math.Ceiling((totalResults + 1) / (float)Constants.CustomLeaderboardsPageSize) - 1;

		stateReducer.LeaderboardListState = stateReducer.LeaderboardListState with
		{
			IsLoading = false,
			CustomLeaderboards = CustomLeaderboards,
			PageIndex = Math.Clamp(stateReducer.LeaderboardListState.PageIndex, 0, newMaxPageIndex),
		};
	}
}
