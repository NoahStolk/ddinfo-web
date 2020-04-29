using CoreBase3.Services;
using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the spawnset file corresponding to the given fileName parameter. Returns to this page if the spawnset could not be found.", ReturnType = MediaTypeNames.Application.Octet)]
	public class GetSpawnsetModel : ApiPageModel
	{
		private readonly ICommonObjects commonObjects;

		public GetSpawnsetModel(ICommonObjects commonObjects)
		{
			this.commonObjects = commonObjects;
		}

		public ActionResult OnGet(string fileName)
		{
			if (ApiFunctions.TryGetSpawnsetPath(commonObjects, fileName, out string path))
				return File(path, MediaTypeNames.Application.Octet, fileName);

			return RedirectToPage("/Api/Index");
		}
	}
}