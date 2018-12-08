using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns a JSON copy of the current leaderboard.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetLeaderboardModel : ApiPageModel
	{
		public async Task<FileResult> OnGetAsync(int rank = 1)
		{
			rank = Math.Max(1, rank);

			Models.Leaderboard.Leaderboard leaderboard = await LeaderboardUtils.LoadLeaderboard(rank);

			return JsonFile(leaderboard, leaderboard.DateTime.ToString("yyyyMMddHHmm"));
		}
	}
}