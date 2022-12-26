using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Ui.Base.States.Actions;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;

public record UpdateDisplayedCustomLeaderboard(GetCustomLeaderboard GetCustomLeaderboard) : IAction<UpdateDisplayedCustomLeaderboard>
{
	public void Reduce()
	{
		StateManager.LeaderboardState = new(GetCustomLeaderboard);
	}
}
