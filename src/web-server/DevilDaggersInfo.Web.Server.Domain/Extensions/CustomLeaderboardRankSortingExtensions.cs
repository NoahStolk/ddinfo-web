using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomLeaderboardRankSortingExtensions
{
	public static bool IsAscending(this CustomLeaderboardRankSorting rankSorting)
		=> rankSorting is CustomLeaderboardRankSorting.TimeAsc;
}
