using DevilDaggersInfo.Api.Main.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;
using DevilDaggersInfo.Web.Shared.Dto.Public.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class CustomLeaderboardConverters
{
	public static GetCustomLeaderboardOverview ToGetCustomLeaderboardOverview(this CustomLeaderboardOverview customLeaderboard) => new()
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
		WorldRecordDagger = customLeaderboard.WorldRecord?.Dagger,
	};

	public static GetCustomLeaderboard ToGetCustomLeaderboard(this SortedCustomLeaderboard customLeaderboard) => new()
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

	private static GetCustomEntry ToGetCustomEntry(this CustomEntry customEntry) => new()
	{
		Id = customEntry.Id,
		Rank = customEntry.Rank,
		PlayerId = customEntry.PlayerId,
		PlayerName = customEntry.PlayerName,
		CountryCode = customEntry.CountryCode,
		Client = customEntry.Client,
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
		CustomLeaderboardDagger = customEntry.CustomLeaderboardDagger,
		HasGraphs = customEntry.HasGraphs,
	};

	private static GetCustomLeaderboardDaggers? ToGetCustomLeaderboardDaggers(this CustomLeaderboardDaggers customLeaderboard) => new()
	{
		Bronze = customLeaderboard.Bronze.ToSecondsTime(),
		Silver = customLeaderboard.Silver.ToSecondsTime(),
		Golden = customLeaderboard.Golden.ToSecondsTime(),
		Devil = customLeaderboard.Devil.ToSecondsTime(),
		Leviathan = customLeaderboard.Leviathan.ToSecondsTime(),
	};
}
