using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Razor.Utils;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class WorldRecordProgressionModel : PageModel
	{
		private readonly LeaderboardHistoryHelper _leaderboardHistoryHelper;

		public WorldRecordProgressionModel(LeaderboardHistoryHelper leaderboardHistoryHelper)
		{
			_leaderboardHistoryHelper = leaderboardHistoryHelper;
		}

		public List<WorldRecordHolder> WorldRecordHolders { get; private set; } = new();

		public Dictionary<WorldRecord, TimeSpan> WorldRecords { get; private set; } = new();

		public void OnGet()
		{
			(WorldRecordHolders, WorldRecords) = _leaderboardHistoryHelper.GetWorldRecordData();
		}

		public string GetHistoryDateString(DateTime dateTime)
		{
			int daysAgo = (int)Math.Round((DateTime.Now - dateTime).TotalDays);
			return $"{dateTime:MMM dd} '{dateTime:yy} ({daysAgo} day{daysAgo.S()} ago)";
		}
	}
}
