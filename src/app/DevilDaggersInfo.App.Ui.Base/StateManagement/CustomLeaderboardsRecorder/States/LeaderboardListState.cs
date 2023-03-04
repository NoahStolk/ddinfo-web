using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

// TODO: Add sorting + ascending/descending.
public record LeaderboardListState(
	CustomLeaderboardCategory Category,
	int PageIndex,
	bool IsLoading,
	string SpawnsetName,
	string AuthorName,
	bool FeaturedOnly,
	LeaderboardListState.CustomLeaderboard? SelectedCustomLeaderboard,
	List<GetCustomLeaderboardForOverview> CustomLeaderboards)
{
	public static LeaderboardListState GetDefault()
	{
		return new(CustomLeaderboardCategory.Survival, 0, false, string.Empty, string.Empty, false, null, new());
	}

	public int GetTotal()
	{
		return CustomLeaderboards.Count(Predicate);
	}

	public int GetTotalPages()
	{
		return (int)Math.Ceiling(GetTotal() / (float)Constants.CustomLeaderboardsPageSize);
	}

	public int GetMaxPageIndex()
	{
		return Math.Max(0, GetTotalPages() - 1);
	}

	public List<GetCustomLeaderboardForOverview> GetPagedCustomLeaderboards()
	{
		return CustomLeaderboards
			.Where(Predicate)
			.Skip(PageIndex * Constants.CustomLeaderboardsPageSize)
			.Take(Constants.CustomLeaderboardsPageSize)
			.ToList();
	}

	private bool Predicate(GetCustomLeaderboardForOverview cl)
	{
		return
			cl.Category == Category &&
			(string.IsNullOrEmpty(SpawnsetName) || cl.SpawnsetName.Contains(SpawnsetName, StringComparison.OrdinalIgnoreCase)) &&
			(string.IsNullOrEmpty(AuthorName) || cl.SpawnsetAuthorName.Contains(AuthorName, StringComparison.OrdinalIgnoreCase)) &&
			(!FeaturedOnly || cl.Daggers != null);
	}

	public record CustomLeaderboard(int Id, int SpawnsetId, string SpawnsetName);
}
