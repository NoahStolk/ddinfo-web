using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using NetBase.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LeaderboardJsonHighscoreStatsSpreader
{
	public static class Program
	{
		public static void Main()
		{
			FixHighscoreStats();
		}

		private static void FixHighscoreStats()
		{
			Dictionary<string, Leaderboard> leaderboards = new Dictionary<string, Leaderboard>();
			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history", "*.json"))
			{
				string jsonString = FileUtils.GetContents(path, Encoding.UTF8);
				leaderboards.Add(path, JsonConvert.DeserializeObject<Leaderboard>(jsonString));
			}

			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, Leaderboard> kvp in leaderboards)
			{
				List<Entry> changes = new List<Entry>();
				foreach (Entry entry in kvp.Value.Entries)
				{
					if (entry.IsEmpty())
					{
						KeyValuePair<string, Leaderboard> leaderboardWithStats = leaderboards.FirstOrDefault(l => l.Value.Entries.Any(e => e.Id == entry.Id && e.Time >= entry.Time - 1 && e.Time <= entry.Time + 1 && !e.IsEmpty()));
						if (leaderboardWithStats.Value == null)
							continue;
						Entry entryWithStats = leaderboardWithStats.Value.Entries.FirstOrDefault(e => e.Id == entry.Id);
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
					sb.AppendLine(kvp.Value.DateTime.ToString());
					foreach (Entry entry in changes)
					{
						sb.AppendLine($"\tSet missing stats for {entry.Username} {entry.Time / 10000f:0.0000}");
						sb.AppendLine($"\t\tGems: {entry.Gems}");
						sb.AppendLine($"\t\tKills: {entry.Kills}");
						sb.AppendLine($"\t\tDeathType: {GameInfo.GetDeathFromDeathType(entry.DeathType).Name}");
						sb.AppendLine($"\t\tAccuracy: {entry.ShotsHit / (float)entry.ShotsFired:00.00%}");
					}

					sb.AppendLine();
				}

				using StreamWriter sw = File.CreateText(kvp.Key);
				sw.Write(JsonConvert.SerializeObject(kvp.Value));
			}

			File.WriteAllText("Results.log", sb.ToString());
		}

		private static bool IsEmpty(this Entry entry) => entry.Gems == 0 || entry.Kills == 0 || entry.DeathType == -1 || entry.ShotsHit == 0 || entry.ShotsFired == 0;
	}
}