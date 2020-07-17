using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the latest version of the tool corresponding to the given toolName parameter as a zip file. Returns to this page if the tool could not be found.", ReturnType = MediaTypeNames.Application.Zip)]
	public class GetToolModel : ApiPageModel
	{
		private readonly IWebHostEnvironment env;

		public GetToolModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public ActionResult OnGet(string toolName)
		{
			if (ApiFunctions.TryGetToolPath(env, toolName, out string fileName, out string path))
				return File(path, MediaTypeNames.Application.Octet, fileName);

			return RedirectToPage("/Api/Index");
		}
	}
}