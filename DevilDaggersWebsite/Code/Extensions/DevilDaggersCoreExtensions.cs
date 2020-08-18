﻿using DevilDaggersCore.Game;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.DataTransferObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DevilDaggersWebsite.Code.Extensions
{
	public static class DevilDaggersCoreExtensions
	{
		public static HtmlString ToHtmlData(this Entry entry, string flagCode)
		{
			ulong deaths = entry.DeathsTotal == 0 ? 1 : entry.DeathsTotal;
			return new HtmlString($@"
                rank='{entry.Rank}'
                flag='{flagCode}'
                username='{HttpUtility.HtmlEncode(entry.Username)}'
                time='{entry.Time}'
                kills='{entry.Kills}'
                gems='{entry.Gems}'
                accuracy='{entry.Accuracy * 10000:0}'
                death-type='{GameInfo.GetDeathByType(entry.DeathType)?.Name ?? "Unknown"}'
                total-time='{entry.TimeTotal}'
                total-kills='{entry.KillsTotal}'
                total-gems='{entry.GemsTotal}'
                total-accuracy='{entry.AccuracyTotal * 10000:0}'
                total-deaths='{entry.DeathsTotal}'
                daggers-hit='{entry.DaggersHit}'
                daggers-fired='{entry.DaggersFired}'
                total-daggers-hit='{entry.DaggersHitTotal}'
                total-daggers-fired='{entry.DaggersFiredTotal}'
                average-time='{entry.TimeTotal * 10000f / deaths:0}'
                average-kills='{entry.KillsTotal * 100f / deaths:0}'
                average-gems='{entry.GemsTotal * 100f / deaths:0}'
                average-daggers-hit='{entry.DaggersHitTotal * 100f / deaths:0}'
                average-daggers-fired='{entry.DaggersFiredTotal * 100f / deaths:0}'
                time-by-death='{entry.Time * 10000f / deaths:0}'
            ");
		}

		public static HtmlString ToHtmlData(this Entry entry, string flagCode, Player player) => new HtmlString($@"
			rank='{entry.Rank}'
			flag='{flagCode}'
			username='{HttpUtility.HtmlEncode(entry.Username)}'
			time='{entry.Time}'
			e-dpi='{player.Edpi * 1000 ?? 0}'
			dpi='{player.Dpi ?? 0}'
			in-game-sens='{player.InGameSens * 1000 ?? 0}'
			fov='{player.Fov ?? 0}'
			hand='{(!player.RightHanded.HasValue ? -1 : player.RightHanded.Value ? 1 : 0)}'
			flash='{(!player.FlashEnabled.HasValue ? -1 : player.FlashEnabled.Value ? 1 : 0)}'
			gamma='{player.Gamma ?? 0}'
		");

		public static bool ExistsInHistory(this Entry entry, IWebHostEnvironment env)
		{
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				if (leaderboard.Entries.Any(e => e.Id == entry.Id))
					return true;
			}

			return false;
		}

		public static List<string> GetAllUsernameAliases(this Entry entry, IWebHostEnvironment env)
		{
			Dictionary<string, int> aliases = new Dictionary<string, int>();
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry historyEntry = leaderboard.Entries.FirstOrDefault(e => e.Id == entry.Id);
				if (historyEntry != null)
				{
					if (string.IsNullOrWhiteSpace(historyEntry.Username))
						continue;

					if (aliases.ContainsKey(historyEntry.Username))
						aliases[historyEntry.Username]++;
					else
						aliases.Add(historyEntry.Username, 1);
				}
			}

			return aliases.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
		}

		public static HtmlString ToChangelogHtmlString(this Tool tool)
		{
			StringBuilder sb = new StringBuilder();
			foreach (ChangelogEntry entry in tool.Changelog)
			{
				sb.Append($"<h3>{entry.VersionNumber} - {entry.Date:MMMM dd, yyyy}</h3><ul>");
				foreach (Change change in entry.Changes)
					sb.Append(change.ToHtmlString());
				sb.Append("</ul>");
			}

			return new HtmlString(sb.ToString());
		}

		public static HtmlString ToHtmlString(this Change change)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"<li>{change.Description}</li>");
			if (change.SubChanges != null && change.SubChanges.Count != 0)
			{
				foreach (Change subChange in change.SubChanges)
					sb.Append($"<ul>{subChange.ToHtmlString()}</ul>");
			}

			return new HtmlString(sb.ToString());
		}
	}
}