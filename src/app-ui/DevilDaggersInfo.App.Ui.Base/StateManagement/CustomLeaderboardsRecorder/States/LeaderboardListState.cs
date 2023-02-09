using DevilDaggersInfo.Api.App;
using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

public record LeaderboardListState(
	int MaxPageIndex,
	int TotalResults,
	CustomLeaderboardCategory Category,
	int PageIndex,
	int PageSize,
	bool IsLoading,
	GetCustomLeaderboardForOverview? SelectedCustomLeaderboard,
	Page<GetCustomLeaderboardForOverview>? Page)
{
	public static LeaderboardListState GetDefault()
	{
		return new(0, 0, CustomLeaderboardCategory.Survival, 0, 20, false, null, null);
	}
}
