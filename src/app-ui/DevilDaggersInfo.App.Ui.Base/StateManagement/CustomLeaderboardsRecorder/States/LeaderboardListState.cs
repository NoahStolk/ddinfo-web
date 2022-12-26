using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

public record LeaderboardListState(int MaxPageIndex, int TotalResults, CustomLeaderboardCategory Category, int PageIndex, int PageSize, bool IsLoading, GetCustomLeaderboardForOverview? SelectedCustomLeaderboard)
{
	public static LeaderboardListState GetDefault()
	{
		return new(int.MaxValue, int.MaxValue, CustomLeaderboardCategory.Survival, 0, 20, false, null);
	}
}
