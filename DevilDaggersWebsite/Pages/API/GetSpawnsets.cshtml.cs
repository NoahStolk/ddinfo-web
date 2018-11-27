using CoreBase.Services;
using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Models.Spawnset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace DevilDaggersWebsite.Pages.API
{
	[Api(ApiReturnType = MediaTypeNames.Application.Json)]
	public class GetSpawnsetsModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetSpawnsetsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet(string searchAuthor = null, string searchName = null)
		{
			string jsonResult = GetSpawnsets(searchAuthor, searchName);
			return File(Encoding.UTF8.GetBytes(jsonResult), MediaTypeNames.Application.Json, $"{GetType().Name.Replace("Model", "")}.json");
		}

		public string GetSpawnsets(string searchAuthor, string searchName)
		{
			List<SpawnsetFile> spawnsets = new List<SpawnsetFile>();

			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets")).ToList())
			{
				SpawnsetFile sf = new SpawnsetFile(spawnsetPath);
				if (!string.IsNullOrEmpty(searchAuthor) && !sf.Author.ToLower().Contains(searchAuthor.ToLower()) ||
					!string.IsNullOrEmpty(searchName) && !sf.Name.ToLower().Contains(searchName.ToLower()))
					continue;
				spawnsets.Add(sf);
			}

			return JsonConvert.SerializeObject(spawnsets, Formatting.Indented);
		}
	}
}