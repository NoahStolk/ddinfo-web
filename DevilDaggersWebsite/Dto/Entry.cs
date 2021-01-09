using DevilDaggersCore.Game;
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.LeaderboardHistory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace DevilDaggersWebsite.Dto
{
	public class Entry
	{
		[CompletionProperty]
		public int Rank { get; set; }

		[CompletionProperty]
		public int Id { get; set; }

		[CompletionProperty]
		public string Username { get; set; }

		[CompletionProperty]
		public int Time { get; set; }

		[CompletionProperty]
		public int Kills { get; set; }

		[CompletionProperty]
		public int Gems { get; set; }

		[CompletionProperty]
		public int DeathType { get; set; }

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
				 || info.Name != nameof(DeathType) && valueString == ReflectionUtils.GetDefaultValue(value.GetType()).ToString())
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

		public HtmlString ToHtmlData(string flagCode)
		{
			ulong deaths = DeathsTotal == 0 ? 1 : DeathsTotal;
			return new HtmlString($@"
                rank='{Rank}'
                flag='{flagCode}'
                username='{HttpUtility.HtmlEncode(Username)}'
                time='{Time}'
                kills='{Kills}'
                gems='{Gems}'
                accuracy='{Accuracy * 10000:0}'
                death-type='{GameInfo.GetDeathByType(DeathType)?.Name ?? "Unknown"}'
                total-time='{TimeTotal}'
                total-kills='{KillsTotal}'
                total-gems='{GemsTotal}'
                total-accuracy='{AccuracyTotal * 10000:0}'
                total-deaths='{DeathsTotal}'
                daggers-hit='{DaggersHit}'
                daggers-fired='{DaggersFired}'
                total-daggers-hit='{DaggersHitTotal}'
                total-daggers-fired='{DaggersFiredTotal}'
                average-time='{TimeTotal * 10000f / deaths:0}'
                average-kills='{KillsTotal * 100f / deaths:0}'
                average-gems='{GemsTotal * 100f / deaths:0}'
                average-daggers-hit='{DaggersHitTotal * 100f / deaths:0}'
                average-daggers-fired='{DaggersFiredTotal * 100f / deaths:0}'
                time-by-death='{Time * 10000f / deaths:0}'
            ");
		}

		public HtmlString ToHtmlData(string flagCode, Player player) => new HtmlString($@"
			rank='{Rank}'
			flag='{flagCode}'
			username='{HttpUtility.HtmlEncode(Username)}'
			time='{Time}'
			e-dpi='{player.Edpi * 1000 ?? 0}'
			dpi='{player.Dpi ?? 0}'
			in-game-sens='{player.InGameSens * 1000 ?? 0}'
			fov='{player.Fov ?? 0}'
			hand='{(!player.RightHanded.HasValue ? -1 : player.RightHanded.Value ? 1 : 0)}'
			flash='{(!player.FlashEnabled.HasValue ? -1 : player.FlashEnabled.Value ? 1 : 0)}'
			gamma='{player.Gamma ?? 0}'
		");

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
			Dictionary<string, int> aliases = new Dictionary<string, int>();
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry historyEntry = leaderboard.Entries.Find(e => e.Id == Id);
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