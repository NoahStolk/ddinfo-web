﻿using DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardStatistics;
using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Utils;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardStatistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
{
	[Route("api/leaderboard-statistics")]
	[ApiController]
	public class LeaderboardStatisticsController : ControllerBase
	{
		private readonly LeaderboardStatisticsCache _leaderboardStatisticsCache;

		public LeaderboardStatisticsController(LeaderboardStatisticsCache leaderboardStatisticsCache)
		{
			_leaderboardStatisticsCache = leaderboardStatisticsCache;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public ActionResult<GetLeaderboardStatisticsPublic> GetStatistics()
		{
			return new GetLeaderboardStatisticsPublic
			{
				DateTime = HistoryUtils.HistoryJsonFileNameToDateTime(_leaderboardStatisticsCache.FileName),
				IsFetched = _leaderboardStatisticsCache.IsFetched,
				TotalEntries = _leaderboardStatisticsCache.Entries.Count,
				DaggerStatistics = _leaderboardStatisticsCache.DaggerStats.OrderBy(kvp => kvp.Key.UnlockSecond).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value),
				DeathStatistics = _leaderboardStatisticsCache.DeathStats.OrderBy(kvp => kvp.Key.DeathType).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value),
				EnemyStatistics = _leaderboardStatisticsCache.EnemyStats.OrderByDescending(kvp => kvp.Value).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value),
				TimeStatistics = _leaderboardStatisticsCache.TimeStats.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
				KillStatistics = _leaderboardStatisticsCache.KillStats.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
				GemStatistics = _leaderboardStatisticsCache.GemStats.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
				PlayersWithLevel1 = _leaderboardStatisticsCache.Level1,
				PlayersWithLevel2 = _leaderboardStatisticsCache.Level2,
				PlayersWithLevel3Or4 = _leaderboardStatisticsCache.Level3Or4,
				Time = _leaderboardStatisticsCache.Time,
				Kills = _leaderboardStatisticsCache.Kills,
				Gems = _leaderboardStatisticsCache.Gems,
			};
		}
	}
}
