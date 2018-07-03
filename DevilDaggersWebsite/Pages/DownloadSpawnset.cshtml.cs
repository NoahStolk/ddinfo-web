using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages
{
	public class DownloadSpawnsetModel : PageModel
	{
		public ActionResult OnGet(string file)
		{
			return File(Path.Combine("spawnsets", file), MediaTypeNames.Application.Octet, file);
		}
	}
}