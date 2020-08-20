using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace DevilDaggersWebsite.Pages.Admin.AdminTests
{
	public class RenameSpawnsetFilesModel : PageModel
	{
		private readonly IWebHostEnvironment env;

		public RenameSpawnsetFilesModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public void OnGet()
		{
			foreach (string path in Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")))
			{
				if (!path.Contains('_', System.StringComparison.InvariantCulture))
					continue;

				string newPath = path.Substring(0, path.LastIndexOf('_'));
				System.IO.File.Move(path, newPath);
			}
		}
	}
}