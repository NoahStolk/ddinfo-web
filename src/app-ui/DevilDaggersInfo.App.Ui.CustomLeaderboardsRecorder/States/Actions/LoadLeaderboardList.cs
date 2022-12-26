namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record LoadLeaderboardList : IAction
{
	public void Reduce()
	{
		StateManager.LeaderboardListState = StateManager.LeaderboardListState with
		{
			IsLoading = true,
		};
	}
}
