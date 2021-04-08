using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.Utils;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Models
{
	public class CustomLeaderboardModel
	{
		public CustomLeaderboardModel(CustomLeaderboard customLeaderboard, List<CustomEntry> entries)
		{
			SpawnsetName = customLeaderboard.SpawnsetFile.Name;
			TotalRunsSubmitted = customLeaderboard.TotalRunsSubmitted;
			DateLastPlayed = customLeaderboard.DateLastPlayed.HasValue ? new(customLeaderboard.DateLastPlayed.Value.ToString("dd MMM yyyy, HH:mm")) : RazorUtils.NAString;
			DateCreated = customLeaderboard.DateCreated.HasValue ? new(customLeaderboard.DateCreated.Value.ToString("dd MMM yyyy")) : RazorUtils.NAString;
			TimeBronze = customLeaderboard.TimeBronze;
			TimeSilver = customLeaderboard.TimeSilver;
			TimeGolden = customLeaderboard.TimeGolden;
			TimeDevil = customLeaderboard.TimeDevil;
			TimeLeviathan = customLeaderboard.TimeLeviathan;
			WorldRecord = entries.Count == 0 ? RazorUtils.NAString : new(entries[0].Time.FormatTimeInteger());
			WorldRecordDagger = entries.Count == 0 ? null : customLeaderboard.GetDagger(entries[0].Time);

			HtmlData = new($@"
name='{SpawnsetName}'
players='{entries.Count}'
submits='{TotalRunsSubmitted}'
last-played='{(customLeaderboard.DateLastPlayed.HasValue ? customLeaderboard.DateLastPlayed.Value.ToString("yyyyMMddHHmmss") : "0")}'
created='{(customLeaderboard.DateCreated.HasValue ? customLeaderboard.DateCreated.Value.ToString("yyyyMMddHHmmss") : "0")}'
bronze='{TimeBronze}'
silver='{TimeSilver}'
golden='{TimeGolden}'
devil='{TimeDevil}'
leviathan='{TimeLeviathan}'
world-record='{(entries.Count == 0 ? "0" : entries[0].Time)}'");
		}

		public string SpawnsetName { get; }
		public int TotalRunsSubmitted { get; }
		public HtmlString DateLastPlayed { get; }
		public HtmlString DateCreated { get; }
		public int TimeBronze { get; }
		public int TimeSilver { get; }
		public int TimeGolden { get; }
		public int TimeDevil { get; }
		public int TimeLeviathan { get; }
		public HtmlString WorldRecord { get; }
		public string? WorldRecordDagger { get; }

		public HtmlString HtmlData { get; }
	}
}
