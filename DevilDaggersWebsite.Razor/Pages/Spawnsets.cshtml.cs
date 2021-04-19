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
		private const string _ascendingSuffix = "_asc";

		public const string NameDesc = nameof(Name);
		public const string AuthorDesc = nameof(Author);
		public const string LastUpdatedDesc = nameof(LastUpdated);
		public const string NonLoopLengthDesc = nameof(NonLoopLength);
		public const string NonLoopSpawnsDesc = nameof(NonLoopSpawns);
		public const string LoopLengthDesc = nameof(LoopLength);
		public const string LoopSpawnsDesc = nameof(LoopSpawns);
		public const string NameAsc = nameof(Name) + _ascendingSuffix;
		public const string AuthorAsc = nameof(Author) + _ascendingSuffix;
		public const string LastUpdatedAsc = nameof(LastUpdated) + _ascendingSuffix;
		public const string NonLoopLengthAsc = nameof(NonLoopLength) + _ascendingSuffix;
		public const string NonLoopSpawnsAsc = nameof(NonLoopSpawns) + _ascendingSuffix;
		public const string LoopLengthAsc = nameof(LoopLength) + _ascendingSuffix;
		public const string LoopSpawnsAsc = nameof(LoopSpawns) + _ascendingSuffix;

		private readonly SpawnsetHelper _spawnsetHelper;

		public SpawnsetsModel(SpawnsetHelper spawnsetHelper)
		{
			_spawnsetHelper = spawnsetHelper;
		}

		public PaginatedList<SpawnsetFile> PaginatedList { get; private set; } = null!;

		public string? SearchAuthor { get; set; }
		public string? SearchName { get; set; }

		public string? Name { get; set; }
		public string? Author { get; set; }
		public string? LastUpdated { get; set; }
		public string? NonLoopLength { get; set; }
		public string? NonLoopSpawns { get; set; }
		public string? LoopLength { get; set; }
		public string? LoopSpawns { get; set; }

		public string? SortOrder { get; set; }

		public int PageSize { get; set; } = 20;
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }
		public int TotalResults { get; private set; }

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			Name = sortOrder == NameAsc ? NameDesc : NameAsc;
			Author = sortOrder == AuthorAsc ? AuthorDesc : AuthorAsc;
			LastUpdated = sortOrder == LastUpdatedAsc ? LastUpdatedDesc : LastUpdatedAsc;
			NonLoopLength = sortOrder == NonLoopLengthDesc ? NonLoopLengthAsc : NonLoopLengthDesc;
			NonLoopSpawns = sortOrder == NonLoopSpawnsDesc ? NonLoopSpawnsAsc : NonLoopSpawnsDesc;
			LoopLength = sortOrder == LoopLengthDesc ? LoopLengthAsc : LoopLengthDesc;
			LoopSpawns = sortOrder == LoopSpawnsDesc ? LoopSpawnsAsc : LoopSpawnsDesc;

			List<SpawnsetFile> spawnsetFiles = _spawnsetHelper.GetSpawnsets(SearchAuthor, SearchName);
			spawnsetFiles = (sortOrder switch
			{
				NameAsc => spawnsetFiles.OrderBy(s => s.Name).ThenByDescending(s => s.AuthorName),
				NameDesc => spawnsetFiles.OrderByDescending(s => s.Name).ThenBy(s => s.AuthorName),
				AuthorAsc => spawnsetFiles.OrderBy(s => s.AuthorName).ThenByDescending(s => s.Name),
				AuthorDesc => spawnsetFiles.OrderByDescending(s => s.AuthorName).ThenBy(s => s.Name),
				LastUpdatedAsc => spawnsetFiles.OrderBy(s => s.LastUpdated).ThenByDescending(s => s.Name),
				NonLoopLengthAsc => spawnsetFiles.OrderBy(s => s.SpawnsetData.NonLoopLength).ThenBy(s => s.SpawnsetData.NonLoopSpawnCount),
				NonLoopLengthDesc => spawnsetFiles.OrderByDescending(s => s.SpawnsetData.NonLoopLength).ThenByDescending(s => s.SpawnsetData.NonLoopSpawnCount),
				NonLoopSpawnsAsc => spawnsetFiles.OrderBy(s => s.SpawnsetData.NonLoopSpawnCount),
				NonLoopSpawnsDesc => spawnsetFiles.OrderByDescending(s => s.SpawnsetData.NonLoopSpawnCount),
				LoopLengthAsc => spawnsetFiles.OrderBy(s => s.SpawnsetData.LoopLength).ThenBy(s => s.SpawnsetData.LoopSpawnCount),
				LoopLengthDesc => spawnsetFiles.OrderByDescending(s => s.SpawnsetData.LoopLength).ThenByDescending(s => s.SpawnsetData.LoopSpawnCount),
				LoopSpawnsAsc => spawnsetFiles.OrderBy(s => s.SpawnsetData.LoopSpawnCount),
				LoopSpawnsDesc => spawnsetFiles.OrderByDescending(s => s.SpawnsetData.LoopSpawnCount),
				_ => spawnsetFiles.OrderByDescending(s => s.LastUpdated).ThenBy(s => s.Name),
			}).ToList();
			TotalResults = spawnsetFiles.Count;
			TotalPages = Math.Max(1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PageIndex = Math.Clamp(PageIndex, 1, TotalPages);
			PaginatedList = new(spawnsetFiles, PageIndex, PageSize);
		}
	}
}
