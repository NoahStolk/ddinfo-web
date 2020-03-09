using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the user data for all users with a username that contains the username search parameter. Returns to this page if the username parameter has less than 3 characters.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserByUsernameModel : ApiPageModel
	{
		public async Task<ActionResult> OnGetAsync(string username, bool formatted = false)
		{
			if (string.IsNullOrEmpty(username) || username.Length < 3)
				return RedirectToPage("/Api/Index");

			return JsonFile(await ApiFunctions.GetUserByUsername(username), formatted ? Formatting.Indented : Formatting.None);
		}
	}
}