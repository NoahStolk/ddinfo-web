using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using System.Diagnostics;
using DdLiveApi = DevilDaggersInfo.Web.ApiSpec.DdLive.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.DdLive;

public static class CustomLeaderboardConverters
{
	public static DdLiveApi.GetCustomLeaderboardOverviewDdLive ToDdLiveApi(this CustomLeaderboardOverview customLeaderboard)
	{
		return new DdLiveApi.GetCustomLeaderboardOverviewDdLive
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
			WorldRecord = customLeaderboard.WorldRecord == null ? null : GameTime.FromGameUnits(customLeaderboard.WorldRecord.Time).Seconds,
			WorldRecordDagger = customLeaderboard.WorldRecord?.Dagger?.ToDdLiveApi(),
			RankSorting = customLeaderboard.RankSorting.ToDdLiveApi(),
			SpawnsetGameMode = customLeaderboard.GameMode.ToDdLiveApi(),
		};
	}

	public static DdLiveApi.GetCustomLeaderboardDdLive ToDdLiveApi(this SortedCustomLeaderboard customLeaderboard, List<int> customEntryReplayIds)
	{
		return new DdLiveApi.GetCustomLeaderboardDdLive
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
	}

	private static DdLiveApi.GetCustomLeaderboardDaggersDdLive ToDdLiveApi(this CustomLeaderboardDaggers customLeaderboard)
	{
		return new DdLiveApi.GetCustomLeaderboardDaggersDdLive
		{
			Bronze = GameTime.FromGameUnits(customLeaderboard.Bronze).Seconds,
			Silver = GameTime.FromGameUnits(customLeaderboard.Silver).Seconds,
			Golden = GameTime.FromGameUnits(customLeaderboard.Golden).Seconds,
			Devil = GameTime.FromGameUnits(customLeaderboard.Devil).Seconds,
			Leviathan = GameTime.FromGameUnits(customLeaderboard.Leviathan).Seconds,
		};
	}

	private static DdLiveApi.GetCustomEntryDdLive ToDdLiveApi(this CustomEntry customEntry, bool hasReplay)
	{
		return new DdLiveApi.GetCustomEntryDdLive
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
			LevelUpTime2 = GameTime.FromGameUnits(customEntry.LevelUpTime2).Seconds,
			LevelUpTime3 = GameTime.FromGameUnits(customEntry.LevelUpTime3).Seconds,
			LevelUpTime4 = GameTime.FromGameUnits(customEntry.LevelUpTime4).Seconds,
			DaggersFired = customEntry.DaggersFired,
			DaggersHit = customEntry.DaggersHit,
			SubmitDate = customEntry.SubmitDate,
			Time = GameTime.FromGameUnits(customEntry.Time).Seconds,
			CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger?.ToDdLiveApi(),
			HasGraphs = customEntry.HasGraphs,
			HasReplay = hasReplay,
		};
	}

	private static DdLiveApi.CustomLeaderboardsClientDdLive ToDdLiveApi(this CustomLeaderboardsClient client)
	{
		return client switch
		{
			CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => DdLiveApi.CustomLeaderboardsClientDdLive.DevilDaggersCustomLeaderboards,
			CustomLeaderboardsClient.DdstatsRust => DdLiveApi.CustomLeaderboardsClientDdLive.DdstatsRust,
			CustomLeaderboardsClient.DdinfoTools => DdLiveApi.CustomLeaderboardsClientDdLive.DdinfoTools,
			_ => throw new UnreachableException(),
		};
	}

	private static DdLiveApi.CustomLeaderboardDaggerDdLive ToDdLiveApi(this CustomLeaderboardDagger dagger)
	{
		return dagger switch
		{
			CustomLeaderboardDagger.Default => DdLiveApi.CustomLeaderboardDaggerDdLive.Default,
			CustomLeaderboardDagger.Bronze => DdLiveApi.CustomLeaderboardDaggerDdLive.Bronze,
			CustomLeaderboardDagger.Silver => DdLiveApi.CustomLeaderboardDaggerDdLive.Silver,
			CustomLeaderboardDagger.Golden => DdLiveApi.CustomLeaderboardDaggerDdLive.Golden,
			CustomLeaderboardDagger.Devil => DdLiveApi.CustomLeaderboardDaggerDdLive.Devil,
			CustomLeaderboardDagger.Leviathan => DdLiveApi.CustomLeaderboardDaggerDdLive.Leviathan,
			_ => throw new UnreachableException(),
		};
	}

	private static DdLiveApi.SpawnsetGameModeDdLive ToDdLiveApi(this SpawnsetGameMode dagger)
	{
		return dagger switch
		{
			SpawnsetGameMode.Survival => DdLiveApi.SpawnsetGameModeDdLive.Survival,
			SpawnsetGameMode.TimeAttack => DdLiveApi.SpawnsetGameModeDdLive.TimeAttack,
			SpawnsetGameMode.Race => DdLiveApi.SpawnsetGameModeDdLive.Race,
			_ => throw new UnreachableException(),
		};
	}

	private static DdLiveApi.CustomLeaderboardRankSortingDdLive ToDdLiveApi(this CustomLeaderboardRankSorting dagger)
	{
		return dagger switch
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
	}

	/// <summary>
	/// Workaround to keep the API backwards compatible with the old categories.
	/// </summary>
	[Obsolete("Remove when no longer used.")]
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
