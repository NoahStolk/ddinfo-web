using DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Utils;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.LeaderboardHistoryStatistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
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
		public List<GetLeaderboardHistoryStatisticsPublic> GetLeaderboardHistoryStatistics()
		{
			List<GetLeaderboardHistoryStatisticsPublic> leaderboardHistoryStatistics = new();

			foreach (string leaderboardHistoryPath in DataUtils.GetLeaderboardHistoryPaths())
			{
				GetLeaderboardHistoryPublic leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				leaderboardHistoryStatistics.Add(new GetLeaderboardHistoryStatisticsPublic
				{
					DateTime = leaderboard.DateTime,
					DaggersFiredGlobal = leaderboard.DaggersFiredGlobal.NullIfDefault(),
					DaggersHitGlobal = leaderboard.DaggersHitGlobal.NullIfDefault(),
					DeathsGlobal = leaderboard.DeathsGlobal.NullIfDefault(),
					GemsGlobal = leaderboard.GemsGlobal.NullIfDefault(),
					KillsGlobal = leaderboard.KillsGlobal.NullIfDefault(),
					TimeGlobal = (leaderboard.TimeGlobal / 10000.0).NullIfDefault(),
					Top100Entrance = leaderboard.Entries.Count > 99 ? leaderboard.Entries[99].Time / 10000.0 : null,
					Top10Entrance = leaderboard.Entries.Count > 9 ? leaderboard.Entries[9].Time / 10000.0 : null,
					TotalPlayers = leaderboard.Players.NullIfDefault(),
				});
			}

			return leaderboardHistoryStatistics;
		}
	}
}
