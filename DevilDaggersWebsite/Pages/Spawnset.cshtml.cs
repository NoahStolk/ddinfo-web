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
		public SpawnsetFile SpawnsetFile { get; private set; }

		public ActionResult OnGet()
		{
			try
			{
				Query = HttpContext.Request.Query["spawnset"];
				SpawnsetFile = SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(Env, Path.Combine(Env.WebRootPath, "spawnsets", Query));

				using (FileStream fs = new FileStream(SpawnsetFile.Path, FileMode.Open, FileAccess.Read))
				{
					if (!Spawnset.TryParse(fs, out spawnset))
						return RedirectToPage("Spawnsets");
				}

				return null;
			}
			catch
			{
				return RedirectToPage("Spawnsets");
			}
		}
	}
}