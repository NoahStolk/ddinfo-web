using CoreBase.Services;
using DevilDaggersWebsite.Models.Spawnset;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class GetSpawnsetsModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public string JsonResult { get; set; }

		public GetSpawnsetsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet()
		{
			JsonResult = GetSpawnsets();
		}

		public string GetSpawnsets()
		{
			List<SpawnsetFile> spawnsets = new List<SpawnsetFile>();

			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets/")).ToList())
			{
				spawnsets.Add(new SpawnsetFile(spawnsetPath));
			}

			return JsonConvert.SerializeObject(spawnsets, Formatting.Indented);
		}
	}
}