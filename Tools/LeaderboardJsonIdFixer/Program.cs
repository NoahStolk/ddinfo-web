using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToolsShared;

namespace LeaderboardJsonIdFixer
{
	public static class Program
	{
		private const string _leaderboardHistoryPath = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite.Razor\wwwroot\leaderboard-history";

		public static void Main()
		{
			// Raven fix
			//SwapIds(new DateTime(1, 1, 1), new DateTime(2019, 10, 11), 86805, 187974);

			// pocket fix
			//SwapIds(new DateTime(1, 1, 1), new DateTime(2020, 1, 14), 116704, 106722);

			ApplyNameTable();
		}

		/// <summary>
		/// Swaps Ids for players with 2 accounts in the leaderboard history files.
		/// </summary>
		/// <param name="dateStart">The date on which the player's alt overtook their main account.</param>
		/// <param name="dateEnd">The date on which the player's main overtook their alt account.</param>
		/// <param name="id1">The first Id to swap.</param>
		/// <param name="id2">The second Id to swap.</param>
		public static void SwapIds(DateTime dateStart, DateTime dateEnd, int id1, int id2)
		{
			foreach (string leaderboardHistoryPath in Directory.GetFiles(_leaderboardHistoryPath, "*.json"))
			{
				string fileName = Path.GetFileNameWithoutExtension(leaderboardHistoryPath);
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8)) ?? throw new("Could not deserialize leaderboard.");
				if (leaderboard.DateTime < dateStart || leaderboard.DateTime > dateEnd)
					continue;

				Entry? entry1 = leaderboard.Entries.Find(e => e.Id == id1);
				Entry? entry2 = leaderboard.Entries.Find(e => e.Id == id2);

				if (entry1 != null)
					entry1.Id = id2;
				if (entry2 != null)
					entry2.Id = id1;

				File.WriteAllText(leaderboardHistoryPath, JsonConvert.SerializeObject(leaderboard));
				Console.WriteLine($"Wrote {fileName}.", ConsoleColor.Green);
			}
		}

		private static void ApplyNameTable()
		{
			foreach (string path in Directory.GetFiles(_leaderboardHistoryPath, "*.json"))
			{
				string jsonString = File.ReadAllText(path, Encoding.UTF8);
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(jsonString) ?? throw new("Could not deserialize leaderboard.");

				List<Entry> changes = new();
				foreach (Entry entry in leaderboard.Entries)
				{
					if ((entry.Id == 0 || entry.Id == -1) && NameData.NameTable.ContainsKey(entry.Username))
					{
						entry.Id = NameData.NameTable[entry.Username];
						changes.Add(entry);
					}
				}

				if (changes.Count != 0)
				{
					Console.WriteLine(HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(path)));
					foreach (Entry entry in changes)
						Console.WriteLine($"\tSet Id to {entry.Id:D6} for rank {entry.Rank:D3} with name {entry.Username} and score {entry.Time.FormatTimeInteger()}");
					Console.WriteLine();
				}

				using StreamWriter sw = File.CreateText(path);
				sw.Write(JsonConvert.SerializeObject(leaderboard));
			}
		}
	}
}
