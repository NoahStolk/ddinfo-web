using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the user data for all users with a username that contains the username search parameter. Returns to this page if the username parameter has less than 3 characters.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetUserByUsernameModel : ApiPageModel
	{
		public async Task<ActionResult> OnGetAsync(string username, bool formatted = false)
		{
			if (string.IsNullOrEmpty(username) || username.Length < 3)
				return RedirectToPage("/API/Index");

			return JsonFile(await ApiFunctions.GetUserByUsername(username), formatted ? Formatting.Indented : Formatting.None);
		}
	}
}