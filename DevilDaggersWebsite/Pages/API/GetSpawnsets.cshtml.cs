using CoreBase.Services;
using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersWebsite.Code.Utils;

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

		public FileResult OnGet(string searchAuthor = "", string searchName = "", bool formatted = false)
		{
			return JsonFile(GetSpawnsets(searchAuthor, searchName), formatted ? Formatting.Indented : Formatting.None);
		}

		public List<SpawnsetFile> GetSpawnsets(string searchAuthor = "", string searchName = "")
		{
			List<SpawnsetFile> spawnsets = new List<SpawnsetFile>();

			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets")))
			{
				SpawnsetFile sf = SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(_commonObjects, spawnsetPath);
				if (!string.IsNullOrEmpty(searchAuthor) && !sf.Author.ToLower().Contains(searchAuthor.ToLower()) ||
					!string.IsNullOrEmpty(searchName) && !sf.Name.ToLower().Contains(searchName.ToLower()))
					continue;
				if (sf != null)
					spawnsets.Add(sf);
			}

			return spawnsets;
		}
	}
}