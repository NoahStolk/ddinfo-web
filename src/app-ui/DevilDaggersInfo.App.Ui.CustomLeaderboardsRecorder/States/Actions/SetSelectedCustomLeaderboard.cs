using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetSelectedCustomLeaderboard(GetCustomLeaderboardForOverview SelectedCustomLeaderboard) : IAction<SetSelectedCustomLeaderboard>
{
	public void Reduce()
	{
		StateManager.LeaderboardListState = StateManager.LeaderboardListState with
		{
			SelectedCustomLeaderboard = SelectedCustomLeaderboard,
		};
	}
}
