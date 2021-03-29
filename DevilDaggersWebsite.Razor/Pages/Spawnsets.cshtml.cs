using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Razor.Pagination;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class SpawnsetsModel : PageModel, IPaginationModel
	{
		private readonly SpawnsetHelper _spawnsetHelper;

		public SpawnsetsModel(SpawnsetHelper spawnsetHelper)
		{
			_spawnsetHelper = spawnsetHelper;
		}

		public PaginatedList<SpawnsetFile> PaginatedList { get; private set; } = null!;

		public string? SearchAuthor { get; set; }
		public string? SearchName { get; set; }

		public string? NameSort { get; set; }
		public string? AuthorSort { get; set; }
		public string? LastUpdatedSort { get; set; }
		public string? NonLoopLengthSort { get; set; }
		public string? NonLoopSpawnsSort { get; set; }
		public string? LoopLengthSort { get; set; }
		public string? LoopSpawnsSort { get; set; }

		public string? SortOrder { get; set; }

		public int PageSize { get; set; } = 20;
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }
		public int TotalResults { get; private set; }

		public string PageName => "Spawnsets";

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			NameSort = sortOrder == "Name" ? "Name_asc" : "Name";
			AuthorSort = sortOrder == "Author_asc" ? "Author" : "Author_asc";
			LastUpdatedSort = sortOrder == "LastUpdated_asc" ? "LastUpdated" : "LastUpdated_asc";
			NonLoopLengthSort = sortOrder == "NonLoopLength_asc" ? "NonLoopLength" : "NonLoopLength_asc";
			NonLoopSpawnsSort = sortOrder == "NonLoopSpawns_asc" ? "NonLoopSpawns" : "NonLoopSpawns_asc";
			LoopLengthSort = sortOrder == "LoopLength_asc" ? "LoopLength" : "LoopLength_asc";
			LoopSpawnsSort = sortOrder == "LoopSpawns_asc" ? "LoopSpawns" : "LoopSpawns_asc";

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
			TotalPages = Math.Max(1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PageIndex = Math.Clamp(PageIndex, 1, TotalPages);
			PaginatedList = new(spawnsetFiles, PageIndex, PageSize);
		}
	}
}
