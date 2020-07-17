using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Utils;
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
		private static readonly StringBuilder log = new StringBuilder();

		public static void SpreadAllHighscoreStats(bool useLogging, bool useConsole)
		{
			Dictionary<string, Leaderboard> leaderboards = GetAllLeaderboards();

			log.Clear();
			foreach (KeyValuePair<string, Leaderboard> kvp in leaderboards)
			{
				SpreadHighscoreStats(leaderboards.Select(kvp => kvp.Value).ToList(), kvp.Value);

				using StreamWriter sw = File.CreateText(kvp.Key);
				sw.Write(JsonConvert.SerializeObject(kvp.Value));
			}

			if (useLogging)
				File.WriteAllText("Results.log", log.ToString());
			if (useConsole)
				Console.WriteLine(log.ToString());
		}

		public static Dictionary<string, Leaderboard> GetAllLeaderboards()
		{
			Dictionary<string, Leaderboard> leaderboards = new Dictionary<string, Leaderboard>();
			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history", "*.json"))
			{
				string jsonString = File.ReadAllText(path, Encoding.UTF8);
				leaderboards.Add(path, JsonConvert.DeserializeObject<Leaderboard>(jsonString));
			}

			return leaderboards;
		}

		public static void SpreadHighscoreStats(List<Leaderboard> leaderboards, Leaderboard leaderboard)
		{
			List<Entry> changes = new List<Entry>();
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
					entry.ShotsHit = entryWithStats.ShotsHit;
					entry.ShotsFired = entryWithStats.ShotsFired;
					changes.Add(entry);
				}
			}

			if (changes.Count != 0)
			{
				log.AppendLine(leaderboard.DateTime.ToString());
				foreach (Entry entry in changes)
				{
					log.AppendLine($"\tSet missing stats for {entry.Username} {entry.Time.FormatTimeInteger()}");
					log.AppendLine($"\t\tGems: {entry.Gems}");
					log.AppendLine($"\t\tKills: {entry.Kills}");
					log.AppendLine($"\t\tDeathType: {GameInfo.GetDeathFromDeathType(entry.DeathType).Name}");
					log.AppendLine($"\t\tAccuracy: {entry.ShotsHit / (float)entry.ShotsFired:00.00%}");
				}

				log.AppendLine();
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

		private static bool IsEmpty(this Entry entry) => entry.Gems == 0 && entry.Kills == 0 && entry.DeathType == -1 && entry.ShotsHit == 0 && entry.ShotsFired == 0;
	}
}