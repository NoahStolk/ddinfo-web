using CoreBase.Services;
using DevilDaggersCore.Spawnset;
using DevilDaggersCore.Spawnset.Web;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetModel : PageModel
	{
		public string Query { get; private set; }
		public SpawnsetFile SpawnsetFile { get; private set; }

		public Spawnset spawnset;

		public ICommonObjects CommonObjects { get; }

		public SpawnsetModel(ICommonObjects commonObjects)
		{
			CommonObjects = commonObjects;
		}

		public ActionResult OnGet()
		{
			try
			{
				Query = HttpContext.Request.Query["spawnset"];
				SpawnsetFile = SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(CommonObjects, Path.Combine(CommonObjects.Env.WebRootPath, "spawnsets", Query));

				using (FileStream fs = new FileStream(SpawnsetFile.Path, FileMode.Open, FileAccess.Read))
					if (!Spawnset.TryParse(fs, out spawnset))
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