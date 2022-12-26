using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

public record LeaderboardState(GetCustomLeaderboard? CustomLeaderboard)
{
	public static LeaderboardState GetDefault()
	{
		return new((GetCustomLeaderboard?)null);
	}
}
