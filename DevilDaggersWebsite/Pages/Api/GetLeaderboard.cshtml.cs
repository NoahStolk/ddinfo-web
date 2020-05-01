using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Threading.Tasks;
using Lb = DevilDaggersCore.Leaderboards.Leaderboard;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns 100 leaderboard entries from the current leaderboard starting at the rank parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetLeaderboardModel : ApiPageModel
	{
		public async Task<FileResult> OnGetAsync(int rank = 1, bool formatted = false)
		{
			Lb leaderboard = await ApiFunctions.GetLeaderboard(rank);

			return JsonFile(leaderboard, leaderboard.DateTime.ToString("yyyyMMddHHmm"), formatted ? Formatting.Indented : Formatting.None);
		}
	}
}