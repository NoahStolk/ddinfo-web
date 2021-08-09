using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using DevilDaggersWebsite.BlazorWasm.Shared.Extensions;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters.Public
{
	public static class CustomLeaderboardConverters
	{
		public static GetCustomLeaderboardDdcl ToGetCustomLeaderboardDdcl(this CustomLeaderboard customLeaderboard) => new()
		{
			SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.PlayerName,
			SpawnsetName = customLeaderboard.SpawnsetFile.Name,
			TimeBronze = customLeaderboard.TimeBronze,
			TimeSilver = customLeaderboard.TimeSilver,
			TimeGolden = customLeaderboard.TimeGolden,
			TimeDevil = customLeaderboard.TimeDevil,
			TimeLeviathan = customLeaderboard.TimeLeviathan,
			DateLastPlayed = customLeaderboard.DateLastPlayed,
			DateCreated = customLeaderboard.DateCreated,
			Category = customLeaderboard.Category,
			IsAscending = customLeaderboard.Category.IsAscending(),
		};

		public static GetCustomLeaderboardOverview ToGetCustomLeaderboardOverview(this CustomLeaderboard customLeaderboard, string? topPlayer, int? worldRecord) => new()
		{
			Id = customLeaderboard.Id,
			SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.PlayerName,
			SpawnsetName = customLeaderboard.SpawnsetFile.Name,
			TimeBronze = customLeaderboard.TimeBronze.ToSecondsTime(),
			TimeSilver = customLeaderboard.TimeSilver.ToSecondsTime(),
			TimeGolden = customLeaderboard.TimeGolden.ToSecondsTime(),
			TimeDevil = customLeaderboard.TimeDevil.ToSecondsTime(),
			TimeLeviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
			DateCreated = customLeaderboard.DateCreated,
			DateLastPlayed = customLeaderboard.DateLastPlayed,
			TopPlayer = topPlayer,
			WorldRecord = worldRecord.ToSecondsTime(),
			WorldRecordDagger = worldRecord.HasValue ? customLeaderboard.GetDaggerFromTime(worldRecord.Value) : CustomLeaderboardDagger.Default,
		};

		public static GetCustomLeaderboard ToGetCustomLeaderboard(this CustomLeaderboard customLeaderboard) => new()
		{
			SpawnsetAuthorName = customLeaderboard.SpawnsetFile.Player.PlayerName,
			SpawnsetName = customLeaderboard.SpawnsetFile.Name,
			TimeBronze = customLeaderboard.TimeBronze.ToSecondsTime(),
			TimeSilver = customLeaderboard.TimeSilver.ToSecondsTime(),
			TimeGolden = customLeaderboard.TimeGolden.ToSecondsTime(),
			TimeDevil = customLeaderboard.TimeDevil.ToSecondsTime(),
			TimeLeviathan = customLeaderboard.TimeLeviathan.ToSecondsTime(),
			DateCreated = customLeaderboard.DateCreated,
			TotalRunsSubmitted = customLeaderboard.DateCreated < CustomLeaderboardFeatureConstants.SubmitCount ? null : customLeaderboard.TotalRunsSubmitted,
			DateLastPlayed = customLeaderboard.DateLastPlayed,
			CustomEntries = customLeaderboard.CustomEntries?.ConvertAll(ce => ce.ToGetCustomEntry()) ?? new(),
		};
	}
}
