using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Entities.Enums;
using DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;
using MainApi = DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class CustomLeaderboardConverters
{
	public static MainApi.GetCustomLeaderboardOverview ToGetCustomLeaderboardOverview(this CustomLeaderboardOverview customLeaderboard) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetAuthorName = customLeaderboard.SpawnsetAuthorName,
		SpawnsetName = customLeaderboard.SpawnsetName,
		Daggers = customLeaderboard.Daggers?.ToGetCustomLeaderboardDaggers(),
		IsFeatured = customLeaderboard.Daggers != null,
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = customLeaderboard.PlayerCount,
		TopPlayer = customLeaderboard.WorldRecord?.PlayerName,
		WorldRecord = customLeaderboard.WorldRecord?.Time.ToSecondsTime(),
		WorldRecordDagger = customLeaderboard.WorldRecord?.Dagger?.ToMainApi(),
	};

	public static MainApi.GetCustomLeaderboard ToGetCustomLeaderboard(this SortedCustomLeaderboard customLeaderboard) => new()
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
		CustomEntries = customLeaderboard.CustomEntries.ConvertAll(ce => ce.ToGetCustomEntry()),
	};

	private static MainApi.GetCustomEntry ToGetCustomEntry(this CustomEntry customEntry) => new()
	{
		Id = customEntry.Id,
		Rank = customEntry.Rank,
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
		CountryCode = customEntry.CountryCode,
		Client = customEntry.Client.ToMainApi(),
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
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger?.ToMainApi(),
		HasGraphs = customEntry.HasGraphs,
	};

	private static MainApi.GetCustomLeaderboardDaggers? ToGetCustomLeaderboardDaggers(this CustomLeaderboardDaggers customLeaderboard) => new()
	{
		Bronze = customLeaderboard.Bronze.ToSecondsTime(),
		Silver = customLeaderboard.Silver.ToSecondsTime(),
		Golden = customLeaderboard.Golden.ToSecondsTime(),
		Devil = customLeaderboard.Devil.ToSecondsTime(),
		Leviathan = customLeaderboard.Leviathan.ToSecondsTime(),
	};

	private static MainApi.CustomLeaderboardDagger ToMainApi(this CustomLeaderboardDagger customLeaderboardDagger) => customLeaderboardDagger switch
	{
		CustomLeaderboardDagger.Default => MainApi.CustomLeaderboardDagger.Default,
		CustomLeaderboardDagger.Bronze => MainApi.CustomLeaderboardDagger.Bronze,
		CustomLeaderboardDagger.Silver => MainApi.CustomLeaderboardDagger.Silver,
		CustomLeaderboardDagger.Golden => MainApi.CustomLeaderboardDagger.Golden,
		CustomLeaderboardDagger.Devil => MainApi.CustomLeaderboardDagger.Devil,
		CustomLeaderboardDagger.Leviathan => MainApi.CustomLeaderboardDagger.Leviathan,
		_ => throw new InvalidEnumConversionException(customLeaderboardDagger),
	};

	private static MainApi.CustomLeaderboardsClient ToMainApi(this CustomLeaderboardsClient customLeaderboardsClient) => customLeaderboardsClient switch
	{
		CustomLeaderboardsClient.DdstatsRust => MainApi.CustomLeaderboardsClient.DdstatsRust,
		CustomLeaderboardsClient.DevilDaggersCustomLeaderboards => MainApi.CustomLeaderboardsClient.DevilDaggersCustomLeaderboards,
		_ => throw new InvalidEnumConversionException(customLeaderboardsClient),
	};
}
