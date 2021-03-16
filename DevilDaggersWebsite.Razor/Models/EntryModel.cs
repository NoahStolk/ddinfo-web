using DevilDaggersCore.Game;
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Models
{
	public class EntryModel
	{
		public EntryModel(Entry entry, Player? player, IEnumerable<Donation> donations, bool isHistory, GameVersion gameVersion)
		{
			IsUnanonymousDonator = donations.Any(d => d.PlayerId == entry.Id) && !(player?.IsAnonymous ?? true);

			FlagCode = player?.CountryCode ?? string.Empty;
			CountryName = UserUtils.CountryNames.ContainsKey(FlagCode) ? UserUtils.CountryNames[FlagCode] : "Invalid country code";
			Titles = player?.PlayerTitles.Select(pt => pt.Title.Name).ToArray() ?? Array.Empty<string>();

			Dagger dagger = GameInfo.GetDaggerFromTime(gameVersion, entry.Time);
			DaggerColor = player?.IsBanned ?? false ? "ban" : dagger.Name.ToLower();

			Death? death = GameInfo.GetDeathByType(gameVersion, entry.DeathType);
			DeathStyle = player?.IsBanned ?? false ? string.Empty : $"color: #{death?.ColorCode ?? "444"};";
			DeathName = death?.Name ?? "Unknown";

			BanString = player?.IsBanned ?? false ? "ban" : string.Empty;

			Rank = entry.Rank.ToString();
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
		}

		public bool IsUnanonymousDonator { get; }
		public string FlagCode { get; }
		public string CountryName { get; }
		public string[] Titles { get; }
		public string DaggerColor { get; }
		public string DeathStyle { get; }
		public string DeathName { get; }
		public string BanString { get; }
		public string Rank { get; }
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
	}
}
