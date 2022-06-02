using DevilDaggersInfo.Web.Server.Caches.LeaderboardStatistics;
using DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;
using DdLiveApiCustomLeaderboards = DevilDaggersInfo.Api.DdLive.CustomLeaderboards;
using DdLiveApiLeaderboardStatistics = DevilDaggersInfo.Api.DdLive.LeaderboardStatistics;

namespace DevilDaggersInfo.Web.Server.Converters.DdLive;

public static class DomainToApi
{
	public static DdLiveApiCustomLeaderboards.GetCustomLeaderboardOverviewDdLive ToGetCustomLeaderboardOverviewDdLive(this CustomLeaderboardOverview customLeaderboard) => new()
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
		Category = customLeaderboard.Category.ToDdLiveApiCustomLeaderboards(),
		TopPlayerId = customLeaderboard.WorldRecord?.PlayerId,
		TopPlayerName = customLeaderboard.WorldRecord?.PlayerName,
		WorldRecord = customLeaderboard.WorldRecord?.Time.ToSecondsTime(),
		WorldRecordDagger = customLeaderboard.WorldRecord?.Dagger.ToDdLiveApiCustomLeaderboards(),
	};

	public static DdLiveApiCustomLeaderboards.GetCustomLeaderboardDdLive ToGetCustomLeaderboardDdLive(this SortedCustomLeaderboard customLeaderboard, List<int> customEntryReplayIds) => new()
	{
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		SpawnsetHtmlDescription = customLeaderboard.SpawnsetHtmlDescription,
		SpawnsetName = customLeaderboard.SpawnsetName,
		Daggers = customLeaderboard.Daggers?.ToGetCustomLeaderboardDaggers(),
		DateCreated = customLeaderboard.DateCreated,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		Category = customLeaderboard.Category.ToDdLiveApiCustomLeaderboards(),
		IsFeatured = customLeaderboard.Daggers != null,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customLeaderboard.CustomEntries.ConvertAll(ce => ce.ToGetCustomEntryDdLive(customEntryReplayIds.Contains(ce.Id))),
	};

	private static DdLiveApiCustomLeaderboards.GetCustomLeaderboardDaggersDdLive? ToGetCustomLeaderboardDaggers(this CustomLeaderboardDaggers customLeaderboard) => new()
	{
		Bronze = customLeaderboard.Bronze.ToSecondsTime(),
		Silver = customLeaderboard.Silver.ToSecondsTime(),
		Golden = customLeaderboard.Golden.ToSecondsTime(),
		Devil = customLeaderboard.Devil.ToSecondsTime(),
		Leviathan = customLeaderboard.Leviathan.ToSecondsTime(),
	};

	private static DdLiveApiCustomLeaderboards.GetCustomEntryDdLive ToGetCustomEntryDdLive(this CustomEntry customEntry, bool hasReplay) => new()
	{
		Id = customEntry.Id,
		Rank = customEntry.Rank,
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
		CountryCode = customEntry.CountryCode,
		Client = customEntry.Client.ToDdLiveApiCustomLeaderboards(),
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
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger.ToDdLiveApiCustomLeaderboards(),
		HasGraphs = customEntry.HasGraphs,
		HasReplay = hasReplay,
	};

	private static DdLiveApiCustomLeaderboards.CustomLeaderboardCategory ToDdLiveApiCustomLeaderboards(this CustomLeaderboardCategory customLeaderboardCategory) => customLeaderboardCategory switch
	{
		CustomLeaderboardCategory.Survival => DdLiveApiCustomLeaderboards.CustomLeaderboardCategory.Survival,
		CustomLeaderboardCategory.TimeAttack => DdLiveApiCustomLeaderboards.CustomLeaderboardCategory.TimeAttack,
		CustomLeaderboardCategory.Speedrun => DdLiveApiCustomLeaderboards.CustomLeaderboardCategory.Speedrun,
		CustomLeaderboardCategory.Race => DdLiveApiCustomLeaderboards.CustomLeaderboardCategory.Race,
		CustomLeaderboardCategory.Pacifist => DdLiveApiCustomLeaderboards.CustomLeaderboardCategory.Pacifist,
		_ => throw new InvalidOperationException($"Cannot convert custom leaderboard category '{customLeaderboardCategory}' to a DDLIVE API model."),
	};

	private static DdLiveApiCustomLeaderboards.CustomLeaderboardDagger? ToDdLiveApiCustomLeaderboards(this CustomLeaderboardDagger? customLeaderboardDagger) => customLeaderboardDagger switch
	{
		CustomLeaderboardDagger.Default => DdLiveApiCustomLeaderboards.CustomLeaderboardDagger.Default,
		CustomLeaderboardDagger.Bronze => DdLiveApiCustomLeaderboards.CustomLeaderboardDagger.Bronze,
		CustomLeaderboardDagger.Silver => DdLiveApiCustomLeaderboards.CustomLeaderboardDagger.Silver,
		CustomLeaderboardDagger.Golden => DdLiveApiCustomLeaderboards.CustomLeaderboardDagger.Golden,
		CustomLeaderboardDagger.Devil => DdLiveApiCustomLeaderboards.CustomLeaderboardDagger.Devil,
		CustomLeaderboardDagger.Leviathan => DdLiveApiCustomLeaderboards.CustomLeaderboardDagger.Leviathan,
		null => null,
		_ => throw new InvalidOperationException($"Cannot convert custom leaderboard dagger '{customLeaderboardDagger}' to a DDLIVE API model."),
	};

	private static DdLiveApiCustomLeaderboards.CustomLeaderboardsClient ToDdLiveApiCustomLeaderboards(this CustomLeaderboardsClient customLeaderboardsClient) => customLeaderboardsClient switch
	{
		CustomLeaderboardsClient.DdstatsRust => DdLiveApiCustomLeaderboards.CustomLeaderboardsClient.DdstatsRust,
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => DdLiveApiCustomLeaderboards.CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		_ => throw new InvalidOperationException($"Cannot convert custom leaderboards client '{customLeaderboardsClient}' to a DDLIVE API model."),
	};

	public static DdLiveApiLeaderboardStatistics.GetArrayStatisticsDdLive ToGetArrayStatisticsDdLive(this ArrayStatistics arrayStatistics) => new()
	{
		Accuracy = arrayStatistics.Accuracy.ToGetArrayStatisticDdLive(),
		DaggersFired = arrayStatistics.DaggersFired.ToGetArrayStatisticDdLive(),
		DaggersHit = arrayStatistics.DaggersHit.ToGetArrayStatisticDdLive(),
		Gems = arrayStatistics.Gems.ToGetArrayStatisticDdLive(),
		Kills = arrayStatistics.Kills.ToGetArrayStatisticDdLive(),
		Times = arrayStatistics.Times.ToGetArrayStatisticDdLive(),
	};

	private static DdLiveApiLeaderboardStatistics.GetArrayStatisticDdLive ToGetArrayStatisticDdLive(this ArrayStatistic arrayStatistic) => new()
	{
		Average = arrayStatistic.Average,
		Median = arrayStatistic.Median,
		Mode = arrayStatistic.Mode,
	};
}
