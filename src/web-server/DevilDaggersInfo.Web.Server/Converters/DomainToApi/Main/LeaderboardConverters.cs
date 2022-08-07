using DevilDaggersInfo.Api.Main.Leaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class LeaderboardConverters
{
	public static GetLeaderboard ToGetLeaderboardPublic(this LeaderboardResponse leaderboardResponse) => new()
	{
		DaggersFiredGlobal = leaderboardResponse.DaggersFiredGlobal,
		DaggersHitGlobal = leaderboardResponse.DaggersHitGlobal,
		DateTime = leaderboardResponse.DateTime,
		DeathsGlobal = leaderboardResponse.DeathsGlobal,
		Entries = leaderboardResponse.Entries.ConvertAll(e => e.ToGetEntryPublic()),
		GemsGlobal = leaderboardResponse.GemsGlobal,
		KillsGlobal = leaderboardResponse.KillsGlobal,
		TotalPlayers = leaderboardResponse.TotalPlayers,
		TimeGlobal = leaderboardResponse.TimeGlobal == 0 ? 0 : leaderboardResponse.TimeGlobal.ToSecondsTime(),
	};

	public static GetEntry ToGetEntryPublic(this EntryResponse entryResponse) => new()
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
