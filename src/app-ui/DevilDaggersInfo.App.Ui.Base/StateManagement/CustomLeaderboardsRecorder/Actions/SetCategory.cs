using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetCategory(CustomLeaderboardCategory Category) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.LeaderboardListState = StateManager.LeaderboardListState with
		{
			Category = Category,
		};
	}
}
