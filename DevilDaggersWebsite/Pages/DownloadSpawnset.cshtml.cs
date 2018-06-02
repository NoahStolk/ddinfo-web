using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevilDaggersWebsite.Pages
{
	public class DownloadSpawnsetModel : PageModel
    {
		private IHostingEnvironment _env;

		public DownloadSpawnsetModel(IHostingEnvironment env)
		{
			_env = env;
		}

		public ActionResult OnGet(string file)
		{
			return File(Path.Combine(_env.WebRootPath, "spawnsets", file), System.Net.Mime.MediaTypeNames.Application.Octet, file);
		}
	}
}