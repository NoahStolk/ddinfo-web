using CoreBase3.Services;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Leaderboards.History;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBase.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class HistoryModel : PageModel
	{
		private readonly ICommonObjects commonObjects;

		public Lb Leaderboard { get; set; } = new Lb();
		public Lb LeaderboardPrevious { get; set; } = new Lb();
		public List<string> ChangesGlobal { get; private set; } = new List<string>();
		public Dictionary<string, string> ChangesTop100 { get; private set; } = new Dictionary<string, string>();

		public List<SelectListItem> JsonFiles { get; set; } = new List<SelectListItem>();
		public string From { get; set; }
		public string FromPrevious { get; set; }
		public string FromNext { get; set; }
		public bool ShowMoreStats { get; private set; }

		public HistoryModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;

			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(this.commonObjects.Env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Lb leaderboard = JsonConvert.DeserializeObject<Lb>(FileUtils.GetContents(leaderboardHistoryPath, Encoding.UTF8));
				JsonFiles.Add(new SelectListItem($"{HistoryUtils.HistoryJsonFileNameToDateString(Path.GetFileNameWithoutExtension(leaderboardHistoryPath))} ({leaderboard.GetCompletionRate():0.0%} complete)", Path.GetFileName(leaderboardHistoryPath)));
			}

			JsonFiles.Reverse();
		}

		public void OnGet(string from, bool showMoreStats)
		{
			From = from;
			if (string.IsNullOrEmpty(From) || !System.IO.File.Exists(Path.Combine(commonObjects.Env.WebRootPath, "leaderboard-history", From)))
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
				}
			}

			Leaderboard = JsonConvert.DeserializeObject<Lb>(FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "leaderboard-history", From), Encoding.UTF8));

			if (FromNext != null)
			{
				LeaderboardPrevious = JsonConvert.DeserializeObject<Lb>(FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "leaderboard-history", FromNext), Encoding.UTF8));

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
						Entry entryPrevious = LeaderboardPrevious.Entries.FirstOrDefault(e => e.Id == entry.Id);
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