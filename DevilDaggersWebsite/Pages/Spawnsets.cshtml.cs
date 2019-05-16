using CoreBase;
using CoreBase.Services;
using DevilDaggersWebsite.Models.Spawnset;
using DevilDaggersWebsite.Pages.API;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetsModel : PageModel
	{
		private readonly ICommonObjects _commonObjects;

		public PaginatedList<SpawnsetFile> PaginatedSpawnsetFiles;

		public string SearchAuthor { get; set; }
		public string SearchName { get; set; }

		public string NameSort { get; set; }
		public string AuthorSort { get; set; }
		public string LastUpdated { get; set; }
		public string NonLoopLength { get; set; }
		public string NonLoopSpawns { get; set; }
		public string LoopStart { get; set; }
		public string LoopLength { get; set; }
		public string LoopSpawns { get; set; }

		public string SortOrder { get; set; }

		public int PageSize { get; set; } = 15;
		public int PageIndex { get; private set; }
		public int TotalResults { get; private set; }

		public SpawnsetsModel(ICommonObjects commonObjects)
		{
			_commonObjects = commonObjects;
		}

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			//Dictionary<string, SpawnsetFileSettings> dict = new Dictionary<string, SpawnsetFileSettings>();
			//foreach (string path in Directory.GetFiles(Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets"), "*_*", SearchOption.TopDirectoryOnly))
			//{
			//	string fileName = Path.GetFileName(path);
			//	SpawnsetFile sf = new SpawnsetFile(_commonObjects, path);
			//	sf.Settings.LastUpdated = System.IO.File.GetLastWriteTime(path);

			//	dict[fileName] = sf.Settings;
			//}

			//JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings()
			//{
			//	DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
			//});
			//using (StreamWriter sw = new StreamWriter(System.IO.File.Create(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\spawnsets\Settings/Settings.json")))
			//using (JsonTextWriter jtw = new JsonTextWriter(sw) { Formatting = Formatting.Indented, IndentChar = '\t', Indentation = 1 })
			//	serializer.Serialize(jtw, dict);

			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			List<SpawnsetFile> spawnsetFiles = new List<SpawnsetFile>();

			string str = JsonConvert.SerializeObject(new GetSpawnsetsModel(_commonObjects).GetSpawnsets(SearchAuthor, SearchName));
			dynamic json = JsonConvert.DeserializeObject(str);
			foreach (dynamic spawnset in json)
				spawnsetFiles.Add(new SpawnsetFile(_commonObjects, Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets", $"{spawnset.Name}_{spawnset.Author}")));

			NameSort = sortOrder == "Name" ? "Name_asc" : "Name";
			AuthorSort = sortOrder == "Author_asc" ? "Author" : "Author_asc";
			LastUpdated = sortOrder == "LastUpdated_asc" ? "LastUpdated" : "LastUpdated_asc";
			NonLoopLength = sortOrder == "NonLoopLength_asc" ? "NonLoopLength" : "NonLoopLength_asc";
			NonLoopSpawns = sortOrder == "NonLoopSpawns_asc" ? "NonLoopSpawns" : "NonLoopSpawns_asc";
			LoopStart = sortOrder == "LoopStart_asc" ? "LoopStart" : "LoopStart_asc";
			LoopLength = sortOrder == "LoopLength_asc" ? "LoopLength" : "LoopLength_asc";
			LoopSpawns = sortOrder == "LoopSpawns_asc" ? "LoopSpawns" : "LoopSpawns_asc";

			switch (sortOrder)
			{
				case "Name_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.Name).ThenByDescending(s => s.Author).ToList();
					break;
				case "Name":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.Name).ThenBy(s => s.Author).ToList();
					break;
				case "Author_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.Author).ThenByDescending(s => s.Name).ToList();
					break;
				case "Author":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.Author).ThenBy(s => s.Name).ToList();
					break;
				case "LastUpdated_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.settings.LastUpdated).ThenByDescending(s => s.Name).ToList();
					break;
				default:
				case "LastUpdated":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.settings.LastUpdated).ThenBy(s => s.Name).ToList();
					break;
				case "NonLoopLength_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.GetSpawnsetData().NonLoopLength).ThenBy(s => s.GetSpawnsetData().NonLoopSpawns).ToList();
					break;
				case "NonLoopLength":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.GetSpawnsetData().NonLoopLength).ThenByDescending(s => s.GetSpawnsetData().NonLoopSpawns).ToList();
					break;
				case "NonLoopSpawns_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.GetSpawnsetData().NonLoopSpawns).ToList();
					break;
				case "NonLoopSpawns":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.GetSpawnsetData().NonLoopSpawns).ToList();
					break;
				case "LoopStart_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.GetSpawnsetData().LoopStart).ThenBy(s => s.GetSpawnsetData().LoopSpawns).ToList();
					break;
				case "LoopStart":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.GetSpawnsetData().LoopStart).ThenByDescending(s => s.GetSpawnsetData().LoopSpawns).ToList();
					break;
				case "LoopLength_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.GetSpawnsetData().LoopLength).ThenBy(s => s.GetSpawnsetData().LoopSpawns).ToList();
					break;
				case "LoopLength":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.GetSpawnsetData().LoopLength).ThenByDescending(s => s.GetSpawnsetData().LoopSpawns).ToList();
					break;
				case "LoopSpawns_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.GetSpawnsetData().LoopSpawns).ToList();
					break;
				case "LoopSpawns":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.GetSpawnsetData().LoopSpawns).ToList();
					break;
			}

			TotalResults = spawnsetFiles.Count;
			if (TotalResults == 0)
				return;

			PageIndex = MathUtils.Clamp(PageIndex, 1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PaginatedSpawnsetFiles = PaginatedList<SpawnsetFile>.Create(spawnsetFiles, PageIndex, PageSize);
		}
	}
}