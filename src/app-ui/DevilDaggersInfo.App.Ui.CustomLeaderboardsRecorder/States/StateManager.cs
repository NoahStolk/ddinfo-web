using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public static class StateManager
{
	public static LeaderboardListState LeaderboardListState { get; private set; } = LeaderboardListState.GetDefault();
	public static MarkerState MarkerState { get; private set; } = MarkerState.GetDefault();

	public static void SetPageIndex(int pageIndex)
	{
		LeaderboardListState = LeaderboardListState with
		{
			PageIndex = pageIndex,
		};
	}

	public static void SetCategory(CustomLeaderboardCategory category)
	{
		LeaderboardListState = LeaderboardListState with
		{
			Category = category,
		};
	}

	public static void SetLoading(bool isLoading)
	{
		LeaderboardListState = LeaderboardListState with
		{
			IsLoading = isLoading,
		};
	}

	public static void SetTotalResults(int totalResults)
	{
		int newMaxPageIndex = (int)Math.Ceiling((totalResults + 1) / (float)LeaderboardListState.PageSize) - 1;
		LeaderboardListState = LeaderboardListState with
		{
			MaxPageIndex = newMaxPageIndex,
			TotalResults = totalResults,
			PageIndex = Math.Clamp(LeaderboardListState.PageIndex, 0, newMaxPageIndex),
		};
	}

	public static void SetSelectedCustomLeaderboard(GetCustomLeaderboardForOverview cl)
	{
		LeaderboardListState = LeaderboardListState with
		{
			SelectedCustomLeaderboard = cl,
		};
	}

	public static void SetMarker(long marker)
	{
		MarkerState = new(marker);
	}
}
