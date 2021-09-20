using DevilDaggersCore.Game;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Utils;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
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
				if (player.IsPublicDonator(donations))
					titles.Add("Donator");
				Titles = titles.ToArray();

				IsBanned = player.IsBanned;
				BanDescription = player.BanDescription;
			}

			Username = entry.Username;
			FlagCode = player?.CountryCode ?? string.Empty;
			CountryName = UserUtils.CountryNames.ContainsKey(FlagCode) ? UserUtils.CountryNames[FlagCode] : "Invalid country code";

			Dagger dagger = GameInfo.GetDaggerFromTenthsOfMilliseconds(gameVersion, entry.Time);
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
total-deaths='{entry.DeathsTotal}'");
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

		public HtmlString HtmlData { get; }
	}
}
