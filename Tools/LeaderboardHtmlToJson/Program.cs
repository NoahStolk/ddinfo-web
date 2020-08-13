using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Utils;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;

namespace LeaderboardHtmlToJson
{
	public class Program
	{
		public static void Main()
		{
			string dateString = "20180902073511";
			File.WriteAllText($"{dateString.Substring(0, 12)}.json", JsonConvert.SerializeObject(GetLeaderboardFromHTML(dateString)), Encoding.UTF8);
		}

		public static Leaderboard GetLeaderboardFromHTML(string dateString)
		{
			Leaderboard lb = new Leaderboard
			{
				DateTime = HistoryUtils.HistoryJsonFileNameToDateTime(dateString),
				// TODO
				Players = 171352,
				TimeGlobal = 10053064700403,
				KillsGlobal = 2599153686,
				GemsGlobal = 293243405,
				DeathsGlobal = 14380029,
				ShotsHitGlobal = 2178,
				ShotsFiredGlobal = 10000
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
						ShotsFired = 10000,
						ShotsHit = int.Parse(GetValue(lines[i], "accuracy")),
						DeathType = GameInfo.GetEntities<Death>().FirstOrDefault(d => d.Name.ToLower() == GetValue(lines[i], "death-type").ToLower()).DeathType,
						TimeTotal = ulong.Parse(GetValue(lines[i], "total-time")),
						KillsTotal = ulong.Parse(GetValue(lines[i], "total-kills")),
						GemsTotal = ulong.Parse(GetValue(lines[i], "total-gems")),
						ShotsFiredTotal = 10000,
						ShotsHitTotal = ulong.Parse(GetValue(lines[i], "total-accuracy")),
						DeathsTotal = ulong.Parse(GetValue(lines[i], "total-deaths"))
					});
				}
			}

			return lb;
		}

		public static string GetValue(string line, string name)
		{
			if (!line.Contains(name))
				return null;

			int pos = line.IndexOf(name) + name.Length + 2;
			string sub = line.Substring(pos);
			return sub.Substring(0, sub.IndexOf('"'));
		}
	}
}