using DevilDaggersCore.Game;
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Razor.Extensions;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DevilDaggersWebsite.Razor.Models
{
	public class CustomEntryModel
	{
		public CustomEntryModel(int rank, CustomEntry customEntry, CustomLeaderboard customLeaderboard, Player? player, IEnumerable<Donation> donations)
		{
			PlayerId = customEntry.PlayerId;
			Rank = rank;
			FlagCode = player?.CountryCode ?? string.Empty;
			string countryName = UserUtils.CountryNames.ContainsKey(FlagCode) ? UserUtils.CountryNames[FlagCode] : "Invalid country code";
			FlagHtml = string.IsNullOrEmpty(FlagCode) ? new("<span><img src='/images/Flags/24x24/00.png' /></span>") : new($"<span class='leaderboard-tooltip' data-toggle='tooltip' title='{countryName}'><img src='/images/Flags/24x24/{FlagCode}.png' /></span>");

			Username = player?.PlayerName ?? "Player not found";
			Time = customEntry.Time;
			EnemiesKilled = customEntry.EnemiesKilled;
			EnemiesAlive = customEntry.EnemiesAlive;
			GemsCollected = customEntry.GemsCollected;
			DeathType = customEntry.DeathType;
			HomingDaggers = customEntry.HomingDaggers;
			SubmitDate = customEntry.SubmitDate;
			ClientVersion = customEntry.ClientVersion;

			List<string> titles = player?.PlayerTitles.ConvertAll(pt => pt.Title.Name) ?? new();
			if (donations.Any(d => d.PlayerId == PlayerId) && !(player?.IsAnonymous ?? true))
				titles.Add("Donator");

			StringBuilder sb = new();
			if (titles != null)
			{
				foreach (string title in titles)
					sb.Append("<span class='leaderboard-tooltip' data-toggle='tooltip' title='").Append(title).Append("'><img src='/images/Icons/").Append(UserUtils.TitleImages[title]).AppendLine(".png' /></span>");
			}

			TitlesHtml = new(sb.ToString());

			DaggerName = customLeaderboard.GetDagger(Time);

			if (customLeaderboard.Category == CustomLeaderboardCategory.Challenge)
			{
				DeathStyle = "color: #ff4400;";
				DeathName = "IMMORTAL";
			}
			else
			{
				Death? death = GameInfo.GetDeathByType(GameVersion.V31, DeathType);
				DeathStyle = $"color: #{death?.ColorCode ?? "444"};";
				DeathName = death?.Name ?? "Invalid";
			}

			LevelUpTime2 = customEntry.LevelUpTime2 == 0 ? RazorUtils.NAString : new(customEntry.LevelUpTime2.FormatTimeInteger());
			LevelUpTime3 = customEntry.LevelUpTime3 == 0 ? RazorUtils.NAString : new(customEntry.LevelUpTime3.FormatTimeInteger());
			LevelUpTime4 = customEntry.LevelUpTime4 == 0 ? RazorUtils.NAString : new(customEntry.LevelUpTime4.FormatTimeInteger());
			string submitDate = customEntry.SubmitDate.ToString("dd MMM yyyy, HH:mm");
			bool v31 = false;
			bool homingEaten = false;
			if (Version.TryParse(customEntry.ClientVersion, out Version? version) && version != null)
			{
				v31 = version > new Version(0, 10, 4, 0);
				homingEaten = version >= new Version(0, 14, 5, 0);
			}

			GemsDespawned = v31 ? new(customEntry.GemsDespawned.ToString(FormatUtils.LeaderboardIntFormat)) : RazorUtils.NAString;
			GemsEaten = v31 ? new(customEntry.GemsEaten.ToString(FormatUtils.LeaderboardIntFormat)) : RazorUtils.NAString;
			HomingDaggersEaten = homingEaten ? new(customEntry.HomingDaggersEaten.ToString(FormatUtils.LeaderboardIntFormat)) : RazorUtils.NAString;

			DaggerTooltipText = FormatUtils.FormatDaggersInt32(customEntry.DaggersHit, customEntry.DaggersFired, false);
			Accuracy = customEntry.Accuracy.ToString(FormatUtils.AccuracyFormat);

			HtmlData = new($@"
rank='{rank}'
flag='{FlagCode}'
username='{HttpUtility.HtmlEncode(Username)}'
time='{Time}'
enemies-killed='{EnemiesKilled}'
enemies-alive='{EnemiesAlive}'
gems-collected='{GemsCollected}'
gems-despawned='{(v31 ? customEntry.GemsDespawned : -1)}'
gems-eaten='{(v31 ? customEntry.GemsEaten : -1)}'
accuracy='{customEntry.Accuracy * 10000:0}'
death-type='{GameInfo.GetDeathByType(GameVersion.V31, DeathType)?.Name ?? "Invalid"}'
homing-daggers='{HomingDaggers}'
homing-daggers-eaten='{(homingEaten ? customEntry.HomingDaggersEaten : -1)}'
level-2='{(customEntry.LevelUpTime2 == 0 ? 999999999 : customEntry.LevelUpTime2)}'
level-3='{(customEntry.LevelUpTime3 == 0 ? 999999999 : customEntry.LevelUpTime3)}'
level-4='{(customEntry.LevelUpTime4 == 0 ? 999999999 : customEntry.LevelUpTime4)}'
submit-date='{SubmitDate:yyyyMMddHHmm}'");
		}

		public int PlayerId { get; }
		public int Rank { get; }
		public string FlagCode { get; }
		public HtmlString FlagHtml { get; }
		public HtmlString TitlesHtml { get; }
		public string Username { get; }
		public int Time { get; }
		public int EnemiesKilled { get; }
		public int EnemiesAlive { get; }
		public int GemsCollected { get; }
		public HtmlString GemsDespawned { get; }
		public HtmlString GemsEaten { get; }
		public int DaggersFired { get; }
		public int DaggersHit { get; }
		public int HomingDaggers { get; }
		public HtmlString HomingDaggersEaten { get; }
		public byte DeathType { get; }
		public HtmlString LevelUpTime2 { get; }
		public HtmlString LevelUpTime3 { get; }
		public HtmlString LevelUpTime4 { get; }
		public DateTime SubmitDate { get; }
		public string ClientVersion { get; }

		public string DaggerName { get; }
		public string DeathStyle { get; }
		public string DeathName { get; }
		public string DaggerTooltipText { get; }

		public HtmlString HtmlData { get; }

		public string Accuracy { get; }
	}
}
