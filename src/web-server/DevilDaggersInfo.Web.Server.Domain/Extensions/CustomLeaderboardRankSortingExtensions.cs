using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomLeaderboardRankSortingExtensions
{
	public static bool IsAscending(this CustomLeaderboardRankSorting rankSorting)
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc => true,
			CustomLeaderboardRankSorting.TimeDesc or CustomLeaderboardRankSorting.GemsCollectedDesc or CustomLeaderboardRankSorting.EnemiesKilledDesc or CustomLeaderboardRankSorting.HomingStoredDesc => false,
			_ => throw new InvalidOperationException($"Rank sorting '{rankSorting}' is not supported."),
		};
	}

	public static bool IsTime(this CustomLeaderboardRankSorting rankSorting)
	{
		return rankSorting is CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc;
	}
}
