using DevilDaggersWebsite.Models.Leaderboard;
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
	public class LeaderboardJsonModel : PageModel
	{
		[BindProperty]
		public Leaderboard Leaderboard { get; set; } = new Leaderboard();
		public string JsonResult { get; set; }
		public DateTime DateTime { get; set; }

		public async Task<ActionResult> OnGetAsync()
		{
			// Top 100 only now
			Leaderboard = await LeaderboardUtils.LoadLeaderboard(Leaderboard, 1);
			JsonResult = JsonConvert.SerializeObject(Leaderboard);
			DateTime = Leaderboard.DateTime;

			return File(Encoding.UTF8.GetBytes(JsonResult), MediaTypeNames.Application.Json, $"{DateTime.ToString("yyyyMMddHHmm")}.json");
		}
	}
}