using DevilDaggersInfo.Api.Main.Leaderboards;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

// TODO: Move elsewhere; this isn't part of the domain.
public static class LeaderboardConverters
{
	public static GetLeaderboard ToMainApi(this IDdLeaderboardService.LeaderboardResponse leaderboardResponse) => new()
	{
		DaggersFiredGlobal = leaderboardResponse.DaggersFiredGlobal,
		DaggersHitGlobal = leaderboardResponse.DaggersHitGlobal,
		DateTime = leaderboardResponse.DateTime,
		DeathsGlobal = leaderboardResponse.DeathsGlobal,
		Entries = leaderboardResponse.Entries.ConvertAll(e => e.ToMainApi()),
		GemsGlobal = leaderboardResponse.GemsGlobal,
		KillsGlobal = leaderboardResponse.KillsGlobal,
		TotalPlayers = leaderboardResponse.TotalPlayers,
		TimeGlobal = leaderboardResponse.TimeGlobal == 0 ? 0 : leaderboardResponse.TimeGlobal.ToSecondsTime(),
	};

	public static GetEntry ToMainApi(this IDdLeaderboardService.EntryResponse entryResponse) => new()
	{
		DaggersFired = entryResponse.DaggersFired,
		DaggersFiredTotal = entryResponse.DaggersFiredTotal,
		DaggersHit = entryResponse.DaggersHit,
		DaggersHitTotal = entryResponse.DaggersHitTotal,
		DeathsTotal = entryResponse.DeathsTotal,
		DeathType = (byte)entryResponse.DeathType,
		Gems = entryResponse.Gems,
		GemsTotal = entryResponse.GemsTotal,
		Id = entryResponse.Id,
		Kills = entryResponse.Kills,
		KillsTotal = entryResponse.KillsTotal,
		Rank = entryResponse.Rank,
		Time = entryResponse.Time.ToSecondsTime(),
		TimeTotal = entryResponse.TimeTotal.ToSecondsTime(),
		Username = entryResponse.Username,
	};
}
