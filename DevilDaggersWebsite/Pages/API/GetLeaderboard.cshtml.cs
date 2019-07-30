using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns 100 leaderboard entries from the current leaderboard starting at the rank parameter. The default value for the rank parameter is 1.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetLeaderboardModel : ApiPageModel
	{
		public async Task<FileResult> OnGetAsync(int rank = 1, bool formatted = false)
		{
			rank = Math.Max(1, rank);

			DevilDaggersCore.Leaderboards.Leaderboard leaderboard = await Hasmodai.GetScores(rank);

			return JsonFile(leaderboard, leaderboard.DateTime.ToString("yyyyMMddHHmm"), formatted ? Formatting.Indented : Formatting.None);
		}
	}
}