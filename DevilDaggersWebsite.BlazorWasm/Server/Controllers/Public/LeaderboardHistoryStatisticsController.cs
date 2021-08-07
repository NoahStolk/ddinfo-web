using DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Utils.Data;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.LeaderboardHistoryStatistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Public
{
	[Route("api/leaderboard-history-statistics")]
	[ApiController]
	public class LeaderboardHistoryStatisticsController : ControllerBase
	{
		private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

		public LeaderboardHistoryStatisticsController(LeaderboardHistoryCache leaderboardHistoryCache)
		{
			_leaderboardHistoryCache = leaderboardHistoryCache;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public List<GetLeaderboardHistoryStatistics> GetLeaderboardHistoryStatistics()
		{
			string? firstPath = DataUtils.GetLeaderboardHistoryPaths().OrderBy(p => p).FirstOrDefault();
			if (firstPath == null)
				return new();

			GetLeaderboardHistory current = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(firstPath);

			ulong daggersFiredGlobal = current.DaggersFiredGlobal;
			ulong daggersHitGlobal = current.DaggersHitGlobal;
			ulong deathsGlobal = current.DeathsGlobal;
			ulong gemsGlobal = current.GemsGlobal;
			ulong killsGlobal = current.KillsGlobal;
			double timeGlobal = current.TimeGlobal.ToSecondsTime();
			double rank100 = GetTimeOr0(current, 99);
			double rank10 = GetTimeOr0(current, 9);
			int totalPlayers = current.Players;

			List<GetLeaderboardHistoryStatistics> leaderboardHistoryStatistics = new();
			DateTime dateTime = current.DateTime;
			Add(true, true, true, true, true, true, true, true, true);

			while (dateTime < DateTime.UtcNow)
			{
				dateTime = dateTime.AddDays(7);
				string historyPath = DataUtils.GetLeaderboardHistoryPathFromDate(dateTime);
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

				double currentRank100 = GetTimeOr0(current, 99);
				if (rank100 < currentRank100)
					rank100 = currentRank100;
				rank100Updated = currentRank100 != 0;

				double currentRank10 = GetTimeOr0(current, 9);
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

			static double GetTimeOr0(GetLeaderboardHistory history, int rankIndex)
				=> history.Entries.Count > rankIndex ? history.Entries[rankIndex].Time.ToSecondsTime() : 0;
		}
	}
}
