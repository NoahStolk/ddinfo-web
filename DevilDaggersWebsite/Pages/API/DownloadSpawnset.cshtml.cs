using DevilDaggersWebsite.Models.API;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the spawnset corresponding to the given file parameter.", ReturnType = MediaTypeNames.Application.Octet, IsDeprecated = true, DeprecationMessage = "Use the GetSpawnset function instead.")]
	public class DownloadSpawnsetModel : ApiPageModel
	{
		public ActionResult OnGet(string file)
		{
			try
			{
				return File(Path.Combine("spawnsets", file), MediaTypeNames.Application.Octet, file);
			}
			catch
			{
				return RedirectToPage("/API/Index");
			}
		}
	}
}