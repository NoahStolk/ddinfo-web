using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the latest version of the tool corresponding to the given toolName parameter as a zip file. Returns to this page if the tool could not be found.", ReturnType = MediaTypeNames.Application.Zip)]
	public class GetToolModel : ApiPageModel
	{
		public ActionResult OnGet(string toolName)
		{
			if (ApiFunctions.TryGetToolPath(toolName, out string fileName, out string path))
				return File(path, MediaTypeNames.Application.Octet, fileName);

			return RedirectToPage("/API/Index");
		}
	}
}