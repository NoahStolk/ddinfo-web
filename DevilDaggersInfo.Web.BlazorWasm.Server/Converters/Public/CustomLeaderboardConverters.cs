using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

public static class CustomLeaderboardConverters
{
	public static GetCustomLeaderboardDdcl ToGetCustomLeaderboardDdcl(this CustomLeaderboardEntity customLeaderboard) => new()
	{
		SpawnsetName = customLeaderboard.Spawnset.Name,
		TimeBronze = customLeaderboard.TimeBronze.ZeroIfArchived(customLeaderboard.IsArchived),
		TimeSilver = customLeaderboard.TimeSilver.ZeroIfArchived(customLeaderboard.IsArchived),
		TimeGolden = customLeaderboard.TimeGolden.ZeroIfArchived(customLeaderboard.IsArchived),
		TimeDevil = customLeaderboard.TimeDevil.ZeroIfArchived(customLeaderboard.IsArchived),
		TimeLeviathan = customLeaderboard.TimeLeviathan.ZeroIfArchived(customLeaderboard.IsArchived),
		Category = customLeaderboard.Category,
		IsAscending = customLeaderboard.Category.IsAscending(),
	};

	public static GetCustomLeaderboardOverview ToGetCustomLeaderboardOverview(this CustomLeaderboardEntity customLeaderboard, int playerCount, string? topPlayer, int? worldRecord) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		TimeBronze = customLeaderboard.TimeBronze.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeSilver = customLeaderboard.TimeSilver.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeGolden = customLeaderboard.TimeGolden.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeDevil = customLeaderboard.TimeDevil.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeLeviathan = customLeaderboard.TimeLeviathan.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = playerCount,
		TopPlayer = topPlayer,
		WorldRecord = worldRecord.ToSecondsTime(),
		WorldRecordDagger = worldRecord.HasValue ? customLeaderboard.GetDaggerFromTime(worldRecord.Value) : CustomLeaderboardDagger.Default,
	};

	public static GetCustomLeaderboardDdLive ToGetCustomLeaderboardDdLive(this CustomLeaderboardEntity customLeaderboard, int playerCount, int? topPlayerId, string? topPlayerName, int? worldRecord) => new()
	{
		Id = customLeaderboard.Id,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		SpawnsetAuthorId = customLeaderboard.Spawnset.PlayerId,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		TimeBronze = customLeaderboard.TimeBronze.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeSilver = customLeaderboard.TimeSilver.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeGolden = customLeaderboard.TimeGolden.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeDevil = customLeaderboard.TimeDevil.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeLeviathan = customLeaderboard.TimeLeviathan.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		DateCreated = customLeaderboard.DateCreated,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		PlayerCount = playerCount,
		Category = customLeaderboard.Category,
		TopPlayerId = topPlayerId,
		TopPlayerName = topPlayerName,
		WorldRecord = worldRecord.ToSecondsTime(),
		WorldRecordDagger = worldRecord.HasValue ? customLeaderboard.GetDaggerFromTime(worldRecord.Value) : CustomLeaderboardDagger.Default,
	};

	public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboardEntity customLeaderboard, List<int> existingReplayIds) => new()
	{
		SpawnsetId = customLeaderboard.SpawnsetId,
		SpawnsetAuthorName = customLeaderboard.Spawnset.Player.PlayerName,
		SpawnsetHtmlDescription = customLeaderboard.Spawnset.HtmlDescription,
		SpawnsetName = customLeaderboard.Spawnset.Name,
		TimeBronze = customLeaderboard.TimeBronze.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeSilver = customLeaderboard.TimeSilver.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeGolden = customLeaderboard.TimeGolden.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeDevil = customLeaderboard.TimeDevil.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		TimeLeviathan = customLeaderboard.TimeLeviathan.NullIfArchived(customLeaderboard.IsArchived).ToSecondsTime(),
		DateCreated = customLeaderboard.DateCreated,
		SubmitCount = customLeaderboard.TotalRunsSubmitted,
		Category = customLeaderboard.Category,
		IsArchived = customLeaderboard.IsArchived,
		DateLastPlayed = customLeaderboard.DateLastPlayed,
		CustomEntries = customLeaderboard.CustomEntries?
			.OrderBy(ce => ce.Time, customLeaderboard.Category.IsAscending())
			.ThenBy(ce => ce.SubmitDate)
			.Select((ce, i) => ce.ToGetCustomEntry(i + 1, existingReplayIds.Contains(ce.Id)))
			.ToList() ?? new(),
	};

	private static int? NullIfArchived(this int time, bool isArchived) => isArchived ? null : time;

	private static int ZeroIfArchived(this int time, bool isArchived) => isArchived ? 0 : time;
}
