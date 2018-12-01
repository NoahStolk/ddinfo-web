using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns a JSON copy of the current leaderboard.", ReturnType = MediaTypeNames.Application.Json)]
	public class LeaderboardJsonModel : PageModel
	{
		public async Task<FileResult> OnGetAsync()
		{
			Models.Leaderboard.Leaderboard leaderboard = await LeaderboardUtils.LoadLeaderboard(1); // Top 100 only now

			string jsonResult = JsonConvert.SerializeObject(leaderboard);
			DateTime dateTime = leaderboard.DateTime;
			return File(Encoding.UTF8.GetBytes(jsonResult), MediaTypeNames.Application.Json, $"{dateTime.ToString("yyyyMMddHHmm")}.json");
		}
	}
}