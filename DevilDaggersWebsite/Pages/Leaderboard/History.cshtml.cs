using CoreBase.Services;
using DevilDaggersCore.Leaderboards;
using DevilDaggersUtilities.Website;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DevilDaggersWebsite.Pages.Leaderboard
{
	public class HistoryModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public DevilDaggersCore.Leaderboards.Leaderboard Leaderboard { get; set; } = new DevilDaggersCore.Leaderboards.Leaderboard();
		public DevilDaggersCore.Leaderboards.Leaderboard LeaderboardPrevious { get; set; }
		public StringBuilder ChangesGlobal { get; private set; }
		public StringBuilder ChangesTop100 { get; private set; }

		public List<SelectListItem> JsonFiles { get; set; } = new List<SelectListItem>();
		public string From { get; set; }
		public string FromPrevious { get; set; }
		public string FromNext { get; set; }

		public HistoryModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;

			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				DevilDaggersCore.Leaderboards.Leaderboard leaderboard = JsonConvert.DeserializeObject<DevilDaggersCore.Leaderboards.Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath, Encoding.UTF8));
				JsonFiles.Add(new SelectListItem($"{LeaderboardHistoryUtils.HistoryJsonFileNameToDateString(Path.GetFileNameWithoutExtension(leaderboardHistoryPath))} UTC ({leaderboard.GetCompletionRate().ToString("0.0%")} complete)", Path.GetFileName(leaderboardHistoryPath)));
			}

			JsonFiles.Reverse();
		}

		public void OnGet(string from)
		{
			From = from;
			if (string.IsNullOrEmpty(From))
				From = JsonFiles[0].Value;

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

			string jsonString;
			try
			{
				jsonString = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history", From), Encoding.UTF8);
			}
			catch (Exception)
			{
				From = JsonFiles[0].Value;
				jsonString = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history", From), Encoding.UTF8);
			}
			Leaderboard = JsonConvert.DeserializeObject<DevilDaggersCore.Leaderboards.Leaderboard>(jsonString);

			ChangesGlobal = new StringBuilder();
			ChangesTop100 = new StringBuilder();
			if (FromNext != null)
			{
				jsonString = FileUtils.GetContents(Path.Combine(_commonObjects.Env.WebRootPath, "leaderboard-history", FromNext), Encoding.UTF8);
				LeaderboardPrevious = JsonConvert.DeserializeObject<DevilDaggersCore.Leaderboards.Leaderboard>(jsonString);

				if (LeaderboardPrevious.GetCompletionRate() > 0.999f && Leaderboard.GetCompletionRate() > 0.999f)
				{
					if (LeaderboardPrevious.Players != Leaderboard.Players)
						ChangesGlobal.AppendLine($"Total players +{Leaderboard.Players - LeaderboardPrevious.Players}");

					if (LeaderboardPrevious.DeathsGlobal != Leaderboard.DeathsGlobal)
						ChangesGlobal.AppendLine($"Global deaths +{Leaderboard.DeathsGlobal - LeaderboardPrevious.DeathsGlobal}\n");

					List<Entry> top100joins = new List<Entry>();
					List<Entry> highscores = new List<Entry>();
					List<Entry> plays = new List<Entry>();
					foreach (Entry entry in Leaderboard.Entries)
					{
						Entry entryPrevious = LeaderboardPrevious.Entries.Where(e => e.ID == entry.ID).FirstOrDefault();
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
						ChangesTop100.AppendLine($"{top100joins.Count} player{top100joins.Count.S()} joined top 100");
					if (highscores.Count != 0)
						ChangesTop100.AppendLine($"{highscores.Count} player{highscores.Count.S()} set a new highscore");
					if (plays.Count != 0)
						ChangesTop100.AppendLine($"{plays.Count} player{plays.Count.S()} played");
					//if (top100s.Count != 0)
					//	Changes.AppendLine($"{top100s.Count} player{(top100s.Count == 1 ? "" : "s")} joined top 100:\n{string.Join(", ", top100s.Select(e => e.Username))}\n");
					//if (highscores.Count != 0)
					//	Changes.AppendLine($"{highscores.Count} player{(top100s.Count == 1 ? "" : "s")} set a new highscore:\n{string.Join(", ", highscores.Select(e => e.Username))}\n");
					//if (plays.Count != 0)
					//	Changes.AppendLine($"{plays.Count} player{(top100s.Count == 1 ? "" : "s")} played:\n{string.Join(", ", plays.Select(e => e.Username))}\n");
				}
			}
		}
	}
}