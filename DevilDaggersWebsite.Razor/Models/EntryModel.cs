using DevilDaggersCore.Game;
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevilDaggersWebsite.Razor.Models
{
	public class EntryModel : IEntryModel
	{
		public EntryModel(Entry entry, Player? player, IEnumerable<Donation> donations, bool isHistory, GameVersion gameVersion)
		{
			PlayerId = entry.Id;
			Rank = entry.Rank;

			Titles = Array.Empty<string>();
			if (player != null)
			{
				List<string> titles = player.PlayerTitles.ConvertAll(pt => pt.Title.Name) ?? new();
				if (donations.Any(d => d.PlayerId == player.Id) && !(player?.IsAnonymous ?? true))
					titles.Add("Donator");
				Titles = titles.ToArray();

				IsBanned = player.IsBanned;
				BanDescription = player.BanDescription;
			}

			Username = entry.Username;
			FlagCode = player?.CountryCode ?? string.Empty;
			CountryName = UserUtils.CountryNames.ContainsKey(FlagCode) ? UserUtils.CountryNames[FlagCode] : "Invalid country code";

			Dagger dagger = GameInfo.GetDaggerFromTime(gameVersion, entry.Time);
			DaggerColor = player?.IsBanned ?? false ? "ban" : dagger.Name.ToLower();

			Death? death = GameInfo.GetDeathByType(gameVersion, entry.DeathType);
			DeathStyle = player?.IsBanned ?? false ? string.Empty : $"color: #{death?.ColorCode ?? "444"};";
			DeathName = death?.Name ?? "Unknown";

			BanString = player?.IsBanned ?? false ? "ban" : string.Empty;

			Rank = entry.Rank;
			Time = entry.Time.FormatTimeInteger();
			Kills = entry.Kills.ToString(FormatUtils.LeaderboardIntFormat);
			Gems = entry.Gems.ToString(FormatUtils.LeaderboardIntFormat);
			TimeTotal = entry.TimeTotal.FormatTimeInteger(true);
			KillsTotal = entry.KillsTotal.ToString(FormatUtils.LeaderboardIntFormat);
			GemsTotal = entry.GemsTotal.ToString(FormatUtils.LeaderboardIntFormat);
			DeathsTotal = entry.DeathsTotal.ToString(FormatUtils.LeaderboardIntFormat);

			Daggers = FormatUtils.FormatDaggersInt32(entry.DaggersHit, entry.DaggersFired, isHistory);
			Accuracy = entry.Accuracy.ToString(FormatUtils.AccuracyFormat);
			DaggersTotal = FormatUtils.FormatDaggersUInt64(entry.DaggersHitTotal, entry.DaggersFiredTotal, isHistory);
			AccuracyTotal = entry.AccuracyTotal.ToString(FormatUtils.AccuracyFormat);

			DaggersHit = entry.DaggersHit.ToString(FormatUtils.LeaderboardIntFormat);
			DaggersFired = entry.DaggersFired.ToString(FormatUtils.LeaderboardIntFormat);
			DaggersHitTotal = entry.DaggersHitTotal.ToString(FormatUtils.LeaderboardIntFormat);
			DaggersFiredTotal = entry.DaggersFiredTotal.ToString(FormatUtils.LeaderboardIntFormat);
			AverageTime = entry.DeathsTotal == 0 ? RazorUtils.NAString : new((entry.TimeTotal / entry.DeathsTotal).FormatTimeInteger());
			AverageKills = entry.DeathsTotal == 0 ? RazorUtils.NAString : new((entry.KillsTotal / (float)entry.DeathsTotal).ToString(FormatUtils.LeaderboardIntAverageFormat));
			AverageGems = entry.DeathsTotal == 0 ? RazorUtils.NAString : new((entry.GemsTotal / (float)entry.DeathsTotal).ToString(FormatUtils.LeaderboardIntAverageFormat));
			AverageDaggersHit = entry.DeathsTotal == 0 ? RazorUtils.NAString : new((entry.DaggersHitTotal / (float)entry.DeathsTotal).ToString(FormatUtils.LeaderboardIntAverageFormat));
			AverageDaggersFired = entry.DeathsTotal == 0 ? RazorUtils.NAString : new((entry.DaggersFiredTotal / (float)entry.DeathsTotal).ToString(FormatUtils.LeaderboardIntAverageFormat));
			TimeByDeath = entry.DeathsTotal == 0 ? RazorUtils.NAString : new((entry.Time / (float)entry.DeathsTotal).ToString(FormatUtils.LeaderboardTimeLargeFormat));

			ulong deathsTotal = entry.DeathsTotal == 0 ? 1 : entry.DeathsTotal;
			HtmlData = new($@"
rank='{entry.Rank}'
flag='{FlagCode}'
username='{HttpUtility.HtmlEncode(entry.Username)}'
time='{entry.Time}'
kills='{entry.Kills}'
gems='{entry.Gems}'
accuracy='{entry.Accuracy * 10000:0}'
death-type='{GameInfo.GetDeathByType(gameVersion, entry.DeathType)?.Name ?? "Unknown"}'
total-time='{entry.TimeTotal}'
total-kills='{entry.KillsTotal}'
total-gems='{entry.GemsTotal}'
total-accuracy='{entry.AccuracyTotal * 10000:0}'
total-deaths='{entry.DeathsTotal}'
daggers-hit='{entry.DaggersHit}'
daggers-fired='{entry.DaggersFired}'
total-daggers-hit='{entry.DaggersHitTotal}'
total-daggers-fired='{entry.DaggersFiredTotal}'
average-time='{entry.TimeTotal * 10000f / deathsTotal:0}'
average-kills='{entry.KillsTotal * 100f / deathsTotal:0}'
average-gems='{entry.GemsTotal * 100f / deathsTotal:0}'
average-daggers-hit='{entry.DaggersHitTotal * 100f / deathsTotal:0}'
average-daggers-fired='{entry.DaggersFiredTotal * 100f / deathsTotal:0}'
time-by-death='{entry.Time * 10000f / deathsTotal:0}'");
		}

		public bool IsBanned { get; }
		public string? BanDescription { get; }

		public int PlayerId { get; }
		public int Rank { get; }
		public string Username { get; }
		public string FlagCode { get; }
		public string CountryName { get; }
		public string[] Titles { get; }
		public string DaggerColor { get; }
		public string DeathStyle { get; }
		public string DeathName { get; }
		public string BanString { get; }
		public string Time { get; }
		public string Kills { get; }
		public string Gems { get; }
		public string TimeTotal { get; }
		public string KillsTotal { get; }
		public string GemsTotal { get; }
		public string DeathsTotal { get; }
		public string Daggers { get; }
		public string Accuracy { get; }
		public string DaggersTotal { get; }
		public string AccuracyTotal { get; }
		public string DaggersHit { get; }
		public string DaggersFired { get; }
		public string DaggersHitTotal { get; }
		public string DaggersFiredTotal { get; }
		public HtmlString AverageTime { get; }
		public HtmlString AverageKills { get; }
		public HtmlString AverageGems { get; }
		public HtmlString AverageDaggersHit { get; }
		public HtmlString AverageDaggersFired { get; }
		public HtmlString TimeByDeath { get; }

		public HtmlString HtmlData { get; }
	}
}
