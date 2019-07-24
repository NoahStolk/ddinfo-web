using DevilDaggersCore.Leaderboard;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the user data for all users with a username that contains the username search parameter. Returns to this page if the username parameter has less than 3 characters.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserByUsernameModel : ApiPageModel
	{
		public async Task<ActionResult> OnGetAsync(string username, bool formatted = false)
		{
			if (username.Length < 3)
				return RedirectToPage("/API/Index");

			List<Entry> leaderboard = await GetUserSearch(username);
			return JsonFile(leaderboard, formatted ? Formatting.Indented : Formatting.None);
		}

		public async Task<List<Entry>> GetUserSearch(string username)
		{
			DevilDaggersCore.Leaderboard.Leaderboard leaderboard = await Hasmodai.GetUserSearch(username);
			return leaderboard.Entries;
		}
	}
}