using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DdLiveApi = DevilDaggersInfo.Api.DdLive.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.DdLive;

public static class CustomLeaderboardConverters
{
	public static DdLiveApi.GetCustomLeaderboardOverviewDdLive ToGetCustomLeaderboardOverviewDdLive(this CustomLeaderboardOverview customLeaderboard) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetName = customLeaderboard.SpawnsetName,
		SpawnsetAuthorId = customLeaderboard.SpawnsetAuthorId,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		Daggers = customLeaderboard.Daggers?.ToGetCustomLeaderboardDaggers(),
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = customLeaderboard.PlayerCount,
		Category = customLeaderboard.Category.ToCustomLeaderboardCategoryDdLive(),
		TopPlayerId = customLeaderboard.WorldRecord?.PlayerId,
		TopPlayerName = customLeaderboard.WorldRecord?.PlayerName,
		WorldRecord = customLeaderboard.WorldRecord?.Time.ToSecondsTime(),
		WorldRecordDagger = customLeaderboard.WorldRecord?.Dagger?.ToCustomLeaderboardDaggerDdLive(),
	};

	public static DdLiveApi.GetCustomLeaderboardDdLive ToGetCustomLeaderboardDdLive(this SortedCustomLeaderboard customLeaderboard, List<int> customEntryReplayIds) => new()
	{
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		SpawnsetHtmlDescription = customLeaderboard.SpawnsetHtmlDescription,
		SpawnsetName = customLeaderboard.SpawnsetName,
		Daggers = customLeaderboard.Daggers?.ToGetCustomLeaderboardDaggers(),
		DateCreated = customLeaderboard.DateCreated,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		Category = customLeaderboard.Category.ToCustomLeaderboardCategoryDdLive(),
		IsFeatured = customLeaderboard.Daggers != null,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customLeaderboard.CustomEntries.ConvertAll(ce => ce.ToGetCustomEntryDdLive(customEntryReplayIds.Contains(ce.Id))),
	};

	private static DdLiveApi.GetCustomLeaderboardDaggersDdLive ToGetCustomLeaderboardDaggers(this CustomLeaderboardDaggers customLeaderboard) => new()
	{
		Bronze = customLeaderboard.Bronze.ToSecondsTime(),
		Silver = customLeaderboard.Silver.ToSecondsTime(),
		Golden = customLeaderboard.Golden.ToSecondsTime(),
		Devil = customLeaderboard.Devil.ToSecondsTime(),
		Leviathan = customLeaderboard.Leviathan.ToSecondsTime(),
	};

	private static DdLiveApi.GetCustomEntryDdLive ToGetCustomEntryDdLive(this CustomEntry customEntry, bool hasReplay) => new()
	{
		Id = customEntry.Id,
		Rank = customEntry.Rank,
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
		CountryCode = customEntry.CountryCode,
		Client = customEntry.Client.ToCustomLeaderboardsClientDdLive(),
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
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger?.ToCustomLeaderboardDaggerDdLive(),
		HasGraphs = customEntry.HasGraphs,
		HasReplay = hasReplay,
	};

	private static DdLiveApi.CustomLeaderboardCategoryDdLive ToCustomLeaderboardCategoryDdLive(this CustomLeaderboardCategory category) => category switch
	{
		CustomLeaderboardCategory.Survival => DdLiveApi.CustomLeaderboardCategoryDdLive.Survival,
		CustomLeaderboardCategory.TimeAttack => DdLiveApi.CustomLeaderboardCategoryDdLive.TimeAttack,
		CustomLeaderboardCategory.Speedrun => DdLiveApi.CustomLeaderboardCategoryDdLive.Speedrun,
		CustomLeaderboardCategory.Race => DdLiveApi.CustomLeaderboardCategoryDdLive.Race,
		_ => throw new ArgumentOutOfRangeException(nameof(category), category, null),
	};

	private static DdLiveApi.CustomLeaderboardsClientDdLive ToCustomLeaderboardsClientDdLive(this CustomLeaderboardsClient client) => client switch
	{
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => DdLiveApi.CustomLeaderboardsClientDdLive.DevilDaggersCustomLeaderboards,
		CustomLeaderboardsClient.DdstatsRust => DdLiveApi.CustomLeaderboardsClientDdLive.DdstatsRust,
		CustomLeaderboardsClient.DdinfoTools => DdLiveApi.CustomLeaderboardsClientDdLive.DdinfoTools,
		_ => throw new ArgumentOutOfRangeException(nameof(client), client, null),
	};

	private static DdLiveApi.CustomLeaderboardDaggerDdLive ToCustomLeaderboardDaggerDdLive(this CustomLeaderboardDagger dagger) => dagger switch
	{
		CustomLeaderboardDagger.Default => DdLiveApi.CustomLeaderboardDaggerDdLive.Default,
		CustomLeaderboardDagger.Bronze => DdLiveApi.CustomLeaderboardDaggerDdLive.Bronze,
		CustomLeaderboardDagger.Silver => DdLiveApi.CustomLeaderboardDaggerDdLive.Silver,
		CustomLeaderboardDagger.Golden => DdLiveApi.CustomLeaderboardDaggerDdLive.Golden,
		CustomLeaderboardDagger.Devil => DdLiveApi.CustomLeaderboardDaggerDdLive.Devil,
		CustomLeaderboardDagger.Leviathan => DdLiveApi.CustomLeaderboardDaggerDdLive.Leviathan,
		_ => throw new ArgumentOutOfRangeException(nameof(dagger), dagger, null),
	};
}
