using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

// TODO: Add sorting, filtering, and ascending/descending.
public record LeaderboardListState(
	CustomLeaderboardCategory Category,
	int PageIndex,
	bool IsLoading,
	LeaderboardListState.CustomLeaderboard? SelectedCustomLeaderboard,
	List<GetCustomLeaderboardForOverview> CustomLeaderboards)
{
	public static LeaderboardListState GetDefault()
	{
		return new(CustomLeaderboardCategory.Survival, 0, false, null, new());
	}

	public int GetTotal()
	{
		return CustomLeaderboards.Count(cl => cl.Category == Category);
	}

	public int GetTotalPages()
	{
		return (int)Math.Ceiling(GetTotal() / (float)Constants.CustomLeaderboardsPageSize);
	}

	public List<GetCustomLeaderboardForOverview> GetPagedCustomLeaderboards()
	{
		return CustomLeaderboards
			.Where(cl => cl.Category == Category)
			.Skip(PageIndex * Constants.CustomLeaderboardsPageSize)
			.Take(Constants.CustomLeaderboardsPageSize)
			.ToList();
	}

	public record CustomLeaderboard(int Id, int SpawnsetId, string SpawnsetName);
}
