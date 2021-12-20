using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels.Json;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.LeaderboardHistoryStatistics;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/leaderboard-history-statistics")]
[ApiController]
public class LeaderboardHistoryStatisticsController : ControllerBase
{
	private readonly IFileSystemService _fileSystemService;
	private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

	public LeaderboardHistoryStatisticsController(IFileSystemService fileSystemService, LeaderboardHistoryCache leaderboardHistoryCache)
	{
		_fileSystemService = fileSystemService;
		_leaderboardHistoryCache = leaderboardHistoryCache;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<List<GetLeaderboardHistoryStatistics>> GetLeaderboardHistoryStatistics()
	{
		string? firstPath = _fileSystemService.TryGetFiles(DataSubDirectory.LeaderboardHistory).OrderBy(p => p).FirstOrDefault();
		if (firstPath == null)
			return new List<GetLeaderboardHistoryStatistics>();

		LeaderboardHistory current = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(firstPath);

		ulong daggersFiredGlobal = current.DaggersFiredGlobal;
		ulong daggersHitGlobal = current.DaggersHitGlobal;
		ulong deathsGlobal = current.DeathsGlobal;
		ulong gemsGlobal = current.GemsGlobal;
		ulong killsGlobal = current.KillsGlobal;
		double timeGlobal = current.TimeGlobal.ToSecondsTime();
		double rank100 = GetTimeOr0(current, 100);
		double rank10 = GetTimeOr0(current, 10);
		int totalPlayers = current.Players;

		List<GetLeaderboardHistoryStatistics> leaderboardHistoryStatistics = new();
		DateTime dateTime = current.DateTime;
		Add(true, true, true, true, true, true, true, true, true);

		while (dateTime < DateTime.UtcNow)
		{
			dateTime = dateTime.AddDays(7);
			string historyPath = _fileSystemService.GetLeaderboardHistoryPathFromDate(dateTime);
			current = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(historyPath);

			bool daggersFiredUpdated = false;
			bool daggersHitUpdated = false;
			bool deathsUpdated = false;
			bool gemsUpdated = false;
			bool killsUpdated = false;
			bool totalPlayersUpdated = false;
			bool timeUpdated = false;
			bool rank100Updated = false;
			bool rank10Updated = false;

			if (daggersFiredGlobal < current.DaggersFiredGlobal)
			{
				daggersFiredGlobal = current.DaggersFiredGlobal;
				daggersFiredUpdated = true;
			}

			if (daggersHitGlobal < current.DaggersHitGlobal)
			{
				daggersHitGlobal = current.DaggersHitGlobal;
				daggersHitUpdated = true;
			}

			if (deathsGlobal < current.DeathsGlobal)
			{
				deathsGlobal = current.DeathsGlobal;
				deathsUpdated = true;
			}

			if (gemsGlobal < current.GemsGlobal)
			{
				gemsGlobal = current.GemsGlobal;
				gemsUpdated = true;
			}

			if (killsGlobal < current.KillsGlobal)
			{
				killsGlobal = current.KillsGlobal;
				killsUpdated = true;
			}

			if (totalPlayers < current.Players)
			{
				totalPlayers = current.Players;
				totalPlayersUpdated = true;
			}

			double currentTimeGlobal = current.TimeGlobal.ToSecondsTime();
			if (timeGlobal < currentTimeGlobal)
			{
				timeGlobal = currentTimeGlobal;
				timeUpdated = true;
			}

			double currentRank100 = GetTimeOr0(current, 100);
			if (rank100 < currentRank100)
				rank100 = currentRank100;
			rank100Updated = currentRank100 != 0;

			double currentRank10 = GetTimeOr0(current, 10);
			if (rank10 < currentRank10)
				rank10 = currentRank10;
			rank10Updated = currentRank10 != 0;

			Add(daggersFiredUpdated, daggersHitUpdated, deathsUpdated, gemsUpdated, killsUpdated, totalPlayersUpdated, timeUpdated, rank100Updated, rank10Updated);
		}

		return leaderboardHistoryStatistics;

		void Add(bool daggersFiredUpdated, bool daggersHitUpdated, bool deathsUpdated, bool gemsUpdated, bool killsUpdated, bool totalPlayersUpdated, bool timeUpdated, bool rank100Updated, bool rank10Updated) => leaderboardHistoryStatistics.Add(new()
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
			TotalPlayers = totalPlayers,
			DaggersFiredGlobalUpdated = daggersFiredUpdated,
			DaggersHitGlobalUpdated = daggersHitUpdated,
			DeathsGlobalUpdated = deathsUpdated,
			GemsGlobalUpdated = gemsUpdated,
			KillsGlobalUpdated = killsUpdated,
			TimeGlobalUpdated = timeUpdated,
			Top100EntranceUpdated = rank100Updated,
			Top10EntranceUpdated = rank10Updated,
			TotalPlayersUpdated = totalPlayersUpdated,
		});

		static double GetTimeOr0(LeaderboardHistory history, int rank)
			=> history.Entries.Find(eh => eh.Rank == rank)?.Time.ToSecondsTime() ?? 0;
	}
}
