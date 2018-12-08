using CoreBase.Services;
using DevilDaggersWebsite.Models.API;
using DevilDaggersWebsite.Models.Spawnset;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.API
{
	[ApiFunction(Description = "Returns the list of available spawnsets on the site. Optional filtering can be done using the searchAuthor and searchName parameters.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetSpawnsetsModel : ApiPageModel
	{
		private readonly ICommonObjects _commonObjects;

		public GetSpawnsetsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public FileResult OnGet(string searchAuthor = null, string searchName = null)
		{
			return JsonFile(GetSpawnsets(searchAuthor, searchName));
		}

		public List<SpawnsetFile> GetSpawnsets(string searchAuthor = null, string searchName = null)
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

			return spawnsets;
		}
	}
}