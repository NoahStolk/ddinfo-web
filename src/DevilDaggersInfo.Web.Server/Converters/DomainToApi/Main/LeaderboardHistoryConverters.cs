using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Web.ApiSpec.Main.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class LeaderboardHistoryConverters
{
	public static GetLeaderboardHistory ToMainApi(this LeaderboardHistory leaderboard) => new()
	{
		DaggersFiredGlobal = leaderboard.DaggersFiredGlobal,
		DaggersHitGlobal = leaderboard.DaggersHitGlobal,
		DateTime = leaderboard.DateTime,
		DeathsGlobal = leaderboard.DeathsGlobal,
		Entries = leaderboard.Entries.ConvertAll(eh => eh.ToMainApi(leaderboard.DateTime)),
		GemsGlobal = leaderboard.GemsGlobal,
		KillsGlobal = leaderboard.KillsGlobal,
		TimeGlobal = leaderboard.TimeGlobal.ToSecondsTime(),
		TotalPlayers = leaderboard.Players,
	};

	private static GetEntryHistory ToMainApi(this EntryHistory entry, DateTime dateTime) => new()
	{
		DateTime = dateTime,
		DaggersFired = entry.DaggersFired,
		DaggersFiredTotal = entry.DaggersFiredTotal,
		DaggersHit = entry.DaggersHit,
		DaggersHitTotal = entry.DaggersHitTotal,
		DeathsTotal = entry.DeathsTotal,
		DeathType = entry.DeathType,
		Gems = entry.Gems,
		GemsTotal = entry.GemsTotal,
		Id = entry.Id,
		Kills = entry.Kills,
		KillsTotal = entry.KillsTotal,
		Rank = entry.Rank,
		Time = entry.Time.ToSecondsTime(),
		TimeTotal = entry.TimeTotal.ToSecondsTime(),
		Username = entry.Username,
	};
}
