using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetSelectedCustomLeaderboard(GetCustomLeaderboardForOverview SelectedCustomLeaderboard) : IAction
{
	public void Reduce()
	{
		StateManager.LeaderboardListState = StateManager.LeaderboardListState with
		{
			SelectedCustomLeaderboard = SelectedCustomLeaderboard,
		};
	}
}
