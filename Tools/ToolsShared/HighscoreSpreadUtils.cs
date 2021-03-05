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
		private static readonly DateTime _fullHistoryDateStart = new(2018, 10, 1);

		private static readonly StringBuilder _log = new();

		public static void SpreadAllHighscoreStats(bool writeLogToFile, bool useConsole)
		{
			Dictionary<string, Leaderboard> leaderboards = GetAllLeaderboards();

			_log.Clear();
			foreach (KeyValuePair<string, Leaderboard> kvp in leaderboards)
			{
				SpreadHighscoreStats(leaderboards.Select(kvp => kvp.Value).ToList(), kvp.Value);
				File.WriteAllText(kvp.Key, JsonConvert.SerializeObject(kvp.Value, kvp.Value.DateTime > _fullHistoryDateStart ? Formatting.None : Formatting.Indented));
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
				if (entry.Id != 0 && entry.HasMissingStats())
				{
					IEnumerable<Leaderboard> leaderboardsWithStats = leaderboards.Where(l => l.Entries.Any(e => e.Id == entry.Id && e.Time >= entry.Time - 1 && e.Time <= entry.Time + 1));
					if (!leaderboardsWithStats.Any())
						continue;

					IEnumerable<Entry> entries = leaderboardsWithStats.SelectMany(lb => lb.Entries).Where(e => e.Id == entry.Id);

					Combine(entry, entries);

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
					_log.AppendLine($"\t\tDeathType: {GameInfo.GetDeathByType(GameInfo.GetGameVersionFromDate(leaderboard.DateTime) ?? GameVersion.V1, entry.DeathType)?.Name ?? "Unknown"}");
					_log.AppendLine($"\t\tAccuracy: {entry.DaggersHit / (float)entry.DaggersFired:00.00%}");
				}

				_log.AppendLine();
			}
		}

		public static bool HasMissingStats(this Entry entry)
			=> entry.Gems == 0 || entry.Kills == 0 || entry.DeathType == -1 || entry.DaggersHit == 0 || entry.DaggersFired == 0 || entry.DaggersFired == 10000;

		private static void Combine(Entry original, IEnumerable<Entry> entries)
		{
			Entry? withGems = entries.FirstOrDefault(e => e.Gems != 0);
			if (withGems != null)
				original.Gems = withGems.Gems;

			Entry? withKills = entries.FirstOrDefault(e => e.Kills != 0);
			if (withKills != null)
				original.Kills = withKills.Kills;

			Entry? withDeathType = entries.FirstOrDefault(e => e.DeathType != -1);
			if (withDeathType != null)
				original.DeathType = withDeathType.DeathType;

			Entry? withFullDaggerStats = entries.FirstOrDefault(e => e.DaggersFired != 0 && e.DaggersFired != 10000);
			if (withFullDaggerStats != null)
			{
				original.DaggersHit = withFullDaggerStats.DaggersHit;
				original.DaggersFired = withFullDaggerStats.DaggersFired;
			}
			else
			{
				Entry? withPartialDaggerStats = entries.FirstOrDefault(e => e.DaggersFired != 0);
				if (withPartialDaggerStats != null)
				{
					original.DaggersHit = withPartialDaggerStats.DaggersHit;
					original.DaggersFired = withPartialDaggerStats.DaggersFired;
				}
			}
		}
	}
}
