using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Io = System.IO;
using Lb = DevilDaggersWebsite.Dto.Leaderboard;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class HistoryModel : PageModel, IDefaultLeaderboardPage
	{
		private readonly IWebHostEnvironment _env;

		public HistoryModel(IWebHostEnvironment env)
		{
			_env = env;

			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				string listItemText = HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(leaderboardHistoryPath)).ToString(FormatUtils.DateTimeUtcFormat, CultureInfo.InvariantCulture);
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
		public bool ShowMoreStats { get; private set; }

		public void OnGet(string from, bool showMoreStats)
		{
			From = from;
			if (string.IsNullOrEmpty(From) || !Io.File.Exists(Path.Combine(_env.WebRootPath, "leaderboard-history", From)))
				From = JsonFiles[0].Value;
			ShowMoreStats = showMoreStats;

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

			Leaderboard = JsonConvert.DeserializeObject<Lb>(Io.File.ReadAllText(Path.Combine(_env.WebRootPath, "leaderboard-history", From), Encoding.UTF8));

			if (FromNext != null)
			{
				LeaderboardPrevious = JsonConvert.DeserializeObject<Lb>(Io.File.ReadAllText(Path.Combine(_env.WebRootPath, "leaderboard-history", FromNext), Encoding.UTF8));

				if (LeaderboardPrevious.GetCompletionRate() > 0.999f && Leaderboard.GetCompletionRate() > 0.999f)
				{
					if (LeaderboardPrevious.Players != Leaderboard.Players)
						ChangesGlobal.Add($"Total players +{Leaderboard.Players - LeaderboardPrevious.Players}");

					if (LeaderboardPrevious.DeathsGlobal != Leaderboard.DeathsGlobal)
						ChangesGlobal.Add($"Global deaths +{Leaderboard.DeathsGlobal - LeaderboardPrevious.DeathsGlobal}\n");

					List<Entry> top100joins = new List<Entry>();
					List<Entry> highscores = new List<Entry>();
					List<Entry> plays = new List<Entry>();
					foreach (Entry entry in Leaderboard.Entries)
					{
						Entry? entryPrevious = LeaderboardPrevious.Entries.Find(e => e.Id == entry.Id);
						if (entryPrevious == null)
						{
							top100joins.Add(entry);
							highscores.Add(entry);
							plays.Add(entry);
						}
						else
						{
							if (entry.Time != entryPrevious.Time)
								highscores.Add(entry);
							if (entry.DeathsTotal != entryPrevious.DeathsTotal)
								plays.Add(entry);
						}
					}

					if (top100joins.Count != 0)
						ChangesTop100[$"{top100joins.Count} player{top100joins.Count.S()} joined top 100"] = string.Join(", ", top100joins.Select(e => e.Username));
					if (highscores.Count != 0)
						ChangesTop100[$"{highscores.Count} player{highscores.Count.S()} set a new highscore"] = string.Join(", ", highscores.Select(e => e.Username));
					if (plays.Count != 0)
						ChangesTop100[$"{plays.Count} player{plays.Count.S()} played"] = string.Join(", ", plays.Select(e => e.Username));
				}
			}
		}
	}
}