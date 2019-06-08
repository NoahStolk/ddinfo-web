using CoreBase.Services;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the spawnset file corresponding to the given fileName parameter. Returns to this page if the spawnset could not be found.", ReturnType = MediaTypeNames.Application.Octet)]
	public class GetSpawnsetModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetSpawnsetModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public ActionResult OnGet(string fileName)
		{
			if (string.IsNullOrEmpty(fileName) || !System.IO.File.Exists(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets", fileName)))
				return RedirectToPage("/API/Index");
				
			return File(Path.Combine("spawnsets", fileName), MediaTypeNames.Application.Octet, fileName);
		}
	}
}