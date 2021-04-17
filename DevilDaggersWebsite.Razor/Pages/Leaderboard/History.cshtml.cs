using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Caches.LeaderboardHistory;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Io = System.IO;
using Lb = DevilDaggersWebsite.Dto.Leaderboard;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class HistoryModel : PageModel, IDefaultLeaderboardPageModel
	{
		private static readonly DateTime _fullHistoryDateStart = new(2018, 10, 1);

		private readonly IWebHostEnvironment _env;

		public HistoryModel(IWebHostEnvironment env)
		{
			_env = env;

			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				string listItemText = HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(leaderboardHistoryPath)).ToString(FormatUtils.DateTimeUtcFormat);
				JsonFiles.Add(new SelectListItem(listItemText, Path.GetFileName(leaderboardHistoryPath)));
			}

			JsonFiles.Reverse();
		}

		public Lb? Leaderboard { get; set; } = new();
		public Lb LeaderboardPrevious { get; set; } = new();
		public List<string> ChangesGlobal { get; } = new();
		public Dictionary<string, string> ChangesTop100 { get; } = new();

		public List<SelectListItem> JsonFiles { get; } = new();
		public string? From { get; set; }
		public string? FromPrevious { get; set; }
		public string? FromNext { get; set; }

		public void OnGet(string from)
		{
			From = from;
			if (string.IsNullOrEmpty(From) || !Io.File.Exists(Path.Combine(_env.WebRootPath, "leaderboard-history", From)))
				From = JsonFiles[0].Value;

			for (int i = 0; i < JsonFiles.Count; i++)
			{
				if (From == JsonFiles[i].Value)
				{
					if (i != 0)
						FromPrevious = JsonFiles[i - 1].Value;
					if (i != JsonFiles.Count - 1)
						FromNext = JsonFiles[i + 1].Value;

					break;
				}
			}

			Leaderboard = LeaderboardHistoryCache.Instance.GetLeaderboardHistoryByFilePath(Path.Combine(_env.WebRootPath, "leaderboard-history", From));

			if (FromNext != null && Leaderboard.DateTime > _fullHistoryDateStart)
			{
				LeaderboardPrevious = LeaderboardHistoryCache.Instance.GetLeaderboardHistoryByFilePath(Path.Combine(_env.WebRootPath, "leaderboard-history", FromNext));

				if (LeaderboardPrevious.Players != Leaderboard.Players)
					ChangesGlobal.Add($"Total players +{Leaderboard.Players - LeaderboardPrevious.Players}");

				if (LeaderboardPrevious.DeathsGlobal != Leaderboard.DeathsGlobal)
					ChangesGlobal.Add($"Global deaths +{Leaderboard.DeathsGlobal - LeaderboardPrevious.DeathsGlobal}\n");

				List<Entry> top100Joins = new();
				List<Entry> highscores = new();
				foreach (Entry entry in Leaderboard.Entries)
				{
					Entry? entryPrevious = LeaderboardPrevious.Entries.Find(e => e.Id == entry.Id);
					if (entryPrevious == null)
					{
						top100Joins.Add(entry);
						highscores.Add(entry);
					}
					else
					{
						if (entry.Time != entryPrevious.Time)
							highscores.Add(entry);
					}
				}

				if (top100Joins.Count != 0)
					ChangesTop100[$"{top100Joins.Count} player{top100Joins.Count.S()} joined top 100"] = string.Join(", ", top100Joins.Select(e => e.Username));
				if (highscores.Count != 0)
					ChangesTop100[$"{highscores.Count} player{highscores.Count.S()} set a new highscore"] = string.Join(", ", highscores.Select(e => e.Username));
			}
		}
	}
}
