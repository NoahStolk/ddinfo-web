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
			DateTime fullHistoryDateStart = new(2018, 10, 1);

			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite.Razor\wwwroot\leaderboard-history", "*.json"))
			{
				try
				{
					Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(path));
					Formatting formatting = leaderboard.DateTime > fullHistoryDateStart ? Formatting.None : Formatting.Indented;
					File.WriteAllText(path, JsonConvert.SerializeObject(leaderboard, formatting));

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"SUCCESS for {path}: {formatting}");
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"FAIL for {path}: {ex.Message}");
				}
			}
		}
	}
}
