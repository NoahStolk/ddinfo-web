using CoreBase.Services;
using DevilDaggersWebsite.Models.Spawnset;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IO;
using System;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetModel : PageModel
	{
		public string Query { get; set; }

		public string Name { get; set; }
		public string Author { get; set; }

		public string Path { get; set; }
		public Spawnset Spawnset { get; set; }

		public string Description { get; set; }

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

				Name = Query.Substring(0, Query.LastIndexOf('_'));
				Author = Query.Substring(Query.LastIndexOf('_') + 1);

				Path = System.IO.Path.Combine(CommonObjects.Env.WebRootPath, "spawnsets", $"{Name}_{Author}");
				Spawnset = SpawnsetParser.ParseFile(Path);

				Description = string.Empty;
				foreach (string settingsPath in Directory.GetFiles(System.IO.Path.Combine(CommonObjects.Env.WebRootPath, "spawnsets", "Settings")))
				{
					if (System.IO.Path.GetFileName(settingsPath.Substring(0, settingsPath.LastIndexOf('.'))) == Name)
					{
						string jsonString = System.IO.File.ReadAllText(settingsPath);
						dynamic json = JsonConvert.DeserializeObject(jsonString);

						Description = json.Description;

						break;
					}
				}
			}
			catch (Exception)
			{
				return RedirectToPage("Spawnsets");
			}

			return null;
		}
	}
}