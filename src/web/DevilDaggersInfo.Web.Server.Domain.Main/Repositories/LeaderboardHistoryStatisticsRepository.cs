using DevilDaggersInfo.Api.Main.LeaderboardHistoryStatistics;
using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.LeaderboardHistory;
using DevilDaggersInfo.Web.Server.Domain.Services.Caching;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Repositories;

public class LeaderboardHistoryStatisticsRepository
{
	private readonly IFileSystemService _fileSystemService;
	private readonly ILeaderboardHistoryCache _leaderboardHistoryCache;

	public LeaderboardHistoryStatisticsRepository(IFileSystemService fileSystemService, ILeaderboardHistoryCache leaderboardHistoryCache)
	{
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	public List<GetLeaderboardHistoryStatistics> GetLeaderboardHistoryStatistics()
	{
		string? firstPath = _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).Where(p => p.EndsWith(".bin")).MinBy(p => p);
		if (firstPath == null)
			return new();

		LeaderboardHistory current = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(firstPath);

		ulong daggersFiredGlobal = current.DaggersFiredGlobal;
		ulong daggersHitGlobal = current.DaggersHitGlobal;
		ulong deathsGlobal = current.DeathsGlobal;
		ulong gemsGlobal = current.GemsGlobal;
		ulong killsGlobal = current.KillsGlobal;
		double timeGlobal = current.TimeGlobal.ToSecondsTime();
		double rank100 = GetTimeOr0(current, 100);
		double rank10 = GetTimeOr0(current, 10);
		double rank3 = GetTimeOr0(current, 3);
		double rank2 = GetTimeOr0(current, 2);
		double rank1 = GetTimeOr0(current, 1);
		int totalPlayers = current.Players;

		List<GetLeaderboardHistoryStatistics> leaderboardHistoryStatistics = new();
		DateTime dateTime = current.DateTime;
		Add(true, true, true, true, true, true, true, true, true, true, true, true);

		const int dayOffset = 7;
		while (dateTime < DateTime.UtcNow.AddDays(-dayOffset))
		{
			dateTime = dateTime.AddDays(dayOffset);
			string historyPath = _fileSystemService.GetLeaderboardHistoryPathFromDate(dateTime);
			current = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(historyPath);

			bool daggersFiredUpdated = false;
			bool daggersHitUpdated = false;
			bool deathsUpdated = false;
			bool gemsUpdated = false;
			bool killsUpdated = false;
			bool totalPlayersUpdated = false;
			bool timeUpdated = false;

			if (daggersFiredGlobal != current.DaggersFiredGlobal)
			{
				daggersFiredGlobal = current.DaggersFiredGlobal;
				daggersFiredUpdated = true;
			}

			if (daggersHitGlobal != current.DaggersHitGlobal)
			{
				daggersHitGlobal = current.DaggersHitGlobal;
				daggersHitUpdated = true;
			}

			if (deathsGlobal != current.DeathsGlobal)
			{
				deathsGlobal = current.DeathsGlobal;
				deathsUpdated = true;
			}

			if (gemsGlobal != current.GemsGlobal)
			{
				gemsGlobal = current.GemsGlobal;
				gemsUpdated = true;
			}

			if (killsGlobal != current.KillsGlobal)
			{
				killsGlobal = current.KillsGlobal;
				killsUpdated = true;
			}

			if (totalPlayers != current.Players)
			{
				totalPlayers = current.Players;
				totalPlayersUpdated = true;
			}

			double currentTimeGlobal = current.TimeGlobal.ToSecondsTime();
			if (Math.Abs(timeGlobal - currentTimeGlobal) > 0.0002)
			{
				timeGlobal = currentTimeGlobal;
				timeUpdated = true;
			}

			double currentRank100 = GetTimeOr0(current, 100);
			rank100 = currentRank100;

			double currentRank10 = GetTimeOr0(current, 10);
			rank10 = currentRank10;

			double currentRank3 = GetTimeOr0(current, 3);
			rank3 = currentRank3;

			double currentRank2 = GetTimeOr0(current, 2);
			rank2 = currentRank2;

			double currentRank1 = GetTimeOr0(current, 1);
			rank1 = currentRank1;

			Add(daggersFiredUpdated, daggersHitUpdated, deathsUpdated, gemsUpdated, killsUpdated, totalPlayersUpdated, timeUpdated, currentRank100 != 0, currentRank10 != 0, currentRank3 != 0, currentRank2 != 0, currentRank1 != 0);
		}

		return leaderboardHistoryStatistics;

		void Add(bool daggersFiredUpdated, bool daggersHitUpdated, bool deathsUpdated, bool gemsUpdated, bool killsUpdated, bool totalPlayersUpdated, bool timeUpdated, bool rank100Updated, bool rank10Updated, bool rank3Updated, bool rank2Updated, bool rank1Updated) => leaderboardHistoryStatistics.Add(new()
		{
			DateTime = dateTime,
			DaggersFiredGlobal = daggersFiredGlobal,
			DaggersHitGlobal = daggersHitGlobal,
			DeathsGlobal = deathsGlobal,
			GemsGlobal = gemsGlobal,
			KillsGlobal = killsGlobal,
			TimeGlobal = timeGlobal,
			Top100Entrance = rank100,
			Top10Entrance = rank10,
			Top3Entrance = rank3,
			Top2Entrance = rank2,
			Top1Entrance = rank1,
			TotalPlayers = totalPlayers,
			DaggersFiredGlobalUpdated = daggersFiredUpdated,
			DaggersHitGlobalUpdated = daggersHitUpdated,
			DeathsGlobalUpdated = deathsUpdated,
			GemsGlobalUpdated = gemsUpdated,
			KillsGlobalUpdated = killsUpdated,
			TimeGlobalUpdated = timeUpdated,
			Top100EntranceUpdated = rank100Updated,
			Top10EntranceUpdated = rank10Updated,
			Top3EntranceUpdated = rank3Updated,
			Top2EntranceUpdated = rank2Updated,
			Top1EntranceUpdated = rank1Updated,
			TotalPlayersUpdated = totalPlayersUpdated,
		});

		static double GetTimeOr0(LeaderboardHistory history, int rank)
			=> history.Entries.Find(eh => eh.Rank == rank)?.Time.ToSecondsTime() ?? 0;
	}
}
