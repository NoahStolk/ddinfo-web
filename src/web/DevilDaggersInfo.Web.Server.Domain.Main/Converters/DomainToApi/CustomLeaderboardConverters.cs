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
		CustomLeaderboardRankSorting.GemsCollectedAsc => MainApi.CustomLeaderboardRankSorting.GemsCollectedAsc,
		CustomLeaderboardRankSorting.GemsDespawnedDesc => MainApi.CustomLeaderboardRankSorting.GemsDespawnedDesc,
		CustomLeaderboardRankSorting.GemsDespawnedAsc => MainApi.CustomLeaderboardRankSorting.GemsDespawnedAsc,
		CustomLeaderboardRankSorting.GemsEatenDesc => MainApi.CustomLeaderboardRankSorting.GemsEatenDesc,
		CustomLeaderboardRankSorting.GemsEatenAsc => MainApi.CustomLeaderboardRankSorting.GemsEatenAsc,
		CustomLeaderboardRankSorting.EnemiesKilledDesc => MainApi.CustomLeaderboardRankSorting.EnemiesKilledDesc,
		CustomLeaderboardRankSorting.EnemiesKilledAsc => MainApi.CustomLeaderboardRankSorting.EnemiesKilledAsc,
		CustomLeaderboardRankSorting.EnemiesAliveDesc => MainApi.CustomLeaderboardRankSorting.EnemiesAliveDesc,
		CustomLeaderboardRankSorting.EnemiesAliveAsc => MainApi.CustomLeaderboardRankSorting.EnemiesAliveAsc,
		CustomLeaderboardRankSorting.HomingStoredDesc => MainApi.CustomLeaderboardRankSorting.HomingStoredDesc,
		CustomLeaderboardRankSorting.HomingStoredAsc => MainApi.CustomLeaderboardRankSorting.HomingStoredAsc,
		CustomLeaderboardRankSorting.HomingEatenDesc => MainApi.CustomLeaderboardRankSorting.HomingEatenDesc,
		CustomLeaderboardRankSorting.HomingEatenAsc => MainApi.CustomLeaderboardRankSorting.HomingEatenAsc,
		_ => throw new UnreachableException(),
	};
}
