using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class CustomLeaderboardRankSortingExtensions
{
	extension(CustomLeaderboardRankSorting rankSorting)
	{
		public bool IsAscending()
		{
			return rankSorting switch
			{
				CustomLeaderboardRankSorting.TimeAsc or
					CustomLeaderboardRankSorting.GemsCollectedAsc or
					CustomLeaderboardRankSorting.GemsDespawnedAsc or
					CustomLeaderboardRankSorting.GemsEatenAsc or
					CustomLeaderboardRankSorting.EnemiesKilledAsc or
					CustomLeaderboardRankSorting.EnemiesAliveAsc or
					CustomLeaderboardRankSorting.HomingStoredAsc or
					CustomLeaderboardRankSorting.HomingEatenAsc
					=> true,

				CustomLeaderboardRankSorting.TimeDesc or
					CustomLeaderboardRankSorting.GemsCollectedDesc or
					CustomLeaderboardRankSorting.GemsDespawnedDesc or
					CustomLeaderboardRankSorting.GemsEatenDesc or
					CustomLeaderboardRankSorting.EnemiesKilledDesc or
					CustomLeaderboardRankSorting.EnemiesAliveDesc or
					CustomLeaderboardRankSorting.HomingStoredDesc or
					CustomLeaderboardRankSorting.HomingEatenDesc
					=> false,

				_ => throw new InvalidOperationException($"Rank sorting '{rankSorting}' is not supported."),
			};
		}

		public bool IsTime()
		{
			return rankSorting is CustomLeaderboardRankSorting.TimeAsc or CustomLeaderboardRankSorting.TimeDesc;
		}
	}
}
