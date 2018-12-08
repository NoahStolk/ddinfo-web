using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns a JSON copy of the current leaderboard.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetLeaderboardModel : ApiPageModel
	{
		public async Task<FileResult> OnGetAsync()
		{
			Models.Leaderboard.Leaderboard leaderboard = await LeaderboardUtils.LoadLeaderboard(1); // Top 100 only now, TODO: Get parameter for rank offset

			return JsonFile(leaderboard, leaderboard.DateTime.ToString("yyyyMMddHHmm"));
		}
	}
}