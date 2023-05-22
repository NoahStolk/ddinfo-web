using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using System.Diagnostics;
using DdLiveApi = DevilDaggersInfo.Api.DdLive.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.DdLive;

public static class CustomLeaderboardConverters
{
	public static DdLiveApi.GetCustomLeaderboardOverviewDdLive ToDdLiveApi(this CustomLeaderboardOverview customLeaderboard) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetName = customLeaderboard.SpawnsetName,
		SpawnsetAuthorId = customLeaderboard.SpawnsetAuthorId,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		Daggers = customLeaderboard.Daggers?.ToDdLiveApi(),
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = customLeaderboard.PlayerCount,
		Category = GetCategory(customLeaderboard.RankSorting, customLeaderboard.GameMode),
		TopPlayerId = customLeaderboard.WorldRecord?.PlayerId,
		TopPlayerName = customLeaderboard.WorldRecord?.PlayerName,
		WorldRecord = customLeaderboard.WorldRecord?.Time.ToSecondsTime(),
		WorldRecordDagger = customLeaderboard.WorldRecord?.Dagger?.ToDdLiveApi(),
		RankSorting = customLeaderboard.RankSorting.ToDdLiveApi(),
		SpawnsetGameMode = customLeaderboard.GameMode.ToDdLiveApi(),
	};

	public static DdLiveApi.GetCustomLeaderboardDdLive ToDdLiveApi(this SortedCustomLeaderboard customLeaderboard, List<int> customEntryReplayIds) => new()
	{
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		SpawnsetHtmlDescription = customLeaderboard.SpawnsetHtmlDescription,
		SpawnsetName = customLeaderboard.SpawnsetName,
		Daggers = customLeaderboard.Daggers?.ToDdLiveApi(),
		DateCreated = customLeaderboard.DateCreated,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		Category = GetCategory(customLeaderboard.RankSorting, customLeaderboard.GameMode),
		IsFeatured = customLeaderboard.Daggers != null,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customLeaderboard.CustomEntries.ConvertAll(ce => ce.ToDdLiveApi(customEntryReplayIds.Contains(ce.Id))),
	};

	private static DdLiveApi.GetCustomLeaderboardDaggersDdLive ToDdLiveApi(this CustomLeaderboardDaggers customLeaderboard) => new()
	{
		Bronze = customLeaderboard.Bronze.ToSecondsTime(),
		Silver = customLeaderboard.Silver.ToSecondsTime(),
		Golden = customLeaderboard.Golden.ToSecondsTime(),
		Devil = customLeaderboard.Devil.ToSecondsTime(),
		Leviathan = customLeaderboard.Leviathan.ToSecondsTime(),
	};

	private static DdLiveApi.GetCustomEntryDdLive ToDdLiveApi(this CustomEntry customEntry, bool hasReplay) => new()
	{
		Id = customEntry.Id,
		Rank = customEntry.Rank,
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
		CountryCode = customEntry.CountryCode,
		Client = customEntry.Client.ToDdLiveApi(),
		ClientVersion = customEntry.ClientVersion,
		DeathType = customEntry.DeathType,
		EnemiesAlive = customEntry.EnemiesAlive,
		GemsCollected = customEntry.GemsCollected,
		GemsDespawned = customEntry.GemsDespawned,
		GemsEaten = customEntry.GemsEaten,
		HomingStored = customEntry.HomingStored,
		HomingEaten = customEntry.HomingEaten,
		EnemiesKilled = customEntry.EnemiesKilled,
		LevelUpTime2 = customEntry.LevelUpTime2.ToSecondsTime(),
		LevelUpTime3 = customEntry.LevelUpTime3.ToSecondsTime(),
		LevelUpTime4 = customEntry.LevelUpTime4.ToSecondsTime(),
		DaggersFired = customEntry.DaggersFired,
		DaggersHit = customEntry.DaggersHit,
		SubmitDate = customEntry.SubmitDate,
		Time = customEntry.Time.ToSecondsTime(),
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger?.ToDdLiveApi(),
		HasGraphs = customEntry.HasGraphs,
		HasReplay = hasReplay,
	};

	private static DdLiveApi.CustomLeaderboardsClientDdLive ToDdLiveApi(this CustomLeaderboardsClient client) => client switch
	{
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => DdLiveApi.CustomLeaderboardsClientDdLive.DevilDaggersCustomLeaderboards,
		CustomLeaderboardsClient.DdstatsRust => DdLiveApi.CustomLeaderboardsClientDdLive.DdstatsRust,
		CustomLeaderboardsClient.DdinfoTools => DdLiveApi.CustomLeaderboardsClientDdLive.DdinfoTools,
		_ => throw new UnreachableException(),
	};

	private static DdLiveApi.CustomLeaderboardDaggerDdLive ToDdLiveApi(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Default => DdLiveApi.CustomLeaderboardDaggerDdLive.Default,
		CustomLeaderboardDagger.Bronze => DdLiveApi.CustomLeaderboardDaggerDdLive.Bronze,
		CustomLeaderboardDagger.Silver => DdLiveApi.CustomLeaderboardDaggerDdLive.Silver,
		CustomLeaderboardDagger.Golden => DdLiveApi.CustomLeaderboardDaggerDdLive.Golden,
		CustomLeaderboardDagger.Devil => DdLiveApi.CustomLeaderboardDaggerDdLive.Devil,
		CustomLeaderboardDagger.Leviathan => DdLiveApi.CustomLeaderboardDaggerDdLive.Leviathan,
		_ => throw new UnreachableException(),
	};

	private static DdLiveApi.SpawnsetGameModeDdLive ToDdLiveApi(this SpawnsetGameMode dagger) => dagger switch
	{
		SpawnsetGameMode.Survival => DdLiveApi.SpawnsetGameModeDdLive.Survival,
		SpawnsetGameMode.TimeAttack => DdLiveApi.SpawnsetGameModeDdLive.TimeAttack,
		SpawnsetGameMode.Race => DdLiveApi.SpawnsetGameModeDdLive.Race,
		_ => throw new UnreachableException(),
	};

	private static DdLiveApi.CustomLeaderboardRankSortingDdLive ToDdLiveApi(this CustomLeaderboardRankSorting dagger) => dagger switch
	{
		CustomLeaderboardRankSorting.TimeAsc => DdLiveApi.CustomLeaderboardRankSortingDdLive.TimeAsc,
		CustomLeaderboardRankSorting.GemsCollectedAsc => DdLiveApi.CustomLeaderboardRankSortingDdLive.GemsCollectedAsc,
		CustomLeaderboardRankSorting.GemsDespawnedAsc => DdLiveApi.CustomLeaderboardRankSortingDdLive.GemsDespawnedAsc,
		CustomLeaderboardRankSorting.GemsEatenAsc => DdLiveApi.CustomLeaderboardRankSortingDdLive.GemsEatenAsc,
		CustomLeaderboardRankSorting.EnemiesKilledAsc => DdLiveApi.CustomLeaderboardRankSortingDdLive.EnemiesKilledAsc,
		CustomLeaderboardRankSorting.EnemiesAliveAsc => DdLiveApi.CustomLeaderboardRankSortingDdLive.EnemiesAliveAsc,
		CustomLeaderboardRankSorting.HomingStoredAsc => DdLiveApi.CustomLeaderboardRankSortingDdLive.HomingStoredAsc,
		CustomLeaderboardRankSorting.HomingEatenAsc => DdLiveApi.CustomLeaderboardRankSortingDdLive.HomingEatenAsc,

		CustomLeaderboardRankSorting.TimeDesc => DdLiveApi.CustomLeaderboardRankSortingDdLive.TimeDesc,
		CustomLeaderboardRankSorting.GemsCollectedDesc => DdLiveApi.CustomLeaderboardRankSortingDdLive.GemsCollectedDesc,
		CustomLeaderboardRankSorting.GemsDespawnedDesc => DdLiveApi.CustomLeaderboardRankSortingDdLive.GemsDespawnedDesc,
		CustomLeaderboardRankSorting.GemsEatenDesc => DdLiveApi.CustomLeaderboardRankSortingDdLive.GemsEatenDesc,
		CustomLeaderboardRankSorting.EnemiesKilledDesc => DdLiveApi.CustomLeaderboardRankSortingDdLive.EnemiesKilledDesc,
		CustomLeaderboardRankSorting.EnemiesAliveDesc => DdLiveApi.CustomLeaderboardRankSortingDdLive.EnemiesAliveDesc,
		CustomLeaderboardRankSorting.HomingStoredDesc => DdLiveApi.CustomLeaderboardRankSortingDdLive.HomingStoredDesc,
		CustomLeaderboardRankSorting.HomingEatenDesc => DdLiveApi.CustomLeaderboardRankSortingDdLive.HomingEatenDesc,

		_ => throw new UnreachableException(),
	};

	[Obsolete("Remove when no longer used.")]
	/// <summary>
	/// Workaround to keep the API backwards compatible with the old categories.
	/// </summary>
	private static DdLiveApi.CustomLeaderboardCategoryDdLive GetCategory(CustomLeaderboardRankSorting rankSorting, SpawnsetGameMode gameMode)
	{
		if (rankSorting == CustomLeaderboardRankSorting.TimeAsc)
		{
			return gameMode switch
			{
				SpawnsetGameMode.Survival => DdLiveApi.CustomLeaderboardCategoryDdLive.Speedrun,
				SpawnsetGameMode.TimeAttack => DdLiveApi.CustomLeaderboardCategoryDdLive.TimeAttack,
				SpawnsetGameMode.Race => DdLiveApi.CustomLeaderboardCategoryDdLive.Race,
				_ => throw new UnreachableException(),
			};
		}

		return DdLiveApi.CustomLeaderboardCategoryDdLive.Survival;
	}
}
