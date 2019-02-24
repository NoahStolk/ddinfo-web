using DevilDaggersUtilities.Tools;
using NetBase.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeaderboardJsonIDFixer
{
	class Program
	{
		public static Dictionary<string, int> table = new Dictionary<string, int>
		{
			{ "bowsr", 1135 },
			{ "Sojk", 229 },
			{ "Bintr", 148788 },
			{ "DraQu", 82891 }, // Probably wrong
			{ "twitch.tv/draqu_", 82891 }, // Probably wrong
			{ "weaksauce13", 112 },
			{ "b0necarver", -1 }, // No idea
			{ "m4ttbush", 1 }
		};

		public static LeaderboardSimplified leaderboard;

		static void Main(string[] args)
		{
			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history"))
			{
				string jsonString = FileUtils.GetContents(path, Encoding.UTF8);
				leaderboard = JsonConvert.DeserializeObject<LeaderboardSimplified>(jsonString);

				foreach (LeaderboardEntrySimplified entry in leaderboard.Entries)
				{
					if (table.ContainsKey(entry.Username))
					{
						entry.ID = table[entry.Username];
					}
				}

				using (StreamWriter sw = File.CreateText(path))
				{
					sw.Write(JsonConvert.SerializeObject(leaderboard));
				}
			}
		}
	}
}