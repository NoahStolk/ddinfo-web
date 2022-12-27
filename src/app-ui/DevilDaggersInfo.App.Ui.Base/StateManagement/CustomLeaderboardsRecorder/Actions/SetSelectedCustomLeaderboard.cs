using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetSelectedCustomLeaderboard(GetCustomLeaderboardForOverview SelectedCustomLeaderboard) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.LeaderboardListState = StateManager.LeaderboardListState with
		{
			SelectedCustomLeaderboard = SelectedCustomLeaderboard,
		};
	}
}
