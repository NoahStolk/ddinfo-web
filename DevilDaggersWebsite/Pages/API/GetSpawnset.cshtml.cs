using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the spawnset file corresponding to the given fileName parameter. Returns to this page if the spawnset could not be found.", ReturnType = MediaTypeNames.Application.Octet)]
	public class GetSpawnsetModel : ApiPageModel
	{
		public ActionResult OnGet(string fileName)
		{
			try
			{
				return File(Path.Combine("spawnsets", fileName), MediaTypeNames.Application.Octet, fileName);
			}
			catch
			{
				return RedirectToPage("/API/Index");
			}
		}
	}
}