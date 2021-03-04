using DevilDaggersWebsite.Dto;
using Newtonsoft.Json;
using System;
using System.IO;

namespace LeaderboardJsonFormatter
{
	public static class Program
	{
		public static void Main()
		{
			DateTime fullHistoryDateStart = new(2018, 9, 1);

			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite.Razor\wwwroot\leaderboard-history", "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(path));
				File.WriteAllText(path, JsonConvert.SerializeObject(leaderboard, leaderboard.DateTime > fullHistoryDateStart ? Formatting.None : Formatting.Indented));
			}
		}
	}
}
