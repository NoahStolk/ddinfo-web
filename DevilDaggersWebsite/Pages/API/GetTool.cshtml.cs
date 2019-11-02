using DevilDaggersCore.Website.Models;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the latest version of the tool corresponding to the given toolName parameter as a zip file. Returns to this page if the tool could not be found.", ReturnType = MediaTypeNames.Application.Zip)]
	public class GetToolModel : ApiPageModel
	{
        public ActionResult OnGet(string toolName)
		{
			Tool tool = ToolList.Tools.Where(t => t.Name == toolName).FirstOrDefault();
			if (tool == null)
				return RedirectToPage("/API/Index");

			string fileName = $"{toolName}{tool.VersionNumber}.zip";
			return File(Path.Combine("tools", toolName, fileName), MediaTypeNames.Application.Octet, fileName);
		}
    }
}