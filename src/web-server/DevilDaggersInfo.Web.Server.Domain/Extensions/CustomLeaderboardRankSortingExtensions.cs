using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomLeaderboardRankSortingExtensions
{
	public static bool IsAscending(this CustomLeaderboardRankSorting rankSorting)
	{
		return rankSorting switch
		{
			CustomLeaderboardRankSorting.TimeAsc => true,
			CustomLeaderboardRankSorting.TimeDesc => false,
			_ => throw new InvalidOperationException($"Rank sorting '{rankSorting}' is not supported."),
		};
	}
}
