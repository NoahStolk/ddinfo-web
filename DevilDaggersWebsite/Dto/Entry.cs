using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DevilDaggersWebsite.Dto
{
	public class Entry
	{
		public int Rank { get; set; }

		public int Id { get; set; }

		public string Username { get; set; } = null!;

		public int Time { get; set; }

		public int Kills { get; set; }

		public int Gems { get; set; }

		public short DeathType { get; set; }

		public int DaggersHit { get; set; }

		public int DaggersFired { get; set; }

		public ulong TimeTotal { get; set; }

		public ulong KillsTotal { get; set; }

		public ulong GemsTotal { get; set; }

		public ulong DeathsTotal { get; set; }

		public ulong DaggersHitTotal { get; set; }

		public ulong DaggersFiredTotal { get; set; }

		[JsonIgnore]
		public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;

		[JsonIgnore]
		public double AccuracyTotal => DaggersFiredTotal == 0 ? 0 : DaggersHitTotal / (double)DaggersFiredTotal;

		public bool ExistsInHistory(IWebHostEnvironment env)
		{
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				if (leaderboard.Entries.Any(e => e.Id == Id))
					return true;
			}

			return false;
		}

		public List<string> GetAllUsernameAliases(IWebHostEnvironment env)
		{
			Dictionary<string, int> aliases = new();
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry? historyEntry = leaderboard.Entries.Find(e => e.Id == Id);
				if (historyEntry != null)
				{
					if (string.IsNullOrWhiteSpace(historyEntry.Username))
						continue;

					if (aliases.ContainsKey(historyEntry.Username))
						aliases[historyEntry.Username]++;
					else
						aliases.Add(historyEntry.Username, 1);
				}
			}

			return aliases.OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
		}
	}
}
