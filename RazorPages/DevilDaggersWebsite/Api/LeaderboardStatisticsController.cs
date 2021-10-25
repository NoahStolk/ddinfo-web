﻿using DevilDaggersWebsite.Caches.LeaderboardStatistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Api
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

		[HttpGet("daggers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public Dictionary<string, int> GetDaggers()
			=> _leaderboardStatisticsCache.DaggerStats.OrderBy(kvp => kvp.Key.UnlockSecond).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value);

		[HttpGet("death-types")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public Dictionary<string, int> GetDeathTypes()
			=> _leaderboardStatisticsCache.DeathStats.OrderBy(kvp => kvp.Key.DeathType).ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value);

		[HttpGet("times")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public Dictionary<int, int> GetTimes()
			=> _leaderboardStatisticsCache.TimeStats;
	}
}
