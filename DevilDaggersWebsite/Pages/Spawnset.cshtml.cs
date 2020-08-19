using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Code.DataTransferObjects;
using DevilDaggersWebsite.Code.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetModel : PageModel
	{
		public Spawnset spawnset;

		private readonly SpawnsetHelper spawnsetHelper;
		private readonly IWebHostEnvironment env;

		public SpawnsetModel(SpawnsetHelper spawnsetHelper, IWebHostEnvironment env)
		{
			this.spawnsetHelper = spawnsetHelper;
			this.env = env;
		}

		public string? Query { get; private set; }
		public SpawnsetFile? SpawnsetFile { get; private set; }

		public ActionResult? OnGet()
		{
			try
			{
				Query = HttpContext.Request.Query["spawnset"];
				SpawnsetFile = spawnsetHelper.CreateSpawnsetFileFromSettingsFile(Path.Combine(env.WebRootPath, "spawnsets", Query));

				if (!Spawnset.TryParse(System.IO.File.ReadAllBytes(SpawnsetFile?.Path), out spawnset))
					return RedirectToPage("Spawnsets");

				return null;
			}
			catch
			{
				return RedirectToPage("Spawnsets");
			}
		}
	}
}