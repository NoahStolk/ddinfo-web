using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Io = System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
{
	[Route("api/leaderboard-history")]
	[ApiController]
	public class LeaderboardHistoryController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly WorldRecordsHelper _worldRecordsHelper;
		private readonly LeaderboardHistoryCache _leaderboardHistoryCache;

		public LeaderboardHistoryController(IWebHostEnvironment environment, WorldRecordsHelper worldRecordsHelper, LeaderboardHistoryCache leaderboardHistoryCache)
		{
			_environment = environment;
			_worldRecordsHelper = worldRecordsHelper;
			_leaderboardHistoryCache = leaderboardHistoryCache;
		}

		[HttpGet("user-progression")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public SortedDictionary<DateTime, Entry> GetUserProgressionById([Required] int userId)
		{
			SortedDictionary<DateTime, Entry> data = new();
			if (userId < 1)
				return data;

			foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_environment.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				Entry? entry = leaderboard.Entries.Find(e => e.Id == userId);

				// + 1 and - 1 are used to fix off-by-one errors in the history based on screenshots and videos. This is due to a rounding error in Devil Daggers itself.
				if (entry != null && !data.Values.Any(e =>
					e.Time == entry.Time ||
					e.Time == entry.Time + 1 ||
					e.Time == entry.Time - 1))
				{
					data[leaderboard.DateTime] = entry;
				}
			}

			return data;
		}

		[HttpGet("world-records")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public List<WorldRecord> GetWorldRecords()
			=> _worldRecordsHelper.GetWorldRecords();

		[HttpGet("user-activity")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Website)]
		public Dictionary<DateTime, ulong> GetUserActivity([Required] int userId)
		{
			Dictionary<DateTime, ulong> data = new();
			foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_environment.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = _leaderboardHistoryCache.GetLeaderboardHistoryByFilePath(leaderboardHistoryPath);
				Entry? entry = leaderboard.Entries.Find(e => e.Id == userId);
				if (entry?.DeathsTotal > 0)
					data.Add(leaderboard.DateTime, entry.DeathsTotal);
			}

			return data;
		}
	}
}
