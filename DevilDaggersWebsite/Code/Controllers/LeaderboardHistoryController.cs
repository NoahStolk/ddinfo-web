﻿using DevilDaggersWebsite.Code.DataTransferObjects;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Io = System.IO;

namespace DevilDaggersWebsite.Code.Controllers
{
	[Route("api/leaderboard-history")]
	[ApiController]
	public class LeaderboardHistoryController : ControllerBase
	{
		private readonly IWebHostEnvironment env;

		public LeaderboardHistoryController(IWebHostEnvironment env)
		{
			this.env = env;
		}

		[HttpGet("user-progression")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public SortedDictionary<DateTime, Entry> GetUserProgressionById([Required] int userId)
		{
			SortedDictionary<DateTime, Entry> data = new SortedDictionary<DateTime, Entry>();

			if (userId != 0)
			{
				foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
				{
					Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(Io.File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
					Entry entry = leaderboard.Entries.FirstOrDefault(e => e.Id == userId);

					if (entry != null && !data.Values.Any(e =>
						e.Time == entry.Time ||
						e.Time == entry.Time + 1 ||
						e.Time == entry.Time - 1)) // Off-by-one errors in the history
					{
						data[leaderboard.DateTime] = entry;
					}
				}
			}

			return data;
		}

		[HttpGet("world-records")]
		[ProducesResponseType(200)]
		public List<WorldRecord> GetWorldRecords(DateTime? date = null)
			=> LeaderboardHistoryUtils.GetWorldRecords(env, date);

		[HttpGet("latest-date-played")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public (DateTime from, DateTime to) GetLatestDatePlayed([Required] int userId)
		{
			List<(DateTime dateTime, Entry entry)> entries = new List<(DateTime, Entry)>();
			foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard>(Io.File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry entry = lb.Entries.FirstOrDefault(e => e.Id == userId);
				if (entry != null)
					entries.Add((lb.DateTime, entry));
			}

			entries = entries.OrderByDescending(l => l.dateTime).ToList();
			ulong deaths = entries[0].entry.DeathsTotal;
			for (int i = 1; i < entries.Count; i++)
			{
				if (entries[i].entry.DeathsTotal < deaths)
					return (entries[i].dateTime, entries[i - 1].dateTime);
			}

			return (DateTime.Now, DateTime.Now);
		}

		[HttpGet("user-activity")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public Dictionary<DateTime, ulong> GetUserActivity([Required] int userId)
		{
			Dictionary<DateTime, ulong> data = new Dictionary<DateTime, ulong>();
			foreach (string leaderboardHistoryPath in Io.Directory.GetFiles(Io.Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard>(Io.File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry entry = lb.Entries.FirstOrDefault(e => e.Id == userId);
				if (entry != null && entry.DeathsTotal > 0)
					data.Add(lb.DateTime, entry.DeathsTotal);
			}

			return data;
		}
	}
}