using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Code.DataTransferObjects;
using DevilDaggersWebsite.Code.Pagination;
using DevilDaggersWebsite.Code.Transients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Pages
{
	public class SpawnsetsModel : PageModel
	{
		private readonly IWebHostEnvironment env;
		private readonly SpawnsetHelper spawnsetHelper;

		public SpawnsetsModel(IWebHostEnvironment env, SpawnsetHelper spawnsetHelper)
		{
			this.env = env;
			this.spawnsetHelper = spawnsetHelper;
		}

		public PaginatedList<SpawnsetFile> PaginatedSpawnsetFiles { get; set; }

		public string SearchAuthor { get; set; }
		public string SearchName { get; set; }

		public string NameSort { get; set; }
		public string AuthorSort { get; set; }
		public string LastUpdated { get; set; }
		public string NonLoopLength { get; set; }
		public string NonLoopSpawns { get; set; }
		public string LoopLength { get; set; }
		public string LoopSpawns { get; set; }

		public string SortOrder { get; set; }

		public int PageSize { get; set; } = 18;
		public int PageIndex { get; private set; }
		public int TotalResults { get; private set; }

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			List<SpawnsetFile> spawnsetFiles = new List<SpawnsetFile>();

			foreach (SpawnsetFile spawnset in spawnsetHelper.GetSpawnsets(SearchAuthor, SearchName))
				spawnsetFiles.Add(spawnsetHelper.CreateSpawnsetFileFromSettingsFile(Path.Combine(env.WebRootPath, "spawnsets", $"{spawnset.Name}_{spawnset.Author}")));

			NameSort = sortOrder == "Name" ? "Name_asc" : "Name";
			AuthorSort = sortOrder == "Author_asc" ? "Author" : "Author_asc";
			LastUpdated = sortOrder == "LastUpdated_asc" ? "LastUpdated" : "LastUpdated_asc";
			NonLoopLength = sortOrder == "NonLoopLength_asc" ? "NonLoopLength" : "NonLoopLength_asc";
			NonLoopSpawns = sortOrder == "NonLoopSpawns_asc" ? "NonLoopSpawns" : "NonLoopSpawns_asc";
			LoopLength = sortOrder == "LoopLength_asc" ? "LoopLength" : "LoopLength_asc";
			LoopSpawns = sortOrder == "LoopSpawns_asc" ? "LoopSpawns" : "LoopSpawns_asc";

			spawnsetFiles = sortOrder switch
			{
				"Name_asc" => spawnsetFiles.OrderBy(s => s.Name).ThenByDescending(s => s.Author).ToList(),
				"Name" => spawnsetFiles.OrderByDescending(s => s.Name).ThenBy(s => s.Author).ToList(),
				"Author_asc" => spawnsetFiles.OrderBy(s => s.Author).ThenByDescending(s => s.Name).ToList(),
				"Author" => spawnsetFiles.OrderByDescending(s => s.Author).ThenBy(s => s.Name).ToList(),
				"LastUpdated_asc" => spawnsetFiles.OrderBy(s => s.settings.LastUpdated).ThenByDescending(s => s.Name).ToList(),
				"NonLoopLength_asc" => spawnsetFiles.OrderBy(s => s.spawnsetData.NonLoopLength).ThenBy(s => s.spawnsetData.NonLoopSpawnCount).ToList(),
				"NonLoopLength" => spawnsetFiles.OrderByDescending(s => s.spawnsetData.NonLoopLength).ThenByDescending(s => s.spawnsetData.NonLoopSpawnCount).ToList(),
				"NonLoopSpawns_asc" => spawnsetFiles.OrderBy(s => s.spawnsetData.NonLoopSpawnCount).ToList(),
				"NonLoopSpawns" => spawnsetFiles.OrderByDescending(s => s.spawnsetData.NonLoopSpawnCount).ToList(),
				"LoopLength_asc" => spawnsetFiles.OrderBy(s => s.spawnsetData.LoopLength).ThenBy(s => s.spawnsetData.LoopSpawnCount).ToList(),
				"LoopLength" => spawnsetFiles.OrderByDescending(s => s.spawnsetData.LoopLength).ThenByDescending(s => s.spawnsetData.LoopSpawnCount).ToList(),
				"LoopSpawns_asc" => spawnsetFiles.OrderBy(s => s.spawnsetData.LoopSpawnCount).ToList(),
				"LoopSpawns" => spawnsetFiles.OrderByDescending(s => s.spawnsetData.LoopSpawnCount).ToList(),
				_ => spawnsetFiles.OrderByDescending(s => s.settings.LastUpdated).ThenBy(s => s.Name).ToList(),
			};
			TotalResults = spawnsetFiles.Count;
			if (TotalResults == 0)
				return;

			PageIndex = MathUtils.Clamp(PageIndex, 1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PaginatedSpawnsetFiles = PaginatedList<SpawnsetFile>.Create(spawnsetFiles, PageIndex, PageSize);
		}
	}
}