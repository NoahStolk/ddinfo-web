using CoreBase3.Services;
using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the latest version of the tool corresponding to the given toolName parameter as a zip file. Returns to this page if the tool could not be found.", ReturnType = MediaTypeNames.Application.Zip)]
	public class GetToolModel : ApiPageModel
	{
		private readonly ICommonObjects commonObjects;

		public GetToolModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;
		}

		public ActionResult OnGet(string toolName)
		{
			if (ApiFunctions.TryGetToolPath(commonObjects, toolName, out string fileName, out string path))
				return File(path, MediaTypeNames.Application.Octet, fileName);

			return RedirectToPage("/Api/Index");
		}
	}
}