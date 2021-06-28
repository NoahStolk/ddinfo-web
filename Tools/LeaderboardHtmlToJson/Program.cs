using DevilDaggersCore.Game;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Utils;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace LeaderboardHtmlToJson
{
	public static class Program
	{
		public static void Main()
		{
			const string dateString = "20180902073511";
			File.WriteAllText($"{dateString.Substring(0, 12)}.json", JsonConvert.SerializeObject(GetLeaderboardFromHtml(dateString)), Encoding.UTF8);
		}

		public static Leaderboard GetLeaderboardFromHtml(string dateString)
		{
			Leaderboard lb = new()
			{
				DateTime = HistoryUtils.HistoryJsonFileNameToDateTime(dateString),
				// TODO
				Players = 171352,
				TimeGlobal = 10053064700403,
				KillsGlobal = 2599153686,
				GemsGlobal = 293243405,
				DeathsGlobal = 14380029,
				DaggersHitGlobal = 2178,
				DaggersFiredGlobal = 10000,
			};

			string[] lines = File.ReadAllLines(Path.Combine("Content", $"{dateString}.html"), Encoding.UTF8);

			for (int i = 0; i < lines.Length; i++)
			{
				lines[i] = lines[i].TrimStart('\t');
				if (lines[i].StartsWith("<div class=\"sort\""))
				{
					lb.Entries.Add(new Entry
					{
						Rank = int.Parse(GetValue(lines[i], "rank")),
						Username = GetValue(lines[i], "username"),
						Time = int.Parse(GetValue(lines[i], "time")),
						Kills = int.Parse(GetValue(lines[i], "kills")),
						Gems = int.Parse(GetValue(lines[i], "gems")),
						DaggersFired = 10000,
						DaggersHit = int.Parse(GetValue(lines[i], "accuracy")),
						DeathType = (short)(GameInfo.GetDeathByName(GameInfo.GetGameVersionFromDate(lb.DateTime) ?? GameVersion.V1, GetValue(lines[i], "death-type"))?.DeathType ?? -1),
						TimeTotal = ulong.Parse(GetValue(lines[i], "total-time")),
						KillsTotal = ulong.Parse(GetValue(lines[i], "total-kills")),
						GemsTotal = ulong.Parse(GetValue(lines[i], "total-gems")),
						DaggersFiredTotal = 10000,
						DaggersHitTotal = ulong.Parse(GetValue(lines[i], "total-accuracy")),
						DeathsTotal = ulong.Parse(GetValue(lines[i], "total-deaths")),
					});
				}
			}

			return lb;
		}

		public static string GetValue(string line, string name)
		{
			if (!line.Contains(name))
				throw new($"Line does not contain '{name}'.");

			int pos = line.IndexOf(name) + name.Length + 2;
			string sub = line[pos..];
			return sub.Substring(0, sub.IndexOf('"'));
		}
	}
}
