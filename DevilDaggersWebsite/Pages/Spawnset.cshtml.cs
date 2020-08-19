using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Code.DataTransferObjects;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetModel : PageModel
	{
		public Spawnset spawnset;

		public SpawnsetModel(IWebHostEnvironment env)
		{
			Env = env;
		}

		public IWebHostEnvironment Env { get; }

		public string Query { get; private set; }
		public SpawnsetFile? SpawnsetFile { get; private set; }

		public ActionResult? OnGet()
		{
			try
			{
				Query = HttpContext.Request.Query["spawnset"];
				SpawnsetFile = SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(Env, Path.Combine(Env.WebRootPath, "spawnsets", Query));

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