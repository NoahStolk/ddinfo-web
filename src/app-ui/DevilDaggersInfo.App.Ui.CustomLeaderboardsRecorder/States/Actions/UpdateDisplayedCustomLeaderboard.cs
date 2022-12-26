using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record UpdateDisplayedCustomLeaderboard(GetCustomLeaderboard GetCustomLeaderboard) : IAction<UpdateDisplayedCustomLeaderboard>
{
	public void Reduce()
	{
		StateManager.LeaderboardState = new(GetCustomLeaderboard);
	}
}
