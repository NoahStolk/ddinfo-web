using CoreBase.Services;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the spawnset corresponding to the given file parameter. Returns to this page if the spawnset could not be found.", ReturnType = MediaTypeNames.Application.Octet, IsDeprecated = true, DeprecationMessage = "Use the GetSpawnset function instead.")]
	public class DownloadSpawnsetModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public DownloadSpawnsetModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public ActionResult OnGet(string file)
		{
			if (string.IsNullOrEmpty(file) || !System.IO.File.Exists(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets", file)))
				return RedirectToPage("/API/Index");

			return File(Path.Combine("spawnsets", file), MediaTypeNames.Application.Octet, file);
		}
	}
}