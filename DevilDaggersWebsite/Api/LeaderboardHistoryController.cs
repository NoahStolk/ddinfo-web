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
using System.Text;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/leaderboard-history")]
	[ApiController]
	public class LeaderboardHistoryController : ControllerBase
	{
		private readonly IWebHostEnvironment _env;
		private readonly LeaderboardHistoryHelper _leaderboardHistoryHelper;

		public LeaderboardHistoryController(IWebHostEnvironment env, LeaderboardHistoryHelper leaderboardHistoryHelper)
		{
			_env = env;
			_leaderboardHistoryHelper = leaderboardHistoryHelper;
		}

		[HttpGet("user-progression")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public SortedDictionary<DateTime, Entry> GetUserProgressionById([Required] int userId)
		{
			SortedDictionary<DateTime, Entry> data = new();
			if (userId < 1)
				return data;

			foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(Io.File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
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
		public List<WorldRecord> GetWorldRecords()
			=> _leaderboardHistoryHelper.GetWorldRecords();

		[HttpGet("latest-date-played")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public (DateTime From, DateTime To) GetLatestDatePlayed([Required] int userId)
		{
			List<(DateTime DateTime, Entry Entry)> entries = new();
			foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard>(Io.File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry? entry = lb.Entries.Find(e => e.Id == userId);
				if (entry != null)
					entries.Add((lb.DateTime, entry));
			}

			entries = entries.OrderByDescending(l => l.DateTime).ToList();
			ulong deaths = entries[0].Entry.DeathsTotal;
			for (int i = 1; i < entries.Count; i++)
			{
				if (entries[i].Entry.DeathsTotal < deaths)
					return (entries[i].DateTime, entries[i - 1].DateTime);
			}

			return (DateTime.UtcNow, DateTime.UtcNow);
		}

		[HttpGet("user-activity")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public Dictionary<DateTime, ulong> GetUserActivity([Required] int userId)
		{
			Dictionary<DateTime, ulong> data = new();
			foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard>(Io.File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry? entry = lb.Entries.Find(e => e.Id == userId);
				if (entry?.DeathsTotal > 0)
					data.Add(lb.DateTime, entry.DeathsTotal);
			}

			return data;
		}
	}
}
