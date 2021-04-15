using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Razor.PageModels;
using DevilDaggersWebsite.Razor.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevilDaggersWebsite.Razor.Pages
{
	public class ModsModel : PageModel, IPaginationModel
	{
		private readonly ApplicationDbContext _dbContext;

		public ModsModel(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public PaginatedList<Mod> PaginatedList { get; private set; } = null!;

		public string? SearchAuthor { get; set; }
		public string? SearchName { get; set; }

		public string? NameSort { get; set; }
		public string? AuthorSort { get; set; }
		public string? TypeSort { get; set; }

		public string? SortOrder { get; set; }

		public int PageSize { get; set; } = 20;
		public int PageIndex { get; private set; }
		public int TotalPages { get; private set; }
		public int TotalResults { get; private set; }

		public string PageName => "Mods";

		public void OnGet(string searchAuthor, string searchName, string sortOrder, int? pageIndex)
		{
			SearchAuthor = searchAuthor;
			SearchName = searchName;
			SortOrder = sortOrder;
			PageIndex = pageIndex ?? 1;

			NameSort = sortOrder == "Name" ? "Name_asc" : "Name";
			AuthorSort = sortOrder == "Author_asc" ? "Author" : "Author_asc";
			TypeSort = sortOrder == "TypeSort_asc" ? "TypeSort" : "TypeSort_asc";

			List<Mod> sortedMods = new();
			foreach (AssetMod assetMod in _dbContext.AssetMods.Include(am => am.PlayerAssetMods).ThenInclude(pam => pam.Player).Where(am => !am.IsHidden))
				sortedMods.Add(new(assetMod.AssetModTypes, assetMod.Name, assetMod.PlayerAssetMods.Select(pam => pam.Player.PlayerName).OrderBy(s => s).ToList()));

			if (!string.IsNullOrWhiteSpace(SearchAuthor))
				sortedMods = sortedMods.Where(am => am.Authors.Any(a => a.Contains(SearchAuthor, StringComparison.InvariantCultureIgnoreCase))).ToList();

			if (!string.IsNullOrWhiteSpace(SearchName))
				sortedMods = sortedMods.Where(am => am.Name.Contains(SearchName, StringComparison.InvariantCultureIgnoreCase)).ToList();

			sortedMods = sortOrder switch
			{
				"Name" => sortedMods.OrderByDescending(s => s.Name).ThenBy(s => s.Authors[0]).ToList(),
				"Author_asc" => sortedMods.OrderBy(s => s.Authors[0]).ThenByDescending(s => s.Name).ToList(),
				"Author" => sortedMods.OrderByDescending(s => s.Authors[0]).ThenBy(s => s.Name).ToList(),
				"TypeSort_asc" => sortedMods.OrderBy(s => s.AssetModTypes).ThenByDescending(s => s.Name).ToList(),
				"TypeSort" => sortedMods.OrderByDescending(s => s.AssetModTypes).ThenBy(s => s.Name).ToList(),
				_ => sortedMods.OrderBy(s => s.Name).ThenByDescending(s => s.Authors[0]).ToList(),
			};
			TotalResults = sortedMods.Count;
			TotalPages = Math.Max(1, (int)Math.Ceiling(TotalResults / (double)PageSize));
			PageIndex = Math.Clamp(PageIndex, 1, TotalPages);
			PaginatedList = new(sortedMods, PageIndex, PageSize);
		}
	}
}
