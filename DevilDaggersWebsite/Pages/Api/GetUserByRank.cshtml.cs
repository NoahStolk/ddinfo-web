using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the current user data based on the specified leaderboard rank parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserByRankModel : ApiPageModel
	{
		public async Task<FileResult> OnGetAsync(int rank = 1, bool formatted = false) => JsonFile(await ApiFunctions.GetUserByRank(rank), formatted ? Formatting.Indented : Formatting.None);
	}
}