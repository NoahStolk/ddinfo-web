using CoreBase;
using CoreBase.Services;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Utils;
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
		private readonly ApplicationDbContext _context;
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

		public SpawnsetsModel(ApplicationDbContext context, ICommonObjects commonObjects)
		{
			_context = context;
			_commonObjects = commonObjects;
		}

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			List<SpawnsetFile> spawnsetFiles = new List<SpawnsetFile>();

			foreach (SpawnsetFile spawnset in new GetSpawnsetsModel(_context, _commonObjects).GetSpawnsets(SearchAuthor, SearchName))
				spawnsetFiles.Add(SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(_context, _commonObjects, Path.Combine(_commonObjects.Env.WebRootPath, "spawnsets", $"{spawnset.Name}_{spawnset.Author}")));

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
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.spawnsetData.NonLoopLength).ThenBy(s => s.spawnsetData.NonLoopSpawns).ToList();
					break;
				case "NonLoopLength":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.spawnsetData.NonLoopLength).ThenByDescending(s => s.spawnsetData.NonLoopSpawns).ToList();
					break;
				case "NonLoopSpawns_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.spawnsetData.NonLoopSpawns).ToList();
					break;
				case "NonLoopSpawns":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.spawnsetData.NonLoopSpawns).ToList();
					break;
				case "LoopStart_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.spawnsetData.LoopStart).ThenBy(s => s.spawnsetData.LoopSpawns).ToList();
					break;
				case "LoopStart":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.spawnsetData.LoopStart).ThenByDescending(s => s.spawnsetData.LoopSpawns).ToList();
					break;
				case "LoopLength_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.spawnsetData.LoopLength).ThenBy(s => s.spawnsetData.LoopSpawns).ToList();
					break;
				case "LoopLength":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.spawnsetData.LoopLength).ThenByDescending(s => s.spawnsetData.LoopSpawns).ToList();
					break;
				case "LoopSpawns_asc":
					spawnsetFiles = spawnsetFiles.OrderBy(s => s.spawnsetData.LoopSpawns).ToList();
					break;
				case "LoopSpawns":
					spawnsetFiles = spawnsetFiles.OrderByDescending(s => s.spawnsetData.LoopSpawns).ToList();
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