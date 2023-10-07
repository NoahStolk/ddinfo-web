using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Web.ApiSpec.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;

public static class CustomLeaderboardConverters
{
	public static CustomLeaderboardSorting ToDomain(this MainApi.CustomLeaderboardSorting customLeaderboardSorting) => customLeaderboardSorting switch
	{
		MainApi.CustomLeaderboardSorting.SpawnsetName => CustomLeaderboardSorting.SpawnsetName,
		MainApi.CustomLeaderboardSorting.AuthorName => CustomLeaderboardSorting.AuthorName,
		MainApi.CustomLeaderboardSorting.TimeBronze => CustomLeaderboardSorting.TimeBronze,
		MainApi.CustomLeaderboardSorting.TimeSilver => CustomLeaderboardSorting.TimeSilver,
		MainApi.CustomLeaderboardSorting.TimeGolden => CustomLeaderboardSorting.TimeGolden,
		MainApi.CustomLeaderboardSorting.TimeDevil => CustomLeaderboardSorting.TimeDevil,
		MainApi.CustomLeaderboardSorting.TimeLeviathan => CustomLeaderboardSorting.TimeLeviathan,
		MainApi.CustomLeaderboardSorting.DateCreated => CustomLeaderboardSorting.DateCreated,
		MainApi.CustomLeaderboardSorting.DateLastPlayed => CustomLeaderboardSorting.DateLastPlayed,
		MainApi.CustomLeaderboardSorting.Submits => CustomLeaderboardSorting.Submits,
		MainApi.CustomLeaderboardSorting.Players => CustomLeaderboardSorting.Players,
		MainApi.CustomLeaderboardSorting.TopPlayer => CustomLeaderboardSorting.TopPlayer,
		MainApi.CustomLeaderboardSorting.WorldRecord => CustomLeaderboardSorting.WorldRecord,
		_ => throw new UnreachableException(),
	};

	public static CustomLeaderboardRankSorting ToDomain(this MainApi.CustomLeaderboardRankSorting rankSorting) => rankSorting switch
	{
		MainApi.CustomLeaderboardRankSorting.TimeAsc => CustomLeaderboardRankSorting.TimeAsc,
		MainApi.CustomLeaderboardRankSorting.GemsCollectedAsc => CustomLeaderboardRankSorting.GemsCollectedAsc,
		MainApi.CustomLeaderboardRankSorting.GemsDespawnedAsc => CustomLeaderboardRankSorting.GemsDespawnedAsc,
		MainApi.CustomLeaderboardRankSorting.GemsEatenAsc => CustomLeaderboardRankSorting.GemsEatenAsc,
		MainApi.CustomLeaderboardRankSorting.EnemiesKilledAsc => CustomLeaderboardRankSorting.EnemiesKilledAsc,
		MainApi.CustomLeaderboardRankSorting.EnemiesAliveAsc => CustomLeaderboardRankSorting.EnemiesAliveAsc,
		MainApi.CustomLeaderboardRankSorting.HomingStoredAsc => CustomLeaderboardRankSorting.HomingStoredAsc,
		MainApi.CustomLeaderboardRankSorting.HomingEatenAsc => CustomLeaderboardRankSorting.HomingEatenAsc,

		MainApi.CustomLeaderboardRankSorting.TimeDesc => CustomLeaderboardRankSorting.TimeDesc,
		MainApi.CustomLeaderboardRankSorting.GemsCollectedDesc => CustomLeaderboardRankSorting.GemsCollectedDesc,
		MainApi.CustomLeaderboardRankSorting.GemsDespawnedDesc => CustomLeaderboardRankSorting.GemsDespawnedDesc,
		MainApi.CustomLeaderboardRankSorting.GemsEatenDesc => CustomLeaderboardRankSorting.GemsEatenDesc,
		MainApi.CustomLeaderboardRankSorting.EnemiesKilledDesc => CustomLeaderboardRankSorting.EnemiesKilledDesc,
		MainApi.CustomLeaderboardRankSorting.EnemiesAliveDesc => CustomLeaderboardRankSorting.EnemiesAliveDesc,
		MainApi.CustomLeaderboardRankSorting.HomingStoredDesc => CustomLeaderboardRankSorting.HomingStoredDesc,
		MainApi.CustomLeaderboardRankSorting.HomingEatenDesc => CustomLeaderboardRankSorting.HomingEatenDesc,

		_ => throw new UnreachableException(),
	};
}
