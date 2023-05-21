using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;

public static class CustomLeaderboardConverters
{
	public static MainApi.CustomLeaderboardRankSorting ToMainApi(this CustomLeaderboardRankSorting rankSorting) => rankSorting switch
	{
		CustomLeaderboardRankSorting.TimeDesc => MainApi.CustomLeaderboardRankSorting.TimeDesc,
		CustomLeaderboardRankSorting.TimeAsc => MainApi.CustomLeaderboardRankSorting.TimeAsc,
		CustomLeaderboardRankSorting.GemsCollectedDesc => MainApi.CustomLeaderboardRankSorting.GemsCollectedDesc,
		CustomLeaderboardRankSorting.EnemiesKilledDesc => MainApi.CustomLeaderboardRankSorting.EnemiesKilledDesc,
		CustomLeaderboardRankSorting.HomingStoredDesc => MainApi.CustomLeaderboardRankSorting.HomingStoredDesc,
		_ => throw new UnreachableException(),
	};
}
