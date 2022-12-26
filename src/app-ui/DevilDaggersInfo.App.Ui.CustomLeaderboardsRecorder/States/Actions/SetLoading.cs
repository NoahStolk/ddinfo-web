using DevilDaggersInfo.App.Ui.Base.States.Actions;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetLoading(bool IsLoading) : IAction<SetLoading>
{
	public void Reduce()
	{
		StateManager.LeaderboardListState = StateManager.LeaderboardListState with
		{
			IsLoading = IsLoading,
		};
	}
}
