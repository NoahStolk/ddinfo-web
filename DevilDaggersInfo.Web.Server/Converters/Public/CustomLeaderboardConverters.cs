using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Entities.Views;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class CustomLeaderboardConverters
{
	public static GetCustomLeaderboardDdcl ToGetCustomLeaderboardDdcl(this CustomLeaderboardEntity customLeaderboard) => new()
	{
		SpawnsetName = customLeaderboard.Spawnset.Name,
		Daggers = ToGetCustomLeaderboardDaggers(customLeaderboard),
		Category = customLeaderboard.Category,
		IsAscending = customLeaderboard.Category.IsAscending(),
	};

	public static GetCustomLeaderboardOverview ToGetCustomLeaderboardOverview(this CustomLeaderboardEntity customLeaderboard, int playerCount, string? topPlayer, int? worldRecord) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		Daggers = ToGetCustomLeaderboardDaggers(customLeaderboard),
		IsFeatured = customLeaderboard.IsFeatured,
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = playerCount,
		TopPlayer = topPlayer,
		WorldRecord = worldRecord.ToSecondsTime(),
		WorldRecordDagger = worldRecord.HasValue ? customLeaderboard.GetDaggerFromTime(worldRecord.Value) : null,
	};

	public static GetCustomLeaderboardOverviewDdLive ToGetCustomLeaderboardOverviewDdLive(this CustomLeaderboardEntity customLeaderboard, int playerCount, int? topPlayerId, string? topPlayerName, int? worldRecord) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		SpawnsetAuthorId = customLeaderboard.Spawnset.PlayerId,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		Daggers = ToGetCustomLeaderboardDaggers(customLeaderboard),
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

	public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboardEntity customLeaderboard, List<CustomEntry> customEntries) => new()
	{
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		SpawnsetHtmlDescription = customLeaderboard.Spawnset.HtmlDescription,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		Daggers = ToGetCustomLeaderboardDaggers(customLeaderboard),
		DateCreated = customLeaderboard.DateCreated,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		Category = customLeaderboard.Category,
		IsFeatured = customLeaderboard.IsFeatured,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customEntries.Select((ce, i) => ce.ToGetCustomEntry(customLeaderboard, i + 1)).ToList(),
	};

	public static GetCustomLeaderboardDdLive ToGetCustomLeaderboardDdLive(this CustomLeaderboardEntity customLeaderboard, List<CustomEntry> customEntries, List<int> customEntryReplayIds) => new()
	{
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		SpawnsetHtmlDescription = customLeaderboard.Spawnset.HtmlDescription,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		Daggers = ToGetCustomLeaderboardDaggers(customLeaderboard),
		DateCreated = customLeaderboard.DateCreated,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		Category = customLeaderboard.Category,
		IsFeatured = customLeaderboard.IsFeatured,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customEntries.Select((ce, i) => ce.ToGetCustomEntryDdLive(customLeaderboard, i + 1, customEntryReplayIds.Contains(ce.Id))).ToList(),
	};

	private static GetCustomLeaderboardDaggers? ToGetCustomLeaderboardDaggers(CustomLeaderboardEntity customLeaderboard) => !customLeaderboard.IsFeatured ? null : new()
	{
		Bronze = customLeaderboard.TimeBronze.ToSecondsTime(),
		Silver = customLeaderboard.TimeSilver.ToSecondsTime(),
		Golden = customLeaderboard.TimeGolden.ToSecondsTime(),
		Devil = customLeaderboard.TimeDevil.ToSecondsTime(),
		Leviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
	};
}
