using DevilDaggersWebsite.BlazorWasm.Server.Clients.Leaderboard;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Leaderboards;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums;

namespace DevilDaggersWebsite.BlazorWasm.Server.Converters
{
	public static class LeaderboardConverters
	{
		public static GetLeaderboardPublic ToGetLeaderboardPublic(this LeaderboardResponse leaderboardResponse) => new()
		{
			DaggersFiredGlobal = leaderboardResponse.DaggersFiredGlobal,
			DaggersHitGlobal = leaderboardResponse.DaggersHitGlobal,
			DateTime = leaderboardResponse.DateTime,
			DeathsGlobal = leaderboardResponse.DeathsGlobal,
			Entries = leaderboardResponse.Entries.ConvertAll(e => e.ToGetEntryPublic()),
			GemsGlobal = leaderboardResponse.GemsGlobal,
			KillsGlobal = leaderboardResponse.KillsGlobal,
			TotalPlayers = leaderboardResponse.TotalPlayers,
			TimeGlobal = leaderboardResponse.TimeGlobal == 0 ? 0 : leaderboardResponse.TimeGlobal / 10000.0,
		};

		public static GetEntryPublic ToGetEntryPublic(this EntryResponse entryResponse) => new()
		{
			DaggersFired = entryResponse.DaggersFired,
			DaggersFiredTotal = entryResponse.DaggersFiredTotal,
			DaggersHit = entryResponse.DaggersHit,
			DaggersHitTotal = entryResponse.DaggersHitTotal,
			DeathsTotal = entryResponse.DeathsTotal,
			DeathType = (DeathType)entryResponse.DeathType,
			Gems = entryResponse.Gems,
			GemsTotal = entryResponse.GemsTotal,
			Id = entryResponse.Id,
			Kills = entryResponse.Kills,
			KillsTotal = entryResponse.KillsTotal,
			Rank = entryResponse.Rank,
			Time = entryResponse.Time / 10000.0,
			TimeTotal = entryResponse.TimeTotal / 10000.0,
			Username = entryResponse.Username,
		};
	}
}
