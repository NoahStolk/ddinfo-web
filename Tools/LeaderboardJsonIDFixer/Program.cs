using DevilDaggersUtilities.Tools;
using DevilDaggersUtilities.Website;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeaderboardJsonIDFixer
{
	public static class Program
	{
		public static Dictionary<string, int> nameTable = new Dictionary<string, int>
		{
			{ "bowsr", 1135 },
			{ "Sojk", 229 },
			{ "Bintr", 148788 },
			{ "DraQu", 82891 }, // Not sure why his ID is so high
			{ "twitch.tv/draqu_", 82891 },
			{ "DraQu^WR", 82891 },
			{ "weaksauce13", 112 },
			{ "b0necarver", -1 }, // No idea, but we need this for the world record graph
			{ "m4ttbush", 1 },
			{ "A.N.T.4", 1677 },
			{ "pocket", 116704 },
			{ "6,1742 ms [S1] Jump", 116704 },
			{ "KrosisOne", 1545 },
			{ "Cookle", 93991 },
			{ "cookie the #11 dd boy", 93991 },
			{ "xvlv", 21854 },
			{ "Kaijt", 438 },
			{ "aircawn", 2316 },
			{ "sjorsbw", 10210 },
			{ "Chupacabra", 118832 }
		};

		public static void Main(string[] args)
		{
			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history", "*.json"))
			{
				string jsonString = FileUtils.GetContents(path, Encoding.UTF8);
				LeaderboardSimplified leaderboard = JsonConvert.DeserializeObject<LeaderboardSimplified>(jsonString);

				List<LeaderboardEntrySimplified> changes = new List<LeaderboardEntrySimplified>();
				foreach (LeaderboardEntrySimplified entry in leaderboard.Entries)
				{
					if (entry.ID == 0 && nameTable.ContainsKey(entry.Username))
					{
						entry.ID = nameTable[entry.Username];
						changes.Add(entry);
					}
				}

				if (changes.Count != 0)
				{
					ConsoleUtils.WriteLineColor(LeaderboardHistoryUtils.HistoryJsonFileNameToDateString(Path.GetFileNameWithoutExtension(path)), ConsoleColor.Yellow);
					foreach (LeaderboardEntrySimplified entry in changes)
						Console.WriteLine($"Set ID to {entry.ID.ToString("D6")} for rank {entry.Rank.ToString("D3")} with name {entry.Username}");
					Console.WriteLine();
				}

				using (StreamWriter sw = File.CreateText(path))
				{
					sw.Write(JsonConvert.SerializeObject(leaderboard));
				}
			}
		}
	}
}