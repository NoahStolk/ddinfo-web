using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

/// <summary>
/// Fires when the custom leaderboards have loaded.
/// </summary>
// TODO: Rename.
public record PageLoaded(List<GetCustomLeaderboardForOverview>? CustomLeaderboards) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		if (CustomLeaderboards == null)
		{
			stateReducer.LeaderboardListState = stateReducer.LeaderboardListState with
			{
				IsLoading = false,
				CustomLeaderboards = new List<GetCustomLeaderboardForOverview>(),
				PageIndex = 0,
			};
			return;
		}

		stateReducer.LeaderboardListState = stateReducer.LeaderboardListState with
		{
			IsLoading = false,
			CustomLeaderboards = CustomLeaderboards,
		};

		stateReducer.LeaderboardListState = stateReducer.LeaderboardListState with
		{
			PageIndex = Math.Clamp(stateReducer.LeaderboardListState.PageIndex, 0, stateReducer.LeaderboardListState.GetMaxPageIndex()),
		};
	}
}
