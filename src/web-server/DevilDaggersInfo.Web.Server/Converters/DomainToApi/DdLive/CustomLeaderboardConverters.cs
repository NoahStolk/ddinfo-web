using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Types.Web;
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
		Category = customLeaderboard.Category,
		TopPlayerId = customLeaderboard.WorldRecord?.PlayerId,
		TopPlayerName = customLeaderboard.WorldRecord?.PlayerName,
		WorldRecord = customLeaderboard.WorldRecord?.Time.ToSecondsTime(),
		WorldRecordDagger = customLeaderboard.WorldRecord?.Dagger.ToDdLiveApi(),
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
		Category = customLeaderboard.Category,
		IsFeatured = customLeaderboard.Daggers != null,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customLeaderboard.CustomEntries.ConvertAll(ce => ce.ToGetCustomEntryDdLive(customEntryReplayIds.Contains(ce.Id))),
	};

	private static DdLiveApi.GetCustomLeaderboardDaggersDdLive? ToGetCustomLeaderboardDaggers(this CustomLeaderboardDaggers customLeaderboard) => new()
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
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger.ToDdLiveApi(),
		HasGraphs = customEntry.HasGraphs,
		HasReplay = hasReplay,
	};

	private static DdLiveApi.CustomLeaderboardDagger? ToDdLiveApi(this CustomLeaderboardDagger? customLeaderboardDagger) => customLeaderboardDagger switch
	{
		CustomLeaderboardDagger.Default => DdLiveApi.CustomLeaderboardDagger.Default,
		CustomLeaderboardDagger.Bronze => DdLiveApi.CustomLeaderboardDagger.Bronze,
		CustomLeaderboardDagger.Silver => DdLiveApi.CustomLeaderboardDagger.Silver,
		CustomLeaderboardDagger.Golden => DdLiveApi.CustomLeaderboardDagger.Golden,
		CustomLeaderboardDagger.Devil => DdLiveApi.CustomLeaderboardDagger.Devil,
		CustomLeaderboardDagger.Leviathan => DdLiveApi.CustomLeaderboardDagger.Leviathan,
		null => null,
		_ => throw new InvalidEnumConversionException(customLeaderboardDagger),
	};

	private static DdLiveApi.CustomLeaderboardsClient ToDdLiveApi(this CustomLeaderboardsClient customLeaderboardsClient) => customLeaderboardsClient switch
	{
		CustomLeaderboardsClient.DdstatsRust => DdLiveApi.CustomLeaderboardsClient.DdstatsRust,
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => DdLiveApi.CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		_ => throw new InvalidEnumConversionException(customLeaderboardsClient),
	};
}
