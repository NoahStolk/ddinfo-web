using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomLeaderboardRankSortingExtensions
{
	public static bool IsAscending(this CustomLeaderboardRankSorting rankSorting)
	{
		return rankSorting is CustomLeaderboardRankSorting.TimeAsc;
	}

	public static bool IsTime(this CustomLeaderboardRankSorting rankSorting)
	{
		return rankSorting is CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc;
	}
}
