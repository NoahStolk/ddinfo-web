using DevilDaggersInfo.App.Ui.Base.States.Actions;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record LoadLeaderboardList : IAction<LoadLeaderboardList>
{
	public void Reduce()
	{
		StateManager.LeaderboardListState = StateManager.LeaderboardListState with
		{
			IsLoading = true,
		};
	}
}
