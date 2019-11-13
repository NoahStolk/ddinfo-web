using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the current user data corresponding to the given userID parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserByIDModel : ApiPageModel
	{
		public async Task<FileResult> OnGetAsync(int userID = default, bool formatted = false) => JsonFile(await ApiFunctions.GetUserByID(userID), formatted ? Formatting.Indented : Formatting.None);
	}
}