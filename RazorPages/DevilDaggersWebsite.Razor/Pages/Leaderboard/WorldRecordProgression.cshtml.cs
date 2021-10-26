using DevilDaggersCore.Game;
using DevilDaggersWebsite.Clients;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Razor.Utils;
using DevilDaggersWebsite.Transients;
using DevilDaggersWebsite.WorldRecords;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
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

		public Dictionary<WorldRecord, WorldRecordData> WorldRecords { get; private set; } = new();

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

		public string GetGameVersionString(GameVersion? gameVersion)
		{
			if (!gameVersion.HasValue)
				return "Pre-release";

			if (gameVersion == GameVersion.V31)
				return "V3.1";

			if (gameVersion == GameVersion.V32)
				return "V3.2";

			return gameVersion.ToString() ?? string.Empty;
		}

		public HtmlString GetWorldRecordSort(WorldRecord worldRecord, WorldRecordData worldRecordData) => new($@"
username='{HttpUtility.HtmlEncode(worldRecord.Entry.Username)}'
time='{worldRecord.Entry.Time}'
duration='{(int)worldRecordData.WorldRecordDuration.TotalHours}'
improvement='{worldRecordData.WorldRecordImprovement ?? 0}'
game-version='{GetGameVersionString(worldRecord.GameVersion)}'");

		public HtmlString GetWorldRecordHolderSort(WorldRecordHolder worldRecordHolder) => new($@"
username='{HttpUtility.HtmlEncode(worldRecordHolder.MostRecentUsername)}'
total-time-held='{(int)worldRecordHolder.TotalTimeHeld.TotalHours}'
longest-time-held='{(int)worldRecordHolder.LongestTimeHeldConsecutively.TotalHours}'
world-record-count='{worldRecordHolder.WorldRecordCount}'
first-held='{worldRecordHolder.FirstHeld:yyyyMMddHHmm}'
last-held='{worldRecordHolder.LastHeld:yyyyMMddHHmm}'");
	}
}
