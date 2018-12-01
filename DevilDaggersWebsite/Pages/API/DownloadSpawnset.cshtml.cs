using DevilDaggersWebsite.Models.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the spawnset file corresponding to the given file parameter (file name).", ReturnType = MediaTypeNames.Application.Octet)]
	public class DownloadSpawnsetModel : PageModel
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