using DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public record LeaderboardState(GetCustomLeaderboard? CustomLeaderboard)
{
	public static LeaderboardState GetDefault()
	{
		return new((GetCustomLeaderboard?)null);
	}
}
