using CoreBase.Services;
using DevilDaggersCore.Spawnset;
using DevilDaggersWebsite.Models.Spawnset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetModel : PageModel
	{
		public string Query { get; private set; }
		public SpawnsetFile SpawnsetFile { get; private set; }
		public string Description { get; private set; }

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
				SpawnsetFile = new SpawnsetFile(Path.Combine(CommonObjects.Env.WebRootPath, "spawnsets", Query));

				using (FileStream fs = new FileStream(SpawnsetFile.Path, FileMode.Open, FileAccess.Read))
				{
					if (!Spawnset.TryParse(fs, out spawnset))
						return RedirectToPage("Spawnsets");
				}

				Description = string.Empty;
				foreach (string settingsPath in Directory.GetFiles(Path.Combine(CommonObjects.Env.WebRootPath, "spawnsets", "Settings")))
				{
					if (Path.GetFileName(settingsPath.Substring(0, settingsPath.LastIndexOf('.'))) == SpawnsetFile.Name)
					{
						string jsonString = System.IO.File.ReadAllText(settingsPath);
						dynamic json = JsonConvert.DeserializeObject(jsonString);

						Description = json.Description;

						break;
					}
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