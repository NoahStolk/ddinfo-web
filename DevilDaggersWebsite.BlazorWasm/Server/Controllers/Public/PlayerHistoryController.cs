using DevilDaggersWebsite.BlazorWasm.Server.Caches.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Utils;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.LeaderboardHistory;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.PlayerHistory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Public
{
	[Route("api/player-history")]
	[ApiController]
	public class PlayerHistoryController : ControllerBase
	{
		private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

		public PlayerHistoryController(LeaderboardHistoryCache leaderboardHistoryCache)
		{
			_leaderboardHistoryCache = leaderboardHistoryCache;
		}

		[HttpGet("progression")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public List<GetEntryHistory> GetPlayerProgressionById([Required, Range(1, 9999999)] int playerId)
		{
			List<GetEntryHistory> data = new();

			foreach (string leaderboardHistoryPath in DataUtils.GetLeaderboardHistoryPaths())
			{
				GetLeaderboardHistory leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				GetEntryHistory? entry = leaderboard.Entries.Find(e => e.Id == playerId);

				// + 1 and - 1 are used to fix off-by-one errors in the history based on screenshots and videos. This is due to a rounding error in Devil Daggers itself.
				if (entry != null && !data.Any(e =>
					e.Time == entry.Time ||
					e.Time == entry.Time + 1 ||
					e.Time == entry.Time - 1))
				{
					data.Add(entry);
				}
			}

			return data;
		}

		[HttpGet("activity")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public List<GetPlayerActivity> GetPlayerActivityById([Required, Range(1, 9999999)] int playerId)
		{
			List<GetPlayerActivity> data = new();
			foreach (string leaderboardHistoryPath in DataUtils.GetLeaderboardHistoryPaths())
			{
				GetLeaderboardHistory leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				GetEntryHistory? entry = leaderboard.Entries.Find(e => e.Id == playerId);
				if (entry?.DeathsTotal > 0)
				{
					data.Add(new()
					{
						DateTime = leaderboard.DateTime,
						DeathsTotal = entry.DeathsTotal,
					});
				}
			}

			return data;
		}
	}
}
