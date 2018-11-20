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

		public void OnGet(string searchAuthor, string searchName)
		{
			JsonResult = GetSpawnsets(searchAuthor, searchName);
		}

		public string GetSpawnsets(string searchAuthor, string searchName)
		{
			List<SpawnsetFile> spawnsets = new List<SpawnsetFile>();

			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets")).ToList())
			{
				SpawnsetFile sf = new SpawnsetFile(spawnsetPath);
				if (!string.IsNullOrEmpty(searchAuthor) && !sf.Author.Contains(searchAuthor) ||
					!string.IsNullOrEmpty(searchName) && !sf.Name.Contains(searchName))
					continue;
				spawnsets.Add(sf);
			}

			return JsonConvert.SerializeObject(spawnsets, Formatting.Indented);
		}
	}
}