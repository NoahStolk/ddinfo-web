using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Razor.Utils;
using DevilDaggersWebsite.Transients;
using DevilDaggersWebsite.WorldRecords;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lb = DevilDaggersWebsite.Dto.Leaderboard;

namespace DevilDaggersWebsite.Razor.Pages.Leaderboard
{
	public class WorldRecordProgressionModel : PageModel
	{
		private readonly WorldRecordsHelper _leaderboardHistoryHelper;

		public WorldRecordProgressionModel(WorldRecordsHelper leaderboardHistoryHelper)
		{
			_leaderboardHistoryHelper = leaderboardHistoryHelper;
		}

		public List<WorldRecordHolder> WorldRecordHolders { get; private set; } = new();

		public Dictionary<WorldRecord, TimeSpan> WorldRecords { get; private set; } = new();

		public Entry? CurrentWorldRecord { get; private set; }

		public async Task OnGetAsync()
		{
			(WorldRecordHolders, WorldRecords) = _leaderboardHistoryHelper.GetWorldRecordData();

			Lb? leaderboard = await LeaderboardClient.Instance.GetScores(1);
			if (leaderboard?.Entries.Count > 0)
				CurrentWorldRecord = leaderboard.Entries[0];
		}

		public string GetHistoryDateString(DateTime dateTime)
		{
			int daysAgo = (int)Math.Round((DateTime.UtcNow - dateTime).TotalDays);
			return $"{dateTime:MMM dd} '{dateTime:yy} ({daysAgo} day{daysAgo.S()} ago)";
		}
	}
}
