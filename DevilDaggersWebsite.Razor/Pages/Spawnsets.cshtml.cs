using DevilDaggersWebsite.Core.Dto;
using DevilDaggersWebsite.Core.Transients;
using DevilDaggersWebsite.Razor.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class SpawnsetsModel : PageModel
	{
		private readonly SpawnsetHelper _spawnsetHelper;

		public SpawnsetsModel(SpawnsetHelper spawnsetHelper)
		{
			_spawnsetHelper = spawnsetHelper;
		}

		public PaginatedList<SpawnsetFile>? PaginatedSpawnsetFiles { get; private set; }

		public string? SearchAuthor { get; set; }
		public string? SearchName { get; set; }

		public string? NameSort { get; set; }
		public string? AuthorSort { get; set; }
		public string? LastUpdated { get; set; }
		public string? NonLoopLength { get; set; }
		public string? NonLoopSpawns { get; set; }
		public string? LoopLength { get; set; }
		public string? LoopSpawns { get; set; }

		public string? SortOrder { get; set; }

		public int PageSize { get; set; } = 18;
		public int PageIndex { get; private set; }
		public int TotalResults { get; private set; }

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			NameSort = sortOrder == "Name" ? "Name_asc" : "Name";
			AuthorSort = sortOrder == "Author_asc" ? "Author" : "Author_asc";
			LastUpdated = sortOrder == "LastUpdated_asc" ? "LastUpdated" : "LastUpdated_asc";
			NonLoopLength = sortOrder == "NonLoopLength_asc" ? "NonLoopLength" : "NonLoopLength_asc";
			NonLoopSpawns = sortOrder == "NonLoopSpawns_asc" ? "NonLoopSpawns" : "NonLoopSpawns_asc";
			LoopLength = sortOrder == "LoopLength_asc" ? "LoopLength" : "LoopLength_asc";
			LoopSpawns = sortOrder == "LoopSpawns_asc" ? "LoopSpawns" : "LoopSpawns_asc";

			List<SpawnsetFile> spawnsetFiles = _spawnsetHelper.GetSpawnsets(SearchAuthor, SearchName);
			spawnsetFiles = sortOrder switch
			{
				"Name_asc" => spawnsetFiles.OrderBy(s => s.Name).ThenByDescending(s => s.AuthorName).ToList(),
				"Name" => spawnsetFiles.OrderByDescending(s => s.Name).ThenBy(s => s.AuthorName).ToList(),
				"Author_asc" => spawnsetFiles.OrderBy(s => s.AuthorName).ThenByDescending(s => s.Name).ToList(),
				"Author" => spawnsetFiles.OrderByDescending(s => s.AuthorName).ThenBy(s => s.Name).ToList(),
				"LastUpdated_asc" => spawnsetFiles.OrderBy(s => s.LastUpdated).ThenByDescending(s => s.Name).ToList(),
				"NonLoopLength_asc" => spawnsetFiles.OrderBy(s => s.SpawnsetData.NonLoopLength).ThenBy(s => s.SpawnsetData.NonLoopSpawnCount).ToList(),
				"NonLoopLength" => spawnsetFiles.OrderByDescending(s => s.SpawnsetData.NonLoopLength).ThenByDescending(s => s.SpawnsetData.NonLoopSpawnCount).ToList(),
				"NonLoopSpawns_asc" => spawnsetFiles.OrderBy(s => s.SpawnsetData.NonLoopSpawnCount).ToList(),
				"NonLoopSpawns" => spawnsetFiles.OrderByDescending(s => s.SpawnsetData.NonLoopSpawnCount).ToList(),
				"LoopLength_asc" => spawnsetFiles.OrderBy(s => s.SpawnsetData.LoopLength).ThenBy(s => s.SpawnsetData.LoopSpawnCount).ToList(),
				"LoopLength" => spawnsetFiles.OrderByDescending(s => s.SpawnsetData.LoopLength).ThenByDescending(s => s.SpawnsetData.LoopSpawnCount).ToList(),
				"LoopSpawns_asc" => spawnsetFiles.OrderBy(s => s.SpawnsetData.LoopSpawnCount).ToList(),
				"LoopSpawns" => spawnsetFiles.OrderByDescending(s => s.SpawnsetData.LoopSpawnCount).ToList(),
				_ => spawnsetFiles.OrderByDescending(s => s.LastUpdated).ThenBy(s => s.Name).ToList(),
			};
			TotalResults = spawnsetFiles.Count;
			if (TotalResults == 0)
				return;

			PageIndex = Math.Clamp(PageIndex, 1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PaginatedSpawnsetFiles = new PaginatedList<SpawnsetFile>(spawnsetFiles, PageIndex, PageSize);
		}
	}
}