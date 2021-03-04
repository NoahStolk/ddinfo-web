using DevilDaggersCore.Game;
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ToolsShared
{
	public static class HighscoreSpreadUtils
	{
		private static readonly StringBuilder _log = new();

		public static void SpreadAllHighscoreStats(bool writeLogToFile, bool useConsole)
		{
			Dictionary<string, Leaderboard> leaderboards = GetAllLeaderboards();

			_log.Clear();
			foreach (KeyValuePair<string, Leaderboard> kvp in leaderboards)
			{
				SpreadHighscoreStats(leaderboards.Select(kvp => kvp.Value).ToList(), kvp.Value);
				File.WriteAllText(kvp.Key, JsonConvert.SerializeObject(kvp.Value));
			}

			if (writeLogToFile)
				File.WriteAllText("Results.log", _log.ToString());
			if (useConsole)
				Console.WriteLine(_log.ToString());
		}

		public static Dictionary<string, Leaderboard> GetAllLeaderboards()
		{
			Dictionary<string, Leaderboard> leaderboards = new();
			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite.Razor\wwwroot\leaderboard-history", "*.json"))
			{
				string jsonString = File.ReadAllText(path, Encoding.UTF8);
				leaderboards.Add(path, JsonConvert.DeserializeObject<Leaderboard>(jsonString));
			}

			return leaderboards;
		}

		public static void SpreadHighscoreStats(List<Leaderboard> leaderboards, Leaderboard leaderboard)
		{
			List<Entry> changes = new();
			foreach (Entry entry in leaderboard.Entries)
			{
				if (entry.Id != 0 && entry.IsEmpty())
				{
					Leaderboard leaderboardWithStats = leaderboards.FirstOrDefault(l => l.Entries.Any(e => e.Id == entry.Id && e.Time >= entry.Time - 1 && e.Time <= entry.Time + 1 && !e.IsEmpty())); // TODO: Get most complete data.
					if (leaderboardWithStats == null)
						continue;
					Entry entryWithStats = leaderboardWithStats.Entries.FirstOrDefault(e => e.Id == entry.Id);
					entry.Gems = entryWithStats.Gems;
					entry.Kills = entryWithStats.Kills;
					entry.DeathType = entryWithStats.DeathType;
					entry.DaggersHit = entryWithStats.DaggersHit;
					entry.DaggersFired = entryWithStats.DaggersFired;
					changes.Add(entry);
				}
			}

			if (changes.Count != 0)
			{
				_log.AppendLine(leaderboard.DateTime.ToString());
				foreach (Entry entry in changes)
				{
					_log.AppendLine($"\tSet missing stats for {entry.Username} {entry.Time.FormatTimeInteger()}");
					_log.AppendLine($"\t\tGems: {entry.Gems}");
					_log.AppendLine($"\t\tKills: {entry.Kills}");
					_log.AppendLine($"\t\tDeathType: {GameInfo.GetDeathByType(GameInfo.GetGameVersionFromDate(leaderboard.DateTime) ?? GameVersion.V1, entry.DeathType).Name}");
					_log.AppendLine($"\t\tAccuracy: {entry.DaggersHit / (float)entry.DaggersFired:00.00%}");
				}

				_log.AppendLine();
			}
		}

		public static Entry GetEntryWithData(List<Leaderboard> leaderboards, Entry entry, int id, int time)
		{
			if (id == 0 || !entry.IsEmpty())
				return null;

			Leaderboard leaderboardWithStats = leaderboards.FirstOrDefault(l => l.Entries.Any(e => e.Id == id && e.Time >= time - 1 && e.Time <= time + 1 && !e.IsEmpty())); // TODO: Get most complete data.
			if (leaderboardWithStats == null)
				return null;

			return leaderboardWithStats.Entries.FirstOrDefault(e => e.Id == id);
		}

		private static bool IsEmpty(this Entry entry)
			=> entry.Gems == 0 && entry.Kills == 0 && entry.DeathType == -1 && entry.DaggersHit == 0 && entry.DaggersFired == 0;
	}
}
