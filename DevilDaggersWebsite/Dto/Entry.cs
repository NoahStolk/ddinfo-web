using DevilDaggersWebsite.LeaderboardHistory;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DevilDaggersWebsite.Dto
{
	public class Entry
	{
		[CompletionProperty]
		public int Rank { get; set; }

		[CompletionProperty]
		public int Id { get; set; }

		[CompletionProperty]
		public string Username { get; set; } = null!;

		[CompletionProperty]
		public int Time { get; set; }

		[CompletionProperty]
		public int Kills { get; set; }

		[CompletionProperty]
		public int Gems { get; set; }

		[CompletionProperty]
		public short DeathType { get; set; }

		[CompletionProperty]
		public int DaggersHit { get; set; }

		[CompletionProperty]
		public int DaggersFired { get; set; }

		[CompletionProperty]
		public ulong TimeTotal { get; set; }

		[CompletionProperty]
		public ulong KillsTotal { get; set; }

		[CompletionProperty]
		public ulong GemsTotal { get; set; }

		[CompletionProperty]
		public ulong DeathsTotal { get; set; }

		[CompletionProperty]
		public ulong DaggersHitTotal { get; set; }

		[CompletionProperty]
		public ulong DaggersFiredTotal { get; set; }

		[JsonIgnore]
		public double Accuracy => DaggersFired == 0 ? 0 : DaggersHit / (double)DaggersFired;

		[JsonIgnore]
		public double AccuracyTotal => DaggersFiredTotal == 0 ? 0 : DaggersHitTotal / (double)DaggersFiredTotal;

		[JsonIgnore]
		public Completion Completion { get; } = new();

		public Completion GetCompletion()
		{
			if (Completion.Initialized)
				return Completion;

			foreach (PropertyInfo info in GetType().GetProperties())
			{
				object? value = info.GetValue(this);
				if (value == null)
					continue;

				string? valueString = value.ToString();
				if (string.IsNullOrEmpty(valueString))
					continue;

				if (!Attribute.IsDefined(info, typeof(CompletionPropertyAttribute)))
					continue;

				if (info.Name == nameof(DeathType) && valueString == "-1"
				 || info.Name != nameof(DeathType) && valueString == ReflectionUtils.GetDefaultValue(value.GetType())?.ToString())
				{
					Completion.CompletionEntries[info.Name] = CompletionEntry.Missing;
				}
				else
				{
					Completion.CompletionEntries[info.Name] = CompletionEntry.Complete;
				}
			}

			Completion.Initialized = true;
			return Completion;
		}

		public bool IsBlankName()
			=> Id == 999999 || Id == 9999999;

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
