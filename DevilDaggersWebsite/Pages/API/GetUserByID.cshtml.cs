using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the current user data corresponding to the given userId parameter.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserByIdModel : ApiPageModel
	{
		public async Task<FileResult> OnGetAsync(int userId = default, bool formatted = false) => JsonFile(await ApiFunctions.GetUserById(userId), formatted ? Formatting.Indented : Formatting.None);
	}
}