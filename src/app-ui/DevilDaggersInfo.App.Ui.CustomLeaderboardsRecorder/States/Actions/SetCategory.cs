using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record SetCategory(CustomLeaderboardCategory Category) : IAction<SetCategory>
{
	public void Reduce()
	{
		StateManager.LeaderboardListState = StateManager.LeaderboardListState with
		{
			Category = Category,
		};
	}
}
