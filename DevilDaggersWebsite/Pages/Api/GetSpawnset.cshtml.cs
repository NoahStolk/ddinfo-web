using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the spawnset file corresponding to the given fileName parameter. Returns to this page if the spawnset could not be found.", ReturnType = MediaTypeNames.Application.Octet)]
	public class GetSpawnsetModel : ApiPageModel
	{
		private readonly IWebHostEnvironment env;

		public GetSpawnsetModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public ActionResult OnGet(string fileName)
		{
			if (ApiFunctions.TryGetSpawnsetPath(env, fileName, out string path))
				return File(path, MediaTypeNames.Application.Octet, fileName);

			return RedirectToPage("/Api/Index");
		}
	}
}