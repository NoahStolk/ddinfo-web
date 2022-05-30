using DevilDaggersInfo.Web.Server.InternalModels.CustomEntries;
using DevilDaggersInfo.Web.Shared.Dto.Public.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.Public;

public static class CustomLeaderboardConverters
{
	public static GetCustomLeaderboardDdcl ToGetCustomLeaderboardDdcl(this CustomLeaderboardEntity customLeaderboard) => new()
	{
		SpawnsetName = customLeaderboard.Spawnset.Name,
		Daggers = customLeaderboard.ToGetCustomLeaderboardDaggers(),
		Category = customLeaderboard.Category,
		IsAscending = customLeaderboard.Category.IsAscending(),
	};

	public static GetCustomLeaderboardOverview ToGetCustomLeaderboardOverview(this CustomLeaderboardEntity customLeaderboard, int playerCount, string? topPlayer, int? worldRecord) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		Daggers = customLeaderboard.ToGetCustomLeaderboardDaggers(),
		IsFeatured = customLeaderboard.IsFeatured,
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = playerCount,
		TopPlayer = topPlayer,
		WorldRecord = worldRecord.ToSecondsTime(),
		WorldRecordDagger = worldRecord.HasValue ? customLeaderboard.GetDaggerFromTime(worldRecord.Value) : null,
	};

	public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboardEntity customLeaderboard, List<CustomEntry> customEntries) => new()
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
		CustomEntries = customEntries.Select((ce, i) => ce.ToGetCustomEntry(customLeaderboard, i + 1)).ToList(),
	};

	private static GetCustomLeaderboardDaggers? ToGetCustomLeaderboardDaggers(this CustomLeaderboardEntity customLeaderboard) => !customLeaderboard.IsFeatured ? null : new()
	{
		Bronze = customLeaderboard.TimeBronze.ToSecondsTime(),
		Silver = customLeaderboard.TimeSilver.ToSecondsTime(),
		Golden = customLeaderboard.TimeGolden.ToSecondsTime(),
		Devil = customLeaderboard.TimeDevil.ToSecondsTime(),
		Leviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
	};
}
