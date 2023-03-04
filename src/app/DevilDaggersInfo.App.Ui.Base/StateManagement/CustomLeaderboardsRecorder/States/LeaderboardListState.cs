using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

// TODO: Add sorting, filtering, and ascending/descending.
public record LeaderboardListState(
	CustomLeaderboardCategory Category,
	int PageIndex,
	List<GetCustomLeaderboardForOverview> PagedCustomLeaderboards,
	bool IsLoading,
	LeaderboardListState.CustomLeaderboard? SelectedCustomLeaderboard,
	List<GetCustomLeaderboardForOverview> CustomLeaderboards)
{
	public static LeaderboardListState GetDefault()
	{
		return new(CustomLeaderboardCategory.Survival, 0, new(), false, null, new());
	}

	public record CustomLeaderboard(int Id, int SpawnsetId, string SpawnsetName);

	public int GetTotal()
	{
		return CustomLeaderboards.Count(cl => cl.Category == Category);
	}

	public int GetTotalPages()
	{
		return (int)Math.Ceiling(GetTotal() / (float)Constants.CustomLeaderboardsPageSize);
	}
}
