using DevilDaggersInfo.Web.Server.InternalModels.CustomEntries;
using DevilDaggersInfo.Web.Shared.Dto.DdLive.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DdLive;

public static class CustomLeaderboardConverters
{
	public static GetCustomLeaderboardOverviewDdLive ToGetCustomLeaderboardOverviewDdLive(this CustomLeaderboardEntity customLeaderboard, int playerCount, int? topPlayerId, string? topPlayerName, int? worldRecord) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		SpawnsetAuthorId = customLeaderboard.Spawnset.PlayerId,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		Daggers = customLeaderboard.ToGetCustomLeaderboardDaggers(),
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = playerCount,
		Category = customLeaderboard.Category,
		TopPlayerId = topPlayerId,
		TopPlayerName = topPlayerName,
		WorldRecord = worldRecord.ToSecondsTime(),
		WorldRecordDagger = worldRecord.HasValue ? customLeaderboard.GetDaggerFromTime(worldRecord.Value) : null,
	};

	public static GetCustomLeaderboardDdLive ToGetCustomLeaderboardDdLive(this CustomLeaderboardEntity customLeaderboard, List<CustomEntry> customEntries, List<int> customEntryReplayIds) => new()
	{
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		SpawnsetHtmlDescription = customLeaderboard.Spawnset.HtmlDescription,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		Daggers = customLeaderboard.ToGetCustomLeaderboardDaggers(),
		DateCreated = customLeaderboard.DateCreated,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		Category = customLeaderboard.Category,
		IsFeatured = customLeaderboard.IsFeatured,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customEntries.Select((ce, i) => ce.ToGetCustomEntryDdLive(customLeaderboard, i + 1, customEntryReplayIds.Contains(ce.Id))).ToList(),
	};

	private static GetCustomLeaderboardDaggersDdLive? ToGetCustomLeaderboardDaggers(this CustomLeaderboardEntity customLeaderboard) => !customLeaderboard.IsFeatured ? null : new()
	{
		Bronze = customLeaderboard.TimeBronze.ToSecondsTime(),
		Silver = customLeaderboard.TimeSilver.ToSecondsTime(),
		Golden = customLeaderboard.TimeGolden.ToSecondsTime(),
		Devil = customLeaderboard.TimeDevil.ToSecondsTime(),
		Leviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
	};
}
