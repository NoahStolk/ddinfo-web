using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record UpdateDisplayedCustomLeaderboard(GetCustomLeaderboard GetCustomLeaderboard) : IAction
{
	public void Reduce()
	{
		StateManager.LeaderboardState = new(GetCustomLeaderboard);
	}
}
