using DevilDaggersCore.Leaderboard;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Utils.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the current user data corresponding to the given userID parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserByIDModel : ApiPageModel
	{
		public async Task<FileResult> OnGetAsync(int userID, bool formatted = false)
		{
			Entry entry = await GetUser(userID);
			return JsonFile(entry, formatted ? Formatting.Indented : Formatting.None);
		}

		public async Task<Entry> GetUser(int userID)
		{
			return await Hasmodai.GetUserByID(userID);
		}
	}
}